using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _moverMonoBehaviour;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;
    [SerializeField] private float _distanceForWalk;

    private Vector2 _directionMove;
    private IMover _mover;
    private bool _isRun;

    private void OnValidate()
    {
        if (_distanceForWalk < _distance)
        {
            _distanceForWalk = _distance;
        }

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

    private void Update()
    {
        float distanceSqr = _distance * _distance;
        float distanceForWalkSqr = _distanceForWalk * _distanceForWalk;
        Vector3 direction = _target.position - transform.position;

        if (direction.sqrMagnitude < distanceForWalkSqr)
        {
            _isRun = false;
        }
        else
        {
            _isRun = true;
        }

        if (direction.sqrMagnitude < distanceSqr)
        {
            _directionMove = Vector2.zero;
            return;
        }

        direction.Normalize();
        _directionMove = new Vector2(direction.x, direction.z);
    }

    private void FixedUpdate()
    {
        _mover.Move(_directionMove);
        _mover.Run(_isRun);
    }
}