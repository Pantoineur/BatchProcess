using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;

namespace BatchProcess.Models.Data;

public struct Vertex
{
    public Vector3D<float> Position;
    public Vector3D<float> Normal;
    public Vector2D<float> TexCoords;

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