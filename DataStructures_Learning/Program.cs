using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace DataStructures_Learning
{
    public class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Hello World Learn OpenTK Window",
                //this is required to run on macos
                Flags = ContextFlags.ForwardCompatible
            };

            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }

        }
    }
}
