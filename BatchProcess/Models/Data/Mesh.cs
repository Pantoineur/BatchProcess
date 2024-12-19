using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;
using Shader = BatchProcess.Models.OpenGL.Shader;

namespace BatchProcess.Models.Data;

public class Mesh
{
    private readonly GL _gl;
    
    private uint _vao;
    private uint _vbo;
    private uint _ebo;
    
    public List<Vertex> Vertices { get; set; }
    public List<int> Indices { get; set; }
    public List<Texture> Textures { get; set; }

    public Mesh(GL gl)
    {
        _gl = gl;
    }

    public Mesh(GL gl, Vertex[] vertices, int[] indices, Texture[] textures)
    {
        _gl = gl;
        
        Vertices = [..vertices];
        Indices = [..indices];
        Textures = [..textures];
        
        SetupMesh();
    }

    private void SetupMesh()
    {
        _vao = _gl.GenVertexArray();
        _vbo = _gl.GenBuffer();
        _ebo = _gl.GenBuffer();

        _gl.BindVertexArray(_vao);
        
        const int vec3Size = 3 * sizeof(float);
        const int vec2Size = 2 * sizeof(float);
        const int totalLength = vec3Size * 2 + vec2Size; // Vertices + Normal + TexCoords

        var verticesBufferData = Vertices.ToBufferFloatArray();
        
        _gl.BindBuffer(GLEnum.ArrayBuffer, _vbo);
        _gl.BufferData<float>(GLEnum.ArrayBuffer, (nuint)verticesBufferData.Length, verticesBufferData, GLEnum.StaticDraw);
            
        // EBO
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, _ebo);
        _gl.BufferData<int>(GLEnum.ElementArrayBuffer, (nuint) Indices.Count * sizeof(int), Indices.ToArray(), GLEnum.StaticDraw);
            
        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, totalLength, 0);
        _gl.EnableVertexAttribArray(0);
        
        _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, totalLength, vec3Size);
        _gl.EnableVertexAttribArray(1);
        
        _gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, totalLength, vec3Size * 2);
        _gl.EnableVertexAttribArray(2);

        _gl.BindVertexArray(0);
    }

    public void Draw(Shader shader)
    {
        uint numDiffuse = 0;
        uint numSpecular = 0;
        for(var i = 0; i < Textures.Count; i++)
        {
            _gl.ActiveTexture(TextureUnit.Texture0 + i);

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
            _gl.BindTexture(TextureTarget.Texture2D, Textures[i].Id);
        }
        
        _gl.ActiveTexture(TextureUnit.Texture0);
        
        _gl.BindVertexArray(_vao);
        _gl.DrawElements(GLEnum.Triangles, (uint)Indices.Count, GLEnum.UnsignedInt, 0);
        _gl.BindVertexArray(0);
    }
    
    public void Destroy()
    {
        _gl.DeleteVertexArray(_vao);
        _gl.DeleteBuffer(_vbo);
        _gl.DeleteBuffer(_ebo);
    }
}