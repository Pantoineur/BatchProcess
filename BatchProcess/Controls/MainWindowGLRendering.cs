using System;
using System.Drawing;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Avalonia;
using Avalonia.Input;
using BatchProcess.Models;
using BatchProcess.Models.OpenGL;
using Pan.Avalonia.OpenGL;
using Pan.Avalonia.OpenGL.Models;

namespace BatchProcess.Controls
{
    public class MainWindowGLRendering : BaseTkOpenGlControl
    {
        private bool _isDragging;
        
        private readonly float[] _verticesCube =
        [
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,

            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
            -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,

            -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,

            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f
        ];

        private readonly Node[] _cubes =
        [
            new(new (0.0f, 0.0f, 0.0f)),
            new(new (2.0f, 5.0f, -15.0f)),
            new(new (-1.5f, -2.2f, -2.5f)),
            new(new (-3.8f, -2.0f, -12.3f)),
            new(new (2.4f, -0.4f, -3.5f)),
            new(new (-1.7f, 3.0f, -7.5f)),
            new(new (1.3f, -2.0f, -2.5f)),
            new(new (1.5f, 2.0f, -2.5f)),
            new(new (1.5f, 0.2f, -1.5f)),
            new(new (-1.3f, 1.0f, -1.5f))
        ];
        
        private readonly int[] _indices =
        [
            // Cube
            // Front
            0, 1, 2,
            2, 3, 1,
        ];

        private Camera? _mainCamera;
        
        private readonly float _rotationSpeed = 30.0f;
        
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

        public static readonly StyledProperty<float> FOVProperty = AvaloniaProperty.Register<MainWindowGLRendering, float>(
            nameof(FOV));

        public float FOV
        {
            get => GetValue(FOVProperty);
            set => SetValue(FOVProperty, value);
        }
        
        protected override void OpenTkInit()
        {
            ChangeWindowTitle();
            GL.ClearColor(_backgroundColor);
            
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

            _mainCamera = new(new (0, 0, -5.0f), Vector3.Zero,
                float.DegreesToRadians(FOV),(float)Bounds.Width / (float)Bounds.Height);
        }

        //OpenTkRender (OnRenderFrame) is called once a frame. The aspect ratio and keyboard state are configured prior to this being called.
        protected override void OpenTkRender(float deltaTime)
        {
            DoUpdate(deltaTime);
            
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.DstAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PolygonMode(TriangleFace.FrontAndBack, Wireframe ? PolygonMode.Line : PolygonMode.Fill);
            
            _mainCamera?.UpdateProjection(FOV, (float)Bounds.Width / (float)Bounds.Height);
            _mainCamera?.UpdateView();
            
            if (_shader is not null)
            {
                _hahaTex?.Use(TextureUnit.Texture1);
                _containerTex?.Use();
            
                _shader.Use();
                _shader.SetVector4("InColor", new(SelectedColor, 1));
                _shader.SetFloat("hOffset", HOffset);
                _shader.SetFloat("blend", Blend);
                _shader.SetMat4("view", _mainCamera?.ViewMatrix ?? Matrix4.Identity);
                _shader?.SetMat4("projection", _mainCamera?.ProjectionMatrix ?? Matrix4.Identity);
            }

            for (var i = 0; i < _cubes.Length; i++)
            {
                var rotationAmount = i % 3 == 0 ? 1 : -1;
                _cubes[i].RotateEuler(deltaTime * _rotationSpeed * (i+1) * rotationAmount, new (1, 0.5f, 0.74f));
                _shader?.SetMat4("model", _cubes[i].Matrix);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }
            
            GL.BindVertexArray(0);
            
            //Clean up the opengl state back to how we got it
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
        }

        private void DoUpdate(float deltaTime)
        {
            if (_mainCamera is null) return;
            
            if (KeyboardState.WasKeyDownLastFrame(Key.Z))
            {
                _mainCamera.Move(Direction.Forward, deltaTime);
            }
            if (KeyboardState.IsKeyDown(Key.S))
            {
                _mainCamera.Move(Direction.Backward, deltaTime);
            }
            if (KeyboardState.IsKeyDown(Key.D))
            {
                _mainCamera.Move(Direction.Right, deltaTime);
            }
            if (KeyboardState.IsKeyDown(Key.Q))
            {
                _mainCamera.Move(Direction.Left, deltaTime);
            }
        }

        protected override void OpenTkTeardown()
        {
            Console.WriteLine("UI: Tearing down gl component");
        }

        private void ChangeWindowTitle()
        {
            // if (this.VisualRoot is Window window)
                // window.Title = "Avalonia Sample : OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            if (point.Properties.IsRightButtonPressed)
            {
                _isDragging = true;
            }
        }
    }
}