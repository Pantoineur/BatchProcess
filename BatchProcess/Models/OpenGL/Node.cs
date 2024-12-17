
using OpenTK.Mathematics;

namespace BatchProcess.Models.OpenGL;

public class Node
{
    private Matrix4 _modelMatrix;
    public Vector3 Position { get; set; }
    public float Yaw { get; set; }
    public float Pitch { get; set; }
    public float Roll { get; set; }
    public Quaternion Rotation { get; set; }

    public Node(){ }
    public Node(Vector3 position)
    {
        Position = position;
        _modelMatrix = Matrix4.CreateTranslation(position);
    }
    public Node(Vector3 position, Vector3 rotationEuler)
    {
        Yaw = rotationEuler.X;
        Pitch = rotationEuler.Y;
        Roll = rotationEuler.Z;
        Position = position;
        
        Rotation = Quaternion.FromEulerAngles(Pitch, Yaw, Roll);
        _modelMatrix = Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(position);
    }

    public void RotateEuler(float angle, Vector3 axis)
    {
        // _modelMatrix *= Matrix4.CreateRotationY(axis.Y * angle) * Matrix4.CreateRotationX(axis.X * angle) * Matrix4.CreateRotationZ(axis.Z * angle);
    }

    public void Scale(Vector3 scale)
    {
        _modelMatrix *= Matrix4.CreateScale(scale);
    }

    public void Translate(Vector3 translation)
    {
        _modelMatrix *= Matrix4.CreateTranslation(translation);
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
        
        _modelMatrix.M41 = position.X;
        _modelMatrix.M42 = position.Y;
        _modelMatrix.M43 = position.Z;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public Matrix4 Matrix => _modelMatrix;

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