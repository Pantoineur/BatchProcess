using System;
using System.Drawing;
using Avalonia_Sample;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Avalonia;
using GlmSharp;
using Pan.Avalonia.OpenGL.Models;

namespace BatchProcess.Controls
{
    public class MainWindowGLRendering : BaseTkOpenGlControl
    {
        private readonly float[] _vertices =
        [
            // ** Rectangle **
            // Vertices          // Colors          // Tex coords
            -0.5f, -0.5f, 0.0f,  1.0f, 1.0f, 0.0f,  0.0f, 0.0f, // Bottom-left vertex
            0.5f,  -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  1.0f, 0.0f, // Bottom-right vertex
            -0.5f,  0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f, // Top-left vertex
            0.5f,   0.5f, 0.0f,  0.5f, 0.5f, 0.0f,  1.0f, 1.0f  // Top-right vertex
            
            // Triangle
            // -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, // Bottom-left vertex
            // 0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, // Bottom-right vertex
            // 0.0f,  0.5f, 0.0f, 0.0f, 0.0f, 1.0f // Top vertex
        ];

        private readonly int[] _indices =
        [
            // Rectangle
            0, 1, 2,
            2, 3, 1
            
            // Triangle
            // 0, 1, 2
        ];

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;

        private mat4 _transformMatrix;
        private mat4 _transformMatrix2; // AFAC
        private readonly float _rotationSpeed = 30.0f;
        private readonly float _scaleSpeed = 0.5f;
        private float _scale = 0;
        private bool _decrease = true;
        
        private Shader? _shader;
        private Texture? _containerTex;
        private Texture? _hahaTex;
        private readonly Color _backgroundColor = Color.Teal;

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

        public static readonly StyledProperty<float> HOffsetProperty = AvaloniaProperty.Register<MainWindowGLRendering, float>(
            nameof(HOffset));

        public float HOffset
        {
            get => GetValue(HOffsetProperty);
            set => SetValue(HOffsetProperty, value);
        }

        public static readonly StyledProperty<float> BlendProperty = AvaloniaProperty.Register<MainWindowGLRendering, float>(
            nameof(Blend));

        public float Blend
        {
            get => GetValue(BlendProperty);
            set => SetValue(BlendProperty, value);
        }

        // OpenTkInit (OnLoad) is called once when the control is created
        protected override void OpenTkInit()
        {
            ChangeWindowTitle();
            
            _vertexArrayObject = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            
            GL.ClearColor(_backgroundColor);
            
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
            const int vec3Size = 3 * sizeof(float);
            const int vec2Size = 2 * sizeof(float);
            const int stride = vec3Size * 2 + vec2Size; // Vertices + Colors (RGB) + TexCoords
            
            // vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);
            
            // colors
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, vec3Size);
            GL.EnableVertexAttribArray(1);
            
            // texcoords
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, vec3Size * 2);
            GL.EnableVertexAttribArray(2);
            
            // EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);
            
            
            _containerTex = new();
            _containerTex.Use();
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, new[] { (int)TextureWrapMode.ClampToEdge });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, new[] { (int)TextureWrapMode.ClampToEdge });
            
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] { (int)TextureMinFilter.Linear });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] { (int)TextureMagFilter.Linear });
            _containerTex.LoadFromImageRGB(@"Resources\Textures\container.jpg");

            _hahaTex = new();
            _hahaTex.Use();
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, new[] { (int)TextureWrapMode.Repeat });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, new[] { (int)TextureWrapMode.Repeat });
            
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] { (int)TextureMinFilter.Linear });
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] { (int)TextureMagFilter.Linear });
            _hahaTex.LoadFromImageRGBA(@"Resources\Textures\awesomeface.png");

            _shader = new("Default");
            _shader.Use();
            _shader.SetInt("tex1", 0);
            _shader.SetInt("tex2", 1);
            _transformMatrix = mat4.Translate(0.5f, -0.5f, 0.0f);
            _transformMatrix = mat4.Translate(0.5f, -0.5f, 0.0f);
            _transformMatrix2 = mat4.Identity * mat4.Translate(-0.5f, 0.5f, 0.0f);
        }

        //OpenTkRender (OnRenderFrame) is called once a frame. The aspect ratio and keyboard state are configured prior to this being called.
        protected override void OpenTkRender(float deltaTime)
        {
            var axis = new vec3(0.0f, 0.0f, 1.0f);
            _transformMatrix *= mat4.Rotate(glm.Radians(deltaTime * _rotationSpeed), axis);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.DstAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PolygonMode(TriangleFace.FrontAndBack, Wireframe ? PolygonMode.Line : PolygonMode.Fill);

            if (_shader is not null)
            {
                _hahaTex?.Use(TextureUnit.Texture1);
                _containerTex?.Use();
            
                _shader.Use();
                _shader.SetVector4("InColor", new(SelectedColor, 1));
                _shader.SetFloat("hOffset", HOffset);
                _shader.SetFloat("blend", Blend);
                _shader.SetMat4("transform", _transformMatrix);
            }
            
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            _scale = _decrease ? 1 - _scaleSpeed * deltaTime : 1 + _scaleSpeed * deltaTime;
            _transformMatrix2 *= mat4.Scale(new vec3(_scale, _scale, 1.0f));

            var totalScale = _transformMatrix2.m11;
            if (totalScale is <= 0.5f or >= 1.5f)
            {
                _decrease = !_decrease;
            }
            
            _shader?.SetMat4("transform", _transformMatrix2);
            
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            
            //Clean up the opengl state back to how we got it
            GL.Disable(EnableCap.DepthTest);
        }

        //OpenTkTeardown is called when the control is being destroyed
        protected override void OpenTkTeardown()
        {
            Console.WriteLine("UI: Tearing down gl component");

            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
        }


        private void ChangeWindowTitle()
        {
            // if (this.VisualRoot is Window window)
                // window.Title = "Avalonia Sample : OpenGL Version: " + GL.GetString(StringName.Version);
        }

    }
}