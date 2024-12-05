using System;
using Avalonia_Sample;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Avalonia;
using BatchProcess.OpenGL.Models;

namespace BatchProcess.Controls
{
    public class MainWindowGLRendering : BaseTkOpenGlControl
    {
        System.Numerics.Vector3 _color;

        private readonly float[] _vertices =
        [
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
            0.5f, -0.5f, 0.0f, // Bottom-right vertex
            -0.5f,  0.5f, 0.0f, // Top-left vertex
            0.5f,  0.5f, 0.0f  // Top-right vertex
        ];

        private readonly int[] _indices =
        [
            0, 1, 2,
            2, 3, 1
        ];

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;
        
        private Shader _shader;
        // Handle Avalonia Color Picker property and update 

        public static readonly StyledProperty<Vector3> SelectedColorProperty =
            AvaloniaProperty.Register<MainWindowGLRendering, Vector3>(nameof(SelectedColor));

        public Vector3 SelectedColor
        {
            get => GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly StyledProperty<bool> WireframeProperty = AvaloniaProperty.Register<MainWindowGLRendering, bool>(
            nameof(Wireframe));

        public bool Wireframe
        {
            get => GetValue(WireframeProperty);
            set => SetValue(WireframeProperty, value);
        }

        public MainWindowGLRendering()
        {
            
        }

        // OpenTkInit (OnLoad) is called once when the control is created
        protected override void OpenTkInit()
        {
            ChangeWindowTitle();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            _vertexArrayObject = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);
            
            _shader = new("Default");
            _shader.Use();
        }



        //OpenTkRender (OnRenderFrame) is called once a frame. The aspect ratio and keyboard state are configured prior to this being called.
        protected override void OpenTkRender()
        {
            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PolygonMode(TriangleFace.FrontAndBack, Wireframe ? PolygonMode.Line : PolygonMode.Fill);

            _shader.Use();
            _shader.SetVector4("InColor", new(SelectedColor, 1));
            
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

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