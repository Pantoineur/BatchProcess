﻿using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using Avalonia.Threading;
using Pan.Avalonia.OpenGL;
using OpenTK.Graphics.OpenGL4;

namespace Avalonia_Sample;

public abstract class BaseTkOpenGlControl : OpenGlControlBase, ICustomHitTest
{
    /// <summary>
    /// KeyboardState provides an easy-to-use, stateful wrapper around Avalonia's Keyboard events, as OpenTK keyboard states are not handled.
    /// You can access full keyboard state for both the current frame and the previous one through this object.
    /// </summary>
    public AvaloniaKeyboardState KeyboardState = new();

    private AvaloniaGLContext? _avaloniaTkContext;

    private GlInterface gl;

    private float _deltaTime = 0f;
    private Stopwatch _stopwatch = new();

    /// <summary>
    /// OpenTkRender is called once a frame to draw to the control.
    /// You can do anything you want here, but make sure you undo any configuration changes after, or you may get weirdness with other controls.
    /// </summary>
    protected virtual void OpenTkRender(float deltaTime)
    {
        
    }

    /// <summary>
    /// OpenTkInit is called once when the control is first created.
    /// At this point, the GL bindings are initialized and you can invoke GL functions.
    /// You could use this function to load and compile shaders, load textures, allocate buffers, etc.
    /// </summary>
    protected virtual void OpenTkInit()
    {
        
    }

    /// <summary>
    /// OpenTkTeardown is called once when the control is destroyed.
    /// Though GL bindings are still valid, as OpenTK provides no way to clear them, you should not invoke GL functions after this function finishes executing.
    /// At best, they will do nothing, at worst, something could go wrong.
    /// You should use this function as a last chance to clean up any GL resources you have allocated - delete buffers, vertex arrays, programs, and textures.
    /// </summary>
    protected virtual void OpenTkTeardown()
    {
        
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        this.gl = gl;
        
        _stopwatch.Stop();
        _deltaTime = _stopwatch.Elapsed.Milliseconds / 1000f;
        _stopwatch = Stopwatch.StartNew();
        
        //Update last key states
        KeyboardState.OnFrame();

        PixelSize size = GetPixelSize();

        //Set up the aspect ratio so shapes aren't stretched.
        GL.Viewport(0, 0, size.Width , size.Height);

        //Tell our subclass to render
        if (Bounds.Width != 0 && Bounds.Height != 0)
        {
            OpenTkRender(_deltaTime);
        }

        //Schedule next UI update with avalonia
        Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Background);
    }


    protected sealed override void OnOpenGlInit(GlInterface gl)
    {
        _stopwatch = Stopwatch.StartNew();
        //Initialize the OpenTK<->Avalonia Bridge
        _avaloniaTkContext = new AvaloniaGLContext(gl);

        GL.LoadBindings(_avaloniaTkContext);
        

        //Invoke the subclass' init function
        OpenTkInit();
    }

    //Simply call the subclass' teardown function
    protected sealed override void OnOpenGlDeinit(GlInterface gl)
    {
        OpenTkTeardown();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (!IsEffectivelyVisible)
            return;
        
        KeyboardState.SetKey(e.Key, true);
    }
    
    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (!IsEffectivelyVisible)
            return;
        
        KeyboardState.SetKey(e.Key, false);
    }

    public bool HitTest(Point point) => Bounds.Contains(point);


    public GlInterface GetGLInterface()
    {
        return gl;
    }

    private PixelSize GetPixelSize()
    {
        var scaling = TopLevel.GetTopLevel(this).RenderScaling;
        return new PixelSize(Math.Max(1, (int)(Bounds.Width * scaling)),
            Math.Max(1, (int)(Bounds.Height * scaling)));
    }


}