using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace BatchProcess.OpenGL.Models;

public class Shader
{
    private const string SHADERS_PATH = @"D:\Dev\Other\BatchProcess\BatchProcess\BatchProcess\OpenGL\Shaders";

    public string Fragment { get; init; }
    public string Vertex { get; init; }
    public int ShaderProgram { get; private set; }

    public Shader(string shaderName)
    {
        Fragment = File.ReadAllText(Path.Combine(SHADERS_PATH, shaderName, $"{shaderName}.frag"));
        Vertex = File.ReadAllText(Path.Combine(SHADERS_PATH, shaderName, $"{shaderName}.vert"));

        Init();
    }

    public Shader(string vertexPath, string fragmentPath)
    {
        Fragment = File.ReadAllText(fragmentPath);
        Vertex = File.ReadAllText(vertexPath);

        Init();
    }

    public void Use()
    {
        GL.UseProgram(ShaderProgram);
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
        GL.ShaderSource(vertexShader, Vertex);
        GL.CompileShader(vertexShader);
        
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, Fragment);
        GL.CompileShader(fragmentShader);
        
        ShaderProgram = GL.CreateProgram();
        GL.AttachShader(ShaderProgram, vertexShader);
        GL.AttachShader(ShaderProgram, fragmentShader);
        Link(ShaderProgram);
        
        GL.DetachShader(ShaderProgram, vertexShader);
        GL.DetachShader(ShaderProgram, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    private static void Link(int program)
    {
        GL.LinkProgram(program);

        // Check for linking errors
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            var error = GL.GetProgramInfoLog(program);
            throw new Exception($"Error occurred whilst linking Program({program}): \n{error}");
        }
    }
    
}