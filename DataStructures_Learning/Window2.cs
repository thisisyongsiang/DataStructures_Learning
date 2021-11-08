using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace DataStructures_Learning
{
    public class Window2 : GameWindow
    {

//        private readonly float[] _vertices =
//{
//              // positions        // colors
//              0.5f, 0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   //top right
//              0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 1.0f,        // bottom right
//             -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
//              -0.5f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top left
//            };
        float[] _vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,1.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,1.0f,

};
        float[] _recVertices = {
    -1f, -1f, 0f,  0.5f, 0.0f,0.0f,
     -1f, 1f, 0f,   0.5f, 0.0f,0.0f,
     1f,  1f, 0f,  0.5f, 0.0f,0.0f,
    1f,  -1f, 0f,  0.5f, 0.0f,0.0f,


};
        private readonly uint[] _recIndices =
        {
            0,1,2,
            2,3,0
        };
        private readonly uint[] _indices =
        {
            0,1,2,      //tri on bot srf
            2,3,0,      //tri on bot srf
            0,1,5,
            5,4,0,
            1,2,6,
            6,5,1,
            2,3,7,
            7,6,2,
            3,0,4,
            4,7,3,
            4,5,6,      //tri on top srf
            6,7,4       //tri on top srf

        };
//        private readonly uint[] _indices =
//{
//            0,1,
//            1,2,
//            2,3,
//            3,0
//        };

        private int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;
        // We create a double to hold how long has passed since the program was opened.
        private double _time;
        // Then, we create two matrices to hold our view and projection. They're initialized at the bottom of OnLoad.
        // The view matrix is what you might consider the "camera". It represents the current viewport in the window.
        private Matrix4 _view;
        private Matrix4 _model;
        // This represents how the vertices will be projected. It's hard to explain through comments,
        // so check out the web version for a good demonstration of what this does.
        private Matrix4 _projection;

        private Camera _camera;
        private const float _cameraSpeed=3.0f;
        private const float _sensitivity = 0.1f;
        private Vector2 _mouseLastPos;
        private bool _firstMove;
        public Window2(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.PointSize(10.0f);
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            CursorVisible = false;
            CursorGrabbed = true;

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
           
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(_shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(_shader.GetAttribLocation("aPosition"));

            GL.VertexAttribPointer(_shader.GetAttribLocation("aColor"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(_shader.GetAttribLocation("aColor"));

            
            _shader.Use();

            // _camera = new Camera(new Vector3(0.0f, 0.0f, 10f));
            _camera = new Camera(Vector3.UnitZ * 3);
            _view = _camera.GetView();
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _view = _camera.GetView();
            _time += 10.0 * args.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time))*
                Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_time))*
                Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_time));

            //_view = Matrix4.CreateTranslation(0.0f, 0.0f, (float)(10*Math.Sin(_time/1)));
            _shader.SetMatrix4("u_model", _model);
            _shader.SetMatrix4("u_view", _view);
            _shader.SetMatrix4("u_projection", _projection);
            _shader.Use();

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawElements(PrimitiveType.Lines, _indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawElements(PrimitiveType.Points, _indices.Length, DrawElementsType.UnsignedInt, 0);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }
            if (!IsFocused) return;
            var input = KeyboardState;
            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _cameraSpeed * _camera.Direction*(float)args.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position += _cameraSpeed * -_camera.Right * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position += _cameraSpeed * -_camera.Direction * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _cameraSpeed * _camera.Right * (float)args.Time;
            }
            var mouse = MouseState;
            if (_firstMove)
            {
                _firstMove = false;
                _mouseLastPos = new Vector2(mouse.X, mouse.Y);
            }
            else
            {
                var deltaX = mouse.X - _mouseLastPos.X;
                var deltaY = mouse.Y - _mouseLastPos.Y;
                _mouseLastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * _sensitivity;
                _camera.Pitch -= deltaY * _sensitivity;
               // Console.WriteLine(_camera.Direction);
            }


        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, this.Size.X, this.Size.Y);
        }

    }
}
