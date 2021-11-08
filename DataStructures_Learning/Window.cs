using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace DataStructures_Learning
{
    public class Window : GameWindow
    {
        // These are the handles to OpenGL objects. A handle is an integer representing where the object lives on the
        // graphics card. Consider them sort of like a pointer; we can't do anything with them directly, but we can
        // send them to OpenGL functions that need them.

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private Stopwatch _timer;
        private Shader _shader;
        float[] vertices = {
    -0.5f, -0.5f, 0.0f, //Bottom-left vertex
     0.5f, -0.5f, 0.0f, //Bottom-right vertex
     0.0f,  0.5f, 0.0f  //Top vertex
        };
        private readonly float[] _vertices =
  {
      // positions        // colors
      0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // bottom right
     -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // bottom left
      0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // top 
    };

        //        float[] vertices = {
        //     0.5f,  0.5f, 0.0f,  // top right
        //     0.5f, -0.5f, 0.0f,  // bottom right
        //    -0.5f, -0.5f, 0.0f,  // bottom left
        //    -0.5f,  0.5f, 0.0f   // top left
        //};
        //uint[] indices = {  // note that we start from 0!
        //    0, 1, 3,   // first triangle
        //    1, 2, 3    // second triangle
        //};
        uint[] indices = {  // note that we start from 0!
    0, 1, 2   // first triangle
};
        public Window(GameWindowSettings gameWindowSettings,NativeWindowSettings nativeWindowSettings)
            :base(gameWindowSettings, nativeWindowSettings)
        {
        }

        //This Function runs on every update frame
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(args);
        }

        //On Load Event Set Background color
        protected override void OnLoad()
        {
            base.OnLoad();
            //int nrAttributes;
            //GL.Enable(EnableCap.Blend);
            //GL.Enable(EnableCap.LineSmooth);
            //GL.Enable(EnableCap.PolygonSmooth);
            GL.PointSize(10.0f);
            //GL.GetInteger(GetPName.MaxVertexAttribs, out nrAttributes);
            //Console.WriteLine("Maximum number of vertex attributes supported: " + nrAttributes);
            //Clears the background, and sets input color value.
            //Parameters for color is a normalized value of Red Green Blue and Alpha.
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            // First, we need to create a buffer. This function returns a handle to it, but as of right now, it's empty.
            _vertexBufferObject = GL.GenBuffer();
            // Now, bind the buffer. OpenGL uses one global state, so after calling this,
            // all future calls that modify the VBO will be applied to this buffer until another buffer is bound instead.
            // The first argument is an enum, specifying what type of buffer we're binding. A VBO is an ArrayBuffer.
            // There are multiple types of buffers, but for now, only the VBO is necessary.
            // The second argument is the handle to our buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            //Buffer usage hint is to specify how we wnat the graphics card to manage the data
            //StaticDraw: the data will most likely not change at all or very rarely.
            //DynamicDraw: the data is likely to change a lot.
            //StreamDraw: the data will change every time it is drawn.
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            //Creating the shaders to tell opengl how to turn the vertice into pixels
            // Shaders are tiny programs that live on the GPU. OpenGL uses them to handle the vertex-to-pixel pipeline.
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");


            //The buffer that we just loaded doesnt have structure. its just an array of floats. opengl doesnt know how this data should be interpreted
            //opengl created vertex array object VAO which keeps track of what parts or what buffers correspond to what data
            //we setup our VAO and bind it.
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            //Linking Vertex Attributes
            //gl vertexattribpointer function tells opengl the format of the data, and associates the current array buffer with the vertexarrayobject
            //this call sets the attribute to source data from current array buffer
            GL.VertexAttribPointer(_shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(_shader.GetAttribLocation("aPosition"));

            GL.VertexAttribPointer(_shader.GetAttribLocation("aColor"), 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3*sizeof(float));
            GL.EnableVertexAttribArray(_shader.GetAttribLocation("aColor"));
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            _shader.Use();
            _timer = new Stopwatch();
            _timer.Start();
        }

        //Render Loop
        protected override void OnRenderFrame(FrameEventArgs args)
        {

            base.OnRenderFrame(args);
            double timeValue = _timer.Elapsed.TotalSeconds;
            double multiplier = 10.0;
            float gValue = (float)Math.Sin(timeValue * multiplier) / 2.0f + 0.5f;
            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "u_Color");
            //GL.PointSize((float)(gValue * 10.0));
            //GL.Uniform4(vertexColorLocation, 0.0f, gValue, 0.0f, 0.2f);

            //Clears the image, using what was set at GL.ClearColor
            //OpenGL has multiple kind of data that can be rendered
            //So you may need to clear multiple kind of buffers, and can be achieved by using multiple bit flags.
            //However, here the modification is color, so color buffer is the only thing required to be cleared.
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _shader.Use();
            //Bind the VAO
            GL.BindVertexArray(_vertexArrayObject);
            //Call the drawing function
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawElements(PrimitiveType.Points, indices.Length, DrawElementsType.UnsignedInt, 0);
           // GL.DrawElements(PrimitiveType.LineLoop, indices.Length, DrawElementsType.UnsignedInt, 0);
            //OpenGL has 2 buffers "double-buffered" that is managed by the window.
            //One is rendered to, while the other is currently displayed by the window.
            //This is to avoid screen tearing, Something that happens if the buffer is modified while being displayed
            //When we are done drawing, We need to swapbuffers to allow the one that is being rendered to be displayed. 
            //Otherwise what was rendered will not be displayed.
            SwapBuffers();
        }

        //This function runs everytime the window is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            //This matches the openGL's viewport to match the new size, to ensure that the "Normalized Device Coordinates"(NDC) is correct
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        //After program ends, we ned to cleannup the buffers
        // You should generally not do cleanup of opengl resources when exiting an application
        // as that is handled by the driver and operating system when the application exits.
        // 
        // There are reasons to delete opengl resources but exiting the application is not one of them.
        // This is provided here as a reference on how resoruce cleanup is done in opengl but
        // should not be done when exiting the application.
        //
        // Places where cleanup is appropriate would be to delete textures that are no
        // longer used for whatever reason (e.g. a new scene is loaded that doesn't use a texture).
        // This would free up video ram (VRAM) that can be used for new textures.
        //
        protected override void OnUnload()
        {
            base.OnUnload();
            //binding buffer to 0 sets the buffer to null so any calls that modify the buffer without binding will result in crash
            //this is for ease of debug
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            _shader.Dispose();

        }
    }
}
