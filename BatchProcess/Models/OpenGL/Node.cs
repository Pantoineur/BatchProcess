using Silk.NET.Maths;

namespace BatchProcess.Models.OpenGL;

public class Node
{
    private Matrix4X4<float> _modelMatrix;
    public Vector3D<float> Position { get; set; }
    public float Yaw { get; set; }
    public float Pitch { get; set; }
    public float Roll { get; set; }
    public Quaternion<float> Rotation { get; set; }

    public Node(){ }
    public Node(Vector3D<float> position)
    {
        Position = position;
        _modelMatrix = Matrix4X4.CreateTranslation(position);
    }
    public Node(Vector3D<float> position, Vector3D<float> rotationEuler)
    {
        Yaw = rotationEuler.X;
        Pitch = rotationEuler.Y;
        Roll = rotationEuler.Z;
        Position = position;
        
        Rotation = Quaternion<float>.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
        _modelMatrix = Matrix4X4.CreateFromQuaternion(Rotation) * Matrix4X4.CreateTranslation(position);
    }

    public void RotateEuler(float angle, Vector3D<float> axis)
    {
        // _modelMatrix *= Matrix4X4.CreateRotationY(axis.Y * angle) * Matrix4X4.CreateRotationX(axis.X * angle) * Matrix4X4.CreateRotationZ(axis.Z * angle);
    }

    public void Scale(Vector3D<float> scale)
    {
        _modelMatrix *= Matrix4X4.CreateScale(scale);
    }

    public void Translate(Vector3D<float> translation)
    {
        _modelMatrix *= Matrix4X4.CreateTranslation(translation);
    }

    public void SetPosition(Vector3D<float> position)
    {
        Position = position;
        
        _modelMatrix.M41 = position.X;
        _modelMatrix.M42 = position.Y;
        _modelMatrix.M43 = position.Z;
    }

    public Vector3D<float> GetPosition()
    {
        return Position;
    }

    public Matrix4X4<float> Matrix => _modelMatrix;

    public virtual void Update(float deltaTime)
    {
        
    }

    public virtual void Init()
    {
        
    }

    public virtual void Destroy()
    {
        
    }
}