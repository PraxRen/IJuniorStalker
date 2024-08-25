using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _moverMonoBehaviour;
    [SerializeField] private float _force;
    [SerializeField] private float _horizontalFactor;

    private IMover _mover;

    private void OnValidate()
    {
        if (_moverMonoBehaviour == null)
        {
            return;
        }

        _mover = _moverMonoBehaviour as IMover;

        if (_mover == null)
        {
            _moverMonoBehaviour = null;
            Debug.LogWarning($"_moverMonoBehaviour is not IMover");
        }
    }

    public bool CanActivate()
    {
        return _mover.IsGrounded;
    }

    public void Activate()
    {
        Vector3 direction = _mover.HorizontalVelocity * _horizontalFactor + Vector3.up;
        float force = _force + Mathf.Abs(_mover.VerticalVelocity);
        _mover.AddForce(direction, force);
    }
}
