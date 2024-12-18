using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;

namespace BatchProcess.Models._3D;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoords;

    public float[] ToFloatArray()
    {
        return [Position.X, Position.Y, Position.Z, Normal.X, Normal.Y, Normal.Z, TexCoords.X, TexCoords.Y];
    }
}

public static class VertexExtensions
{
    public static float[] ToBufferFloatArray(this List<Vertex> vertex)
    {
        return vertex.SelectMany(x => x.ToFloatArray()).ToArray();
    }
}