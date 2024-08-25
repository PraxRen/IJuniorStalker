using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMover : BaseMover
{
    private CharacterController _characterController;

    protected override void AwakeAddon()
    {
        _characterController = GetComponent<CharacterController>();
    }

    protected override float GetHeightCollider()
    {
        return _characterController.height;
    }

    protected override float GetOffsetAngelAxisY()
    {
        return 0f;
    }

    protected override float GetSlopeLimit() 
    {
        return _characterController.slopeLimit;
    }

    protected override void HandleMove()
    {
        Vector3 velocity = new Vector3(HorizontalVelocity.x, VerticalVelocity, HorizontalVelocity.z) + Force + Slope;
        _characterController.Move(velocity);
    }
}