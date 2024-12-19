using System;
using System.IO;
using BatchProcess.Extensions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace BatchProcess.Models.OpenGL
{
    public class Shader : IDisposable
    {
        private readonly uint _handle;
        private readonly GL _gl;

        public Shader(GL gl, string vertexPath, string fragmentPath)
        {
            _gl = gl;

            var vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            var fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);
            _handle = _gl.CreateProgram();
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            _gl.LinkProgram(_handle);
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
            }
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public void Use()
        {
            _gl.UseProgram(_handle);
        }

        public void SetInt(string name, int value)
        {
            var location = GetUniformLocation(name);
            _gl.Uniform1(location, value);
        }

        public void SetFloat(string name, float value)
        {
            var location = GetUniformLocation(name);
            _gl.Uniform1(location, value);
        }

        public void SetVector3(string name, Vector3D<float> value)
        {
            var location = GetUniformLocation(name);
            _gl.Uniform3(location, value.ToSystem());
        }

        public void SetVector4(string name, Vector4D<float> value)
        {
            var location = GetUniformLocation(name);
            _gl.Uniform4(location, value.ToSystem());
        }

        public void SetMat4(string name, Matrix4X4<float> matrix)
        {
            var location = GetUniformLocation(name);
            _gl.UniformMatrix4(location, 1, false, matrix.ToFloatArray());
        }

        private int GetUniformLocation(string name)
        {
            var location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new($"{name} uniform not found on shader.");
            }

            return location;
        }
        
        public void Dispose()
        {
            _gl.DeleteProgram(_handle);
            GC.SuppressFinalize(this);
        }

        private uint LoadShader(ShaderType type, string path)
        {
            var src = File.ReadAllText(path);
            var handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, src);
            _gl.CompileShader(handle);
            var infoLog = _gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }
    }
}