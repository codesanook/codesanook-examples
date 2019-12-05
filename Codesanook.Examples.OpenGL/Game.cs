using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codesanook.Examples.OpenGL
{
    public class Game
    {
        private readonly GameWindow window;

        public Game(GameWindow window)
        {
            this.window = window;


            Start();
        }

        private void Start()
        {
            window.Load += Load;
            window.RenderFrame += RenderFrame;

            window.Run();//60 frames in 1 second
        }

        private void RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            window.SwapBuffers();

        }

        private void Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.0F, 0.0F, 0.0F, 0.0F);
        }
    }
}
