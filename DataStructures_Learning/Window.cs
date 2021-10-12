using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace DataStructures_Learning
{
    public class Window : GameWindow
    {

        public Window(GameWindowSettings gameWindowSettings,NativeWindowSettings nativeWindowSettings)
            :base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            base.OnUpdateFrame(args);
        }
    }
}
