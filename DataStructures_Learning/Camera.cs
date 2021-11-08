using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace DataStructures_Learning
{
    public class Camera
    {
        private Vector3 _up=Vector3.UnitY;
        //positive z axis is pointing towards the screen
        private Vector3 _direction = -Vector3.UnitZ;
        private Vector3 _right=Vector3.UnitX;
        private float _pitch = 0.0f;
        private float _yaw =-MathHelper.PiOver2;
        public Vector3 Up => _up;
        public Vector3 Direction => _direction;
        public Vector3 Position { get; set; }
        public Vector3 Right => _right;
        public Camera(Vector3 Position)
        {
            this.Position = Position;
            UpdateCamera();
        }

        /// <summary>
        /// Get the View Matrix using inbuilt lookat method from opentk
        /// </summary>
        /// <param name="Target"></param>
        /// <returns></returns>
        public Matrix4 GetView()
        {
            Matrix4 view = Matrix4.LookAt(Position, Position+Direction, Up);
            return view;
        }
        //public Matrix4 GetProjection()
        //{

        //}
        public float Pitch
        {
            get { return MathHelper.RadiansToDegrees(_pitch); }
            set {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateCamera();
            }
        }
        public float Yaw
        {
            get { return MathHelper.RadiansToDegrees(_yaw); }
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateCamera();
            }
        }
         
        //public float Roll { get; set; }
        private void UpdateCamera()
        {

            _direction = Matrix3.CreateRotationX(_pitch)*Matrix3.CreateRotationY(-_yaw)*-Vector3.UnitZ;
            Console.WriteLine("pitch");
            Console.WriteLine(_pitch);
            Console.WriteLine("len");
            Console.WriteLine(_direction.Length);
            Console.WriteLine(_direction);
            _direction = Vector3.Normalize(_direction);

            _right = Vector3.Normalize(Vector3.Cross(_direction, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _direction));
        }
    }
}
