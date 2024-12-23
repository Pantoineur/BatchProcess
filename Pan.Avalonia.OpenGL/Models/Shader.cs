﻿using GlmSharp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Pan.Avalonia.OpenGL.Models;

public class Shader
{
    private string _shadersPath = @"Resources\OpenGL\Shaders";
    
    private Dictionary<string, int> _uniformLocations = [];

    public string Fragment { get; init; }
    public string Vertex { get; init; }
    public int Handle { get; private set; }

    public Shader(string shaderName)
    {
        Fragment = File.ReadAllText(Path.Combine(_shadersPath, shaderName, $"{shaderName}.frag"));
        Vertex = File.ReadAllText(Path.Combine(_shadersPath, shaderName, $"{shaderName}.vert"));

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
        GL.UseProgram(Handle);
    }
    
    private void Init()
    {
        Compile();
        PrepareUniforms();
    }

    private void PrepareUniforms()
    {
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numUniforms);

        for (var i = 0; i < numUniforms; i++)
        {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);
            
            _uniformLocations[key] = GL.GetUniformLocation(Handle, key);
        }
    }

    private void Compile()
    {
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, Vertex);
        GL.CompileShader(vertexShader);
        
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, Fragment);
        GL.CompileShader(fragmentShader);
        
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        Link(Handle);
        
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
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

    public void SetFloat(string parameterName, float value)
    {
        if (_uniformLocations.TryGetValue(parameterName, out var location))
        {
            GL.Uniform1(location, value);
        }
    }

    public void SetVector3(string parameterName, Vector3 vec)
    {
        if (_uniformLocations.TryGetValue(parameterName, out var location))
        {
            GL.Uniform3(location, vec);
        }
    }

    public void SetVector4(string parameterName, Vector4 vec)
    {
        if (_uniformLocations.TryGetValue(parameterName, out var location))
        {
            GL.Uniform4(location, vec);
        }
    }

    public void SetInt(string parameterName, int value)
    {
        if (_uniformLocations.TryGetValue(parameterName, out var location))
        {
            GL.Uniform1(location, value);
        }
    }

    public void SetMat4(string parameterName, mat4 matrix)
    {
        if (_uniformLocations.TryGetValue(parameterName, out var location))
        {
            GL.UniformMatrix4(location, 1, transpose: false, matrix.Values1D);
        }
    }
}