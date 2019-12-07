using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

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
            window.Resize += Resize;
            window.RenderFrame += RenderFrame;
            window.Run(1.0 / 60.0);//60 frames in 1 second	
        }

        private void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 50.0, 0.0, 50.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(1.0, 1.0);
            GL.Vertex2(49.0, 1.0);
            GL.Vertex2(25.0, 49.0);
            GL.End();
            window.SwapBuffers();

        }

        private void Load(object sender, EventArgs e)
        {
            GL.ClearColor(1.0F, 0.0F, 0.0F, 0.0F);
        }
    }
}
