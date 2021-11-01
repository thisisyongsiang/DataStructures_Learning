using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace DataStructures_Learning
{
    public class Shader :IDisposable
    {
        //location of the shader program at the end after compilation
        int _handle;

        private bool disposedValue=false;

        public Shader(string vertexPath,string fragmentPath)
        {
            //loading shaders from individual shader files
            string vertexShaderSource= File.ReadAllText(vertexPath);
            string fragmentShaderSource= File.ReadAllText(fragmentPath); ;
            //using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            //{
            //    vertexShaderSource = reader.ReadToEnd();
            //}
            //using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            //{
            //    fragmentShaderSource = reader.ReadToEnd();
            //}

            //Generating shaders
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            //Binding the source code to the shaders
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            //Compiling shaders and checking for errors
            //getshaderinfolog gives the debug string if theres error during compiling
            GL.CompileShader(vertexShader);
            string infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVert != System.String.Empty) 
                Console.WriteLine(infoLogVert);
            
            GL.CompileShader(fragmentShader);
            string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFrag != System.String.Empty)
                Console.WriteLine(infoLogFrag);

            //After compiling, to use the shaders, we need to link them together into a program that can be run on the GPU
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            GL.LinkProgram(_handle);

            //Once linked, we can remove the shaders as they are useless now
            //Compiled data is copied to the shader program when linked. We dont need the individual shaders that are attached to the program.
            //Detach and delete the shaders
            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        //Method to use the shader
        public void Use()
        {

            GL.UseProgram(_handle);
        }
        //if you wish not to hardocde the location of the variable in shader.vert
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }
        //Dispose Pattern
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                GL.DeleteProgram(_handle);
                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(_handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
