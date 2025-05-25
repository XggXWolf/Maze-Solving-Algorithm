using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    internal interface ISolver
    {
        public Point Start { get; }
        public Point End { get; }
        public int[,] Maze { get; }

        int[,] Solve();
    }
}
