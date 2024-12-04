using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace BatchProcess.OpenGL.Models;

public class Shader
{
    private const string SHADERS_PATH = @"..\Shaders";

    public string FragmentPath { get; init; }
    public string VertexPath { get; init; }
    public int ShaderProgram { get; private set; }

    public Shader(string shaderName)
    {
        FragmentPath = Path.Combine(SHADERS_PATH, shaderName, $"{shaderName}.frag");
        VertexPath = Path.Combine(SHADERS_PATH, shaderName, $"{shaderName}.vert");

        Init();
    }

    public Shader(string vertexPath, string fragmentPath)
    {
        FragmentPath = fragmentPath;
        VertexPath = vertexPath;

        Init();
    }

    private void Init()
    {
        CompileShader();
        CreateProgram();

    }

    private void CreateProgram()
    {
    }

    private void CompileShader()
    {
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, VertexPath);
        GL.CompileShader(vertexShader);
        
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, FragmentPath);
        GL.CompileShader(fragmentShader);
        
        ShaderProgram = GL.CreateProgram();
        GL.AttachShader(ShaderProgram, vertexShader);
        GL.AttachShader(ShaderProgram, fragmentShader);
        GL.LinkProgram(ShaderProgram);
        
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        
        GL.UseProgram(ShaderProgram);
    }
}