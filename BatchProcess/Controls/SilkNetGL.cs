using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.OpenGL;
using Avalonia.Threading;
using BatchProcess.Data;
using BatchProcess.Models.OpenGL;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = BatchProcess.Models.OpenGL.Shader;
using Texture = BatchProcess.Models.OpenGL.Texture;

namespace BatchProcess.Controls
{
    public class SilkNetGL : SilkNetGLBase
    {
        private uint _vao;
        private uint _vbo;

        private Shader _mainShader;
        private Camera _mainCamera;

        private Texture _containerTex;
        private Texture _hahaTex;
        
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
        
        protected override void Start()
        {
            GL.ClearColor(0.6f, 0.3f, 0.3f, 1.0f);
            
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            GL.BufferData<float>(BufferTargetARB.ArrayBuffer, (uint) _verticesCube.Length * sizeof(float), _verticesCube, GLEnum.StaticDraw);
            
            const uint vec3Size = 3 * sizeof(float);
            const uint vec2Size = 2 * sizeof(float);
            const uint stride = vec3Size + vec2Size;
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(1);
            
            _containerTex = new(GL);
            _containerTex.Use();
            _containerTex.LoadFromImageRGB(@"Resources\Textures\container.jpg");

            _hahaTex = new(GL);
            _hahaTex.Use();
            _hahaTex.LoadFromImageRGBA(@"Resources\Textures\awesomeface.png");

            _mainShader = new(GL, @"Resources\OpenGL\Shaders\Default\default_tex.vert", @"Resources\OpenGL\Shaders\Default\default_tex.frag");
            _mainShader.Use();
            _mainShader.SetInt("tex1", 0);
            _mainShader.SetInt("tex2", 1);
            
            _mainCamera = new(new (0, 0, -5.0f), Vector3D<float>.Zero,
                float.DegreesToRadians(FOV),(float)Bounds.Width / (float)Bounds.Height);
        }

        protected override void UpdateRender(float deltaTIme)
        {
            DoUpdate();
            
            Console.WriteLine(FrameCounter);
            
            GL.Enable(EnableCap.DepthTest);
            GL.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            
            GL.Viewport(0, 0, (uint)Bounds.Width, (uint)Bounds.Height);
            GL.PolygonMode(TriangleFace.FrontAndBack, Wireframe ? PolygonMode.Line : PolygonMode.Fill);
            
            _mainCamera.UpdateProjection(FOV, (float)Bounds.Width / (float)Bounds.Height);
            _mainCamera.UpdateView();
            
            _hahaTex.Use(TextureUnit.Texture1);
            _containerTex.Use();
            
            _mainShader.Use();
            _mainShader.SetFloat("hOffset", HOffset);
            _mainShader.SetFloat("blend", Blend);
            _mainShader.SetMat4("view", _mainCamera.ViewMatrix);
            _mainShader.SetMat4("projection", _mainCamera.ProjectionMatrix);

            GL.BindVertexArray(_vao);
            foreach (var t in _cubes)
            {
                _mainShader?.SetMat4("model", t.Matrix);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }
            
            GL.BindVertexArray(0);
            GL.Disable(EnableCap.DepthTest);
            
            Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Background);
        }

        private void DoUpdate()
        {
            if (KeyboardManager.IsKeyDown(Key.Z))
            {
                
            }
            else if (KeyboardManager.IsKeyDown(Key.Z))
            {
                
            }
            else if (KeyboardManager.IsKeyDown(Key.Z))
            {
                
            }
            else if (KeyboardManager.IsKeyDown(Key.Z))
            {
                
            }
        }

        protected override void OnOpenGlDeinit(GlInterface gl)
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            Console.WriteLine($"Rendering GL DEINIT... {gl.GetType().FullName}");
            
            base.OnOpenGlDeinit(gl);
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

        #region Styled properties

        public static readonly StyledProperty<float> BlendProperty = AvaloniaProperty.Register<SilkNetGL, float>(
            nameof(Blend));

        public float Blend
        {
            get => GetValue(BlendProperty);
            set => SetValue(BlendProperty, value);
        }
        
        public static readonly StyledProperty<float> HOffsetProperty = AvaloniaProperty.Register<SilkNetGL, float>(
            nameof(HOffset));

        public float HOffset
        {
            get => GetValue(HOffsetProperty);
            set => SetValue(HOffsetProperty, value);
        }

        public static readonly StyledProperty<bool> WireframeProperty = AvaloniaProperty.Register<SilkNetGL, bool>(
            nameof(Wireframe));

        public bool Wireframe
        {
            get => GetValue(WireframeProperty);
            set => SetValue(WireframeProperty, value);
        }

        public static readonly StyledProperty<Vector3D<float>> SelectedColorProperty = AvaloniaProperty.Register<SilkNetGL, Vector3D<float>>(
            nameof(SelectedColor));

        public Vector3D<float> SelectedColor
        {
            get => GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly StyledProperty<float> FOVProperty = AvaloniaProperty.Register<SilkNetGL, float>(
            nameof(FOV));

        public float FOV
        {
            get => GetValue(FOVProperty);
            set => SetValue(FOVProperty, value);
        }
        
        #endregion
    }
}