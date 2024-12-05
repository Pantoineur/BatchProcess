using System;
using Avalonia.OpenGL;
using OpenTK;

namespace BatchProcess.OpenGL;

public class AvaloniaGLContext : IBindingsContext
{
    private readonly GlInterface _glInterface;

    public AvaloniaGLContext(GlInterface glInterface)
    {
        _glInterface = glInterface;
    }
    
    public IntPtr GetProcAddress(string procName)
    {
        return _glInterface.GetProcAddress(procName);
    }
}