using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public static class Extensions
    {
        public static Point Sum(this Point p, Point n)
        {
            return new Point(p.X + n.X, p.Y + n.Y);
        }

    }
}
