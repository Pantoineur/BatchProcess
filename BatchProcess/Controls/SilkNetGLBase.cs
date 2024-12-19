using System;
using System.Diagnostics;
using System.Drawing;
using Avalonia;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using BatchProcess.Data;
using BatchProcess.Models;
using BatchProcess.Models.OpenGL;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = BatchProcess.Models.OpenGL.Shader;
using Texture = BatchProcess.Models.OpenGL.Texture;

namespace BatchProcess.Controls
{
    public class SilkNetGLBase : OpenGlControlBase
    {
        public KeyboardManager KeyboardManager { get; }
        protected GL GL = null!;
        protected uint FrameCounter;
        private uint _lastFrameCounter;
        private int _fps = 60;
        private Stopwatch _stopwatch = new();
        private float _lastElapsed;
        private float _totalElapsed;

        public SilkNetGLBase()
        {
            KeyboardManager = new();
        }
        
        protected override void OnOpenGlInit(GlInterface gl)
        {
            GL = GL.GetApi(gl.GetProcAddress);
            _stopwatch.Start();
            Start();
        }

        protected virtual void Start()
        {
            
        }

        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            var deltaTime = _stopwatch.ElapsedMilliseconds / 1000f;
            _stopwatch.Restart();
            _totalElapsed += deltaTime;

            UpdateRender(deltaTime);
            FrameCounter++;

            // if (_totalElapsed >= 1f)
            // {
            //     var fps = FrameCounter - _lastFrameCounter;
            //     _lastFrameCounter = FrameCounter;
            //     
            //     Console.WriteLine($"FPS: {fps:F0}");
            //     _totalElapsed = 0;
            // }
            
            Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Background);
        }

        protected virtual void UpdateRender(float deltaTime)
        {
            
        }

        protected override void OnOpenGlDeinit(GlInterface gl)
        {
            TearDown();
        }

        protected virtual void TearDown()
        {
            
        }

        protected override void OnOpenGlLost()
        {
            base.OnOpenGlLost();
            Console.WriteLine($"Lost GL !");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            KeyboardManager.SetKeyState(e.Key, true);
        }
    }
}