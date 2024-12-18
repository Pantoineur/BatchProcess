using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using Pan.Avalonia.OpenGL.Models;

namespace BatchProcess.Models._3D;

public class Mesh
{
    private int _vao;
    private int _vbo;
    private int _ebo;
    
    public List<Vertex> Vertices { get; set; }
    public List<int> Indices { get; set; }
    public List<Texture> Textures { get; set; }

    public Mesh() {}

    public Mesh(Vertex[] vertices, int[] indices, Texture[] textures)
    {
        Vertices = [..vertices];
        Indices = [..indices];
        Textures = [..textures];
        
        SetupMesh();
    }

    private void SetupMesh()
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);
        
        const int vec3Size = 3 * sizeof(float);
        const int vec2Size = 2 * sizeof(float);
        const int totalLength = vec3Size * 2 + vec2Size; // Vertices + Normal + TexCoords

        var verticesBufferData = Vertices.ToBufferFloatArray();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, verticesBufferData.Length, verticesBufferData, BufferUsageHint.StaticDraw);
            
        // EBO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(int), Indices.ToArray(), BufferUsageHint.StaticDraw);
            
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, totalLength, 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, totalLength, vec3Size);
        GL.EnableVertexAttribArray(1);
        
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, totalLength, vec3Size * 2);
        GL.EnableVertexAttribArray(2);

        GL.BindVertexArray(0);
    }

    public void Draw(Shader shader)
    {
        uint numDiffuse = 0;
        uint numSpecular = 0;
        for(var i = 0; i < Textures.Count; i++)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + i);

            var number = "";
            var name = Textures[i].Type;
            if (name.Contains("diff", StringComparison.OrdinalIgnoreCase))
            {
                numDiffuse++;
                number = numDiffuse.ToString();
            }
            else if (name.Contains("spec", StringComparison.OrdinalIgnoreCase))
            {
                numSpecular++;
                number = numSpecular.ToString();
            }
            
            shader.SetInt($"material.name{number}", i);
            GL.BindTexture(TextureTarget.Texture2D, Textures[i].Id);
        }
        
        GL.ActiveTexture(TextureUnit.Texture0);
        
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
    
    public void Destroy()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
    }
}