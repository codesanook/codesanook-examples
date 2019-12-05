using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Codesanook.Examples.OpenGL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var window = new GameWindow(500, 500);
            var game =new Game(window);
        }
    }
}
