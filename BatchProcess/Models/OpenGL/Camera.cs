using System;
using OpenTK.Mathematics;

namespace BatchProcess.Models.OpenGL;

public class Camera : Node
{
    private Matrix4 _projectionMatrix;
    private float _speed = 5.0f;

    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix => _projectionMatrix;
    
    public float FOV { get; private set; }
    public float AspectRatio { get; private set; }


    public Camera(Vector3 pos, Vector3 rotation, float fov, float aspectRatio) : base(pos, rotation)
    {
        FOV = fov;
        AspectRatio = aspectRatio;
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.DegreesToRadians(FOV), aspectRatio, 0.01f, 100.0f);
    }

    public void UpdateProjection()
    {
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(float.DegreesToRadians(FOV), AspectRatio, 0.01f, 100.0f);
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
        Rotation = Quaternion.FromEulerAngles(Pitch, Yaw, Roll);
        var forward = Vector3.UnitZ;
        var up = Vector3.UnitY;
        
        ViewMatrix = Matrix4.LookAt(Position, Position + new Vector3((0, 0, 1)), up);
    }

    public void Move(Direction direction, float deltaTime)
    {
        var velocity = _speed * deltaTime;
        Rotation = Quaternion.FromEulerAngles(Pitch, Yaw, Roll);
        var forward = Vector3.UnitZ;
        var right = Vector3.UnitX;

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