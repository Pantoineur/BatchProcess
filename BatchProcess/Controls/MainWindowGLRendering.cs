using System;
using Avalonia_Sample;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Avalonia;
using Avalonia.Media;
using BatchProcess.OpenGL.Models;
using Window = Avalonia.Controls.Window;

namespace BatchProcess.Controls
{
    public class MainWindowGLRendering : BaseTkOpenGlControl
    {
        System.Numerics.Vector3 _color;

        private readonly float[] _vertices =
        {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        
        private Shader _shader;
        // Handle Avalonia Color Picker property and update 

        public static readonly StyledProperty<Color> SelectedColorProperty =
    AvaloniaProperty.Register<MainWindowGLRendering, Color>(nameof(SelectedColor));

        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public MainWindowGLRendering()
        {
            
        }



        // OpenTkInit (OnLoad) is called once when the control is created
        protected override void OpenTkInit()
        {
            ChangeWindowTitle();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            _shader = new("Default");
            _shader.Use();
        }



        //OpenTkRender (OnRenderFrame) is called once a frame. The aspect ratio and keyboard state are configured prior to this being called.
        protected override void OpenTkRender()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.ClearColor(new Color4( 255/*SelectedColor.R*/, 0/*SelectedColor.G*/, 0/*SelectedColor.B*/, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            
            _shader.Use();
            
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);

            //Clean up the opengl state back to how we got it
            GL.Disable(EnableCap.DepthTest); 
        }

        //OpenTkTeardown is called when the control is being destroyed
        protected override void OpenTkTeardown()
        {
            Console.WriteLine("UI: Tearing down gl component");

        }


        private void ChangeWindowTitle()
        {
            // if (this.VisualRoot is Window window)
                // window.Title = "Avalonia Sample : OpenGL Version: " + GL.GetString(StringName.Version);
        }

    }
}