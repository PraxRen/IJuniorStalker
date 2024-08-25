using UnityEngine;

[RequireComponent(typeof(CameraRoot))]
public class PlayerMover : CharacterControllerMover
{
    private CameraRoot _cameraRoot;
    private Transform _cameraTransform;

    private void LateUpdate()
    {
        _cameraRoot.Rotate(InputDirectionLook);
    }

    protected override void AwakeAddon()
    {
        base.AwakeAddon();
        _cameraTransform = Camera.main.transform;
        _cameraRoot = GetComponent<CameraRoot>();
    }

    protected override float GetOffsetAngelAxisY() => _cameraTransform.eulerAngles.y;
}