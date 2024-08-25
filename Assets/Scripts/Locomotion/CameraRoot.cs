using UnityEngine;

public class CameraRoot : MonoBehaviour
{
    private const float MaxValueAngle = 360f;
    private const float ThresholdDirectionLook = 0.01f;

    [Header("Camera")]
    [SerializeField] private GameObject _cameraTarget;
    [SerializeField] private float _speedCameraRotationYaw;
    [SerializeField] private float _speedCameraRotationPitch;
    [SerializeField] private float _topClamp;
    [SerializeField] private float _bottomClamp;

    public float CameraTargetYaw { get; private set; }
    public float CameraTargetPitch { get; private set; }

    private void Awake()
    {
        CameraTargetYaw = _cameraTarget.transform.eulerAngles.y;
    }

    public void Rotate(Vector2 direction)
    {
        if (direction.sqrMagnitude >= ThresholdDirectionLook)
        {
            CameraTargetYaw += direction.x * Time.deltaTime * _speedCameraRotationYaw;
            CameraTargetPitch += direction.y * Time.deltaTime * _speedCameraRotationPitch;
        }

        CameraTargetYaw = ClampAngle(CameraTargetYaw, float.MinValue, float.MaxValue);
        CameraTargetPitch = ClampAngle(CameraTargetPitch, _bottomClamp, _topClamp);
        _cameraTarget.transform.rotation = Quaternion.Euler(CameraTargetPitch, CameraTargetYaw, 0.0f);
    }

    private float ClampAngle(float angle, float minAngel, float maxAngel)
    {
        if (angle < -MaxValueAngle)
            angle += MaxValueAngle;

        if (angle > MaxValueAngle)
            angle -= MaxValueAngle;

        return Mathf.Clamp(angle, minAngel, maxAngel);
    }
}