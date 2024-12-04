using System;
using Avalonia_Sample;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Avalonia;
using Avalonia.Media;
using Window = Avalonia.Controls.Window;

namespace BatchProcess.Controls
{
    public class MainWindowGLRendering : BaseTkOpenGlControl
    {
        System.Numerics.Vector3 _color;

        // Create the vertices for our triangle. These are listed in normalized device coordinates (NDC)
        // In NDC, (0, 0) is the center of the screen.
        // Negative X coordinates move to the left, positive X move to the right.
        // Negative Y coordinates move to the bottom, positive Y move to the top.
        // OpenGL only supports rendering in 3D, so to create a flat triangle, the Z coordinate will be kept as 0.
        private readonly float[] _vertices =
        {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };

        // These are the handles to OpenGL objects. A handle is an integer representing where the object lives on the
        // graphics card. Consider them sort of like a pointer; we can't do anything with them directly, but we can
        // send them to OpenGL functions that need them.

        // What these objects are will be explained in OnLoad.
        private int _vertexBufferObject;

        private int _vertexArrayObject;


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
        }



        //OpenTkRender (OnRenderFrame) is called once a frame. The aspect ratio and keyboard state are configured prior to this being called.
        protected override void OpenTkRender()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.ClearColor(new Color4(SelectedColor.R, SelectedColor.G, SelectedColor.B, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

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