using UnityEngine;
using TMPro;

public class LocomotionUI : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private MonoBehaviour _moverMonoBehaviour;
    [SerializeField] private CameraRoot _cameraRoot;
    [SerializeField] private TextMeshProUGUI _textInputDirectionMove;
    [SerializeField] private TextMeshProUGUI _textInputDirectionLook;
    [SerializeField] private TextMeshProUGUI _textHorizontalVelocity;
    [SerializeField] private TextMeshProUGUI _textVerticalVelocity;
    [SerializeField] private TextMeshProUGUI _textAngelAxisY;
    [SerializeField] private TextMeshProUGUI _textSpeed;
    [SerializeField] private TextMeshProUGUI _textCameraTargetYaw;
    [SerializeField] private TextMeshProUGUI _textCameraTargetPitch;
    [SerializeField] private TextMeshProUGUI _textForce;
    [SerializeField] private TextMeshProUGUI _textSlope;
    [SerializeField] private TextMeshProUGUI _textIsRun;
    [SerializeField] private TextMeshProUGUI _textIsGrounded;

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

    private void Update()
    {
        _textInputDirectionMove.text = _mover.InputDirectionMove.ToString();
        _textInputDirectionLook.text = _mover.InputDirectionLook.ToString();
        _textHorizontalVelocity.text = _mover.HorizontalVelocity.ToString();
        _textVerticalVelocity.text = _mover.VerticalVelocity.ToString();
        _textAngelAxisY.text = _mover.AngelAxisY.ToString();
        _textSpeed.text = _mover.Speed.ToString();
        _textCameraTargetYaw.text = _cameraRoot.CameraTargetYaw.ToString();
        _textCameraTargetPitch.text = _cameraRoot.CameraTargetPitch.ToString();
        _textForce.text = _mover.Force.ToString();
        _textSlope.text = _mover.Slope.ToString();
        _textIsRun.text = _mover.IsRun.ToString();
        _textIsGrounded.text = _mover.IsGrounded.ToString();
    }
#endif
}