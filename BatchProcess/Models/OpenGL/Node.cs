using GlmSharp;
using OpenTK.Mathematics;

namespace BatchProcess.Models.OpenGL;

public class Node
{
    private mat4 _modelMatrix;
    
    public Node(){ }
    public Node(vec3 position)
    {
        _modelMatrix = mat4.Translate(position);
    }

    public void RotateEuler(float angle, vec3 axis)
    {
        _modelMatrix *= mat4.Rotate(glm.Radians(angle), axis);
    }

    public void Scale(vec3 scale)
    {
        _modelMatrix *= mat4.Scale(scale);
    }

    public void Translate(vec3 translation)
    {
        _modelMatrix *= mat4.Translate(translation);
    }

    public void SetPosition(vec3 position)
    {
        _modelMatrix.m30 = position.x;
        _modelMatrix.m31 = position.y;
        _modelMatrix.m32 = position.z;
    }

    public vec3 GetPosition()
    {
        return new(_modelMatrix.m30, _modelMatrix.m31, _modelMatrix.m32);
    }

    public mat4 Matrix => _modelMatrix;
}