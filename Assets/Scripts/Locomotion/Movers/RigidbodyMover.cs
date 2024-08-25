using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class RigidbodyMover : BaseMover
{
    [SerializeField] private float _slopeLimit;

    [Header("Step Up Stairs")]
    [SerializeField] private Vector3 _stepOffsetLow; //0.31
    [SerializeField] private float _stepYOffsetUp; //0.3
    [SerializeField] private float _stepUpForce;
    [SerializeField] private LayerMask _stairsLayerMask;
    [SerializeField] private Vector3 _sizeBoxRayStairs;

#if UNITY_EDITOR
    [Header("Gizmos Collision Stairs")]
    [SerializeField] private Color _colorIsCollisionStair;
    [SerializeField] private Color _colorIsNotCollisionStair;
#endif

    private CapsuleCollider _capsuleCollider;
    private Rigidbody _rigidbody;
    private Vector3 _stepe;
    private bool _isCollisionStairLow;
    private bool _isCollisionStairUp;

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelectedAddon()
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.color = _isCollisionStairLow ? _colorIsCollisionStair : _colorIsNotCollisionStair;
        Gizmos.DrawCube(GetLocalPositionStepLow(), _sizeBoxRayStairs);
        Gizmos.color = _isCollisionStairUp ? _colorIsCollisionStair : _colorIsNotCollisionStair;
        Gizmos.DrawCube(GetLocalPositionStepUp(), _sizeBoxRayStairs);
        Gizmos.matrix = originalMatrix;
    }
#endif

    protected override void AwakeAddon()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void FixedUpdateAddon()
    {
        UpdateStep();
    }

    protected override float GetHeightCollider()
    {
        return _capsuleCollider.height;
    }

    protected override float GetOffsetAngelAxisY()
    {
        return 0f;
    }

    protected override float GetSlopeLimit()
    {
        return _slopeLimit;
    }

    protected override void HandleMove()
    {
        Vector3 velocity = new Vector3(HorizontalVelocity.x, VerticalVelocity, HorizontalVelocity.z) + Force + Slope + _stepe;
        _rigidbody.velocity = velocity;
    }

    private void UpdateStep()
    {
        _stepe = Vector3.zero;

        if (IsGrounded == false)
        {
            _isCollisionStairLow = false;
            _isCollisionStairUp = false;
            return;
        }

        if (InputDirectionMove == Vector2.zero)
        {
            _isCollisionStairLow = false;
            _isCollisionStairUp = false;
            return;
        }

        Vector3 positionRaycast = transform.rotation * GetLocalPositionStepLow() + transform.position;

        if (HasCollisionStair(positionRaycast) == false)
        {
            _isCollisionStairLow = false;
            _isCollisionStairUp = false;
            return;
        }

        _isCollisionStairLow = true;
        positionRaycast = transform.rotation * GetLocalPositionStepUp() + transform.position;

        if (HasCollisionStair(positionRaycast))
        {
            _isCollisionStairUp = true;
            return;
        }

        _isCollisionStairUp = false;
        _stepe.y = _stepUpForce;
    }

    private bool HasCollisionStair(Vector3 positionRaycast)
    {
        return Physics.CheckBox(positionRaycast, _sizeBoxRayStairs, transform.rotation, _stairsLayerMask, QueryTriggerInteraction.Ignore);
    }

    private Vector3 GetLocalPositionStepLow()
    {
        return _stepOffsetLow;
    }

    private Vector3 GetLocalPositionStepUp()
    {
        return new Vector3(_stepOffsetLow.x, _stepOffsetLow.y + _stepYOffsetUp + _sizeBoxRayStairs.y, _stepOffsetLow.z);
    }
}