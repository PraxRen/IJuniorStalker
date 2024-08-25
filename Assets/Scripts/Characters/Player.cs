using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private MonoBehaviour _moverMonoBehaviour;
    [SerializeField] private Jump _jump;

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

    private void OnEnable()
    {
        _inputReader.Run += OnRun;
        _inputReader.Jump += OnJump;
    }

    private void OnDisable()
    {
        _inputReader.Run -= OnRun;
        _inputReader.Jump -= OnJump;
    }

    private void Update()
    {
        _mover.Move(_inputReader.DirectionMove);
        _mover.Look(_inputReader.Look);
    }

    private void OnRun(bool isSprint)
    {
        _mover.Run(isSprint);
    }

    private void OnJump()
    {
        if (_jump.CanActivate() == false)
            return;

        _jump.Activate();
    }
}