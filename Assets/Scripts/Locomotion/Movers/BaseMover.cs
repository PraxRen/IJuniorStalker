using UnityEngine;

public abstract class BaseMover : MonoBehaviour, IMover
{
    [Header("Speed")]
    [SerializeField] private float _speedWalk;
    [SerializeField] private float _speedRun;
    [SerializeField] private float _speedChangeRate;
    [SerializeField] private float _offsetSpeed;

    [Header("Rotation")]
    [SerializeField][Range(0.0f, 0.3f)] private float _rotationSmoothTime;

    [Header("Collision Ground")]
    [SerializeField] private float _groundRaycastOffset;
    [SerializeField] private float _groundRaycastRadius;
    [SerializeField] private LayerMask _groundLayers;

#if UNITY_EDITOR
    [Header("Gizmos Collision Ground")]
    [SerializeField] private Color _colorIsGrounded;
    [SerializeField] private Color _colorIsNotGrounded;
#endif

    [Header("Vertical Velocity")]
    [SerializeField] private float _gravityVerticalForce;
    [SerializeField] private float _alwayVerticalVelocity;
    [SerializeField] private float _terminalVerticalVelocity;

    [Header("Force")]
    [SerializeField] private float _speedChangeForce;
    [SerializeField] private float _maxValueForce;

    [Header("Slope")]
    [SerializeField] private float _slopeForce;

    private float _rotationVelocity;
    private RaycastHit _hitGround;
    private Vector3 _slope;

    public Vector2 InputDirectionMove { get; private set; }
    public Vector2 InputDirectionLook { get; private set; }
    public Vector3 HorizontalVelocity { get; private set; }
    public Vector3 Force { get; private set; }
    public Vector3 Slope => _slope;
    public float VerticalVelocity { get; private set; }
    public float AlwayVerticalVelocity => _alwayVerticalVelocity;
    public float AngelAxisY { get; private set; }
    public float Speed { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsGrounded { get; private set; }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? _colorIsGrounded : _colorIsNotGrounded;
        Vector3 centerSphere = new Vector3(transform.position.x, transform.position.y - _groundRaycastOffset, transform.position.z);
        Gizmos.DrawSphere(centerSphere, _groundRaycastRadius);
        OnDrawGizmosSelectedAddon();
    }
#endif

    private void Awake()
    {
        VerticalVelocity = _alwayVerticalVelocity;
        AwakeAddon();
    }

    private void FixedUpdate()
    {
        UpdateCollisionGround();
        UpdateAngelAxisY();
        UpdateSpeed();
        UpdateForce();
        UpdateSlope();
        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();
        HandleRotation();
        HandleMove();
        FixedUpdateAddon();
    }

    public void Move(Vector2 direction)
    {
        InputDirectionMove = direction.normalized;
    }

    public void Look(Vector2 direction)
    {
        InputDirectionLook = direction.normalized;
    }

    public void Run(bool value)
    {
        IsRun = value;
    }

    public void AddForce(Vector3 direction, float force)
    {
        float minValueForce = 0f;
        Force = direction.normalized * Mathf.Clamp(force, minValueForce, _maxValueForce);
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelectedAddon() { }
#endif

    protected virtual void AwakeAddon() { }

    protected virtual void FixedUpdateAddon() { }

    protected virtual void HandleRotation()
    {
        if (InputDirectionMove == Vector2.zero)
        {
            return;
        }

        float currentAngelAxisY = Mathf.SmoothDampAngle(transform.eulerAngles.y, AngelAxisY, ref _rotationVelocity, _rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, currentAngelAxisY, 0f);
    }

    protected virtual void HandleMove()
    {
        Vector3 velocity = new Vector3(HorizontalVelocity.x, VerticalVelocity, HorizontalVelocity.z) + Force + Slope;
        transform.position += velocity;
    }

    protected abstract float GetOffsetAngelAxisY();

    protected abstract float GetSlopeLimit();

    protected abstract float GetHeightCollider();

    private void UpdateCollisionGround()
    {
        float halfHeight = GetHeightCollider() / 2;
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + halfHeight, transform.position.z);
        float distance = halfHeight + _groundRaycastOffset;
        IsGrounded = Physics.SphereCast(spherePosition, _groundRaycastRadius, Vector3.down, out _hitGround, distance, _groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void UpdateAngelAxisY()
    {
        if (InputDirectionMove == Vector2.zero)
        {
            AngelAxisY = transform.eulerAngles.y;
            return;
        }

        AngelAxisY = Mathf.Atan2(InputDirectionMove.x, InputDirectionMove.y) * Mathf.Rad2Deg + GetOffsetAngelAxisY();
    }

    private void UpdateSpeed()
    {
        float targetSpeed = 0f;

        if (InputDirectionMove != Vector2.zero)
        {
            targetSpeed = IsRun ? _speedRun : _speedWalk;
        }

        if (Speed < targetSpeed - _offsetSpeed || Speed > targetSpeed + _offsetSpeed)
        {
            Speed = Mathf.Lerp(Speed, targetSpeed, Time.fixedDeltaTime * _speedChangeRate);
        }
        else
        {
            Speed = targetSpeed;
        }
    }

    private void UpdateForce()
    {
        Force = Vector3.Lerp(Force, Vector3.zero, Time.fixedDeltaTime * _speedChangeForce);
    }

    private void UpdateSlope()
    {
        if (IsGrounded == false)
        {
            _slope = Vector3.zero;
            return;
        }

        Vector3 groundNormal = _hitGround.normal;

        if (Vector3.Angle(groundNormal, Vector3.up) <= GetSlopeLimit())
        {
            _slope = Vector3.zero;
            return;
        }


        _slope.x += (1f - groundNormal.y) * groundNormal.x * _slopeForce;
        _slope.z += (1f - groundNormal.y) * groundNormal.z * _slopeForce;
        _slope.y -= _slopeForce;
    }

    private void UpdateVerticalVelocity()
    {
        if (IsGrounded)
        {
            VerticalVelocity = _alwayVerticalVelocity;
            return;
        }

        if (Mathf.Abs(VerticalVelocity) < Mathf.Abs(_terminalVerticalVelocity))
        {
            VerticalVelocity += _gravityVerticalForce * Time.fixedDeltaTime;
        }
    }

    private void UpdateHorizontalVelocity()
    {
        if(IsGrounded == false)
        {
            HorizontalVelocity = Vector3.zero;
        }

        Vector3 directionHorizontalMove = (Quaternion.Euler(0.0f, AngelAxisY, 0.0f) * Vector3.forward).normalized * Speed;
        HorizontalVelocity = directionHorizontalMove * Time.fixedDeltaTime;
    }
}