using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.OpenGl;

namespace Codesanook.Examples.OpenGL
{
    public partial class Form1 : Form
    {
        readonly bool fullscreen = false;
        FastLoop fastLoop;

        public Form1()
        {
            fastLoop = new FastLoop(GameLoop);
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            if (fullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void GameLoop(double elapsedTime)
        {
            Gl.glClearColor(1.0f, 0.0f, 0.0f, 1.0f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glFinish();
            simpleOpenGlControl1.Refresh();
        }
    }
}
