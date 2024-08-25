using UnityEngine;

public interface IMover
{
    Vector2 InputDirectionMove { get;}
    Vector2 InputDirectionLook { get;}
    Vector3 HorizontalVelocity { get;}
    Vector3 Force { get;}
    Vector3 Slope { get;}
    float VerticalVelocity { get; }
    float AlwayVerticalVelocity { get;}
    float AngelAxisY { get; }
    float Speed { get;}
    bool IsRun { get;}
    bool IsGrounded { get;}

    void Move(Vector2 direction);
    void Look(Vector2 direction);
    void Run(bool value);
    void AddForce(Vector3 direction, float force);
}
