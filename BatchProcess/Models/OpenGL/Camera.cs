using System;
using Silk.NET.Maths;

namespace BatchProcess.Models.OpenGL;

public class Camera : Node
{
    private Matrix4X4<float> _projectionMatrix;

    public Matrix4X4<float> ViewMatrix { get; private set; }
    public Matrix4X4<float> ProjectionMatrix => _projectionMatrix;
    public float Speed { get; set; } = 5.0f;
    
    public float FOV { get; private set; }
    public float AspectRatio { get; private set; }


    public Camera(Vector3D<float> pos, Vector3D<float> rotation, float fov, float aspectRatio) : base(pos, rotation)
    {
        FOV = Math.Clamp(fov, 40, 120);
        AspectRatio = aspectRatio;
        
        UpdateProjection();
    }

    public void UpdateProjection()
    {
        _projectionMatrix = Matrix4X4.CreatePerspectiveFieldOfView(float.DegreesToRadians(FOV), AspectRatio, 0.01f, 100.0f);
    }
    public void UpdateProjection(float fov, float aspectRatio)
    {
        if (fov > FOV || fov < FOV && fov > 39.0f || fov <= 120.0f || aspectRatio > AspectRatio || aspectRatio < AspectRatio)
        {
            FOV = fov;
            AspectRatio = aspectRatio;
        }

        UpdateProjection();
    }

    public void UpdateView()
    {
        Rotation = Quaternion<float>.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
        var forward = Vector3D<float>.UnitZ;
        var up = Vector3D<float>.UnitY;
        
        ViewMatrix = Matrix4X4.CreateLookAt(Position, Position + forward, up);
    }

    public void Move(Direction direction, float deltaTime)
    {
        var velocity = Speed * deltaTime;
        Rotation = Quaternion<float>.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
        var forward = Vector3D<float>.UnitZ;
        var right = Vector3D<float>.UnitX;

        switch (direction)
        {
            case Direction.Forward:
                Position += forward * velocity;
                break;
            case Direction.Backward:
                Position -= forward * velocity;
                break;
            case Direction.Right:
                Position -= right * velocity;
                break;
            case Direction.Left:
                Position += right * velocity;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        
        UpdateView();
    }
}