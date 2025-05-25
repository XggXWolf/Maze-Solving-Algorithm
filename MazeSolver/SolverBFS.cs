using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    internal class SolverBFS : ISolver
    {
        public Point Start { get; }
        public Point End { get; }
        public int[,] Maze { get; }

        private bool isSolved;

        public SolverBFS(Point start, Point end, int[,] maze)
        {

            this.Start = start;
            this.End = end;
            this.Maze = maze;
        }

        public int[,] Solve()
        {
;
            Stopwatch time = new();
            time.Start();

            Point[] neighbors = [
                    new Point(0, -1),
                    new Point(0, 1),
                    new Point(-1, 0),
                    new Point(1, 0),

                    new Point(1, 1),
                    new Point(-1,-1),
                    new Point(1,-1),
                    new Point(-1,1)
            ];

            var currentPoint = Start;
            Maze[currentPoint.Y, currentPoint.X] = 2;
            
            var cellsQueue = new Queue<Point>();
            cellsQueue.Enqueue(currentPoint);

            var cameFrom = new Point[Maze.GetLength(0), Maze.GetLength(1)];

            while(!isSolved && cellsQueue.Count > 0)
            {
                currentPoint = cellsQueue.Dequeue();

                if(currentPoint == End)
                {
                    isSolved = true;
                    break;
                }



                foreach(var neighbor in neighbors)
                {
                    Point addedNeighbor = currentPoint.Sum(neighbor);

                    if (addedNeighbor.X >= 0 && addedNeighbor.X < Maze.GetLength(1) &&
                        addedNeighbor.Y >= 0 && addedNeighbor.Y < Maze.GetLength(0) &&
                        Maze[addedNeighbor.Y, addedNeighbor.X] == 0)
                    {
                        cellsQueue.Enqueue(addedNeighbor);
                        Maze[addedNeighbor.Y, addedNeighbor.X] = 2;
                        cameFrom[addedNeighbor.Y, addedNeighbor.X] = currentPoint;
                    }
                }

                
            }
            
            while(currentPoint != Start && isSolved)
            {
                Point cameFromPoint = cameFrom[currentPoint.Y, currentPoint.X];
                
                Maze[cameFromPoint.Y, cameFromPoint.X] = 3;
                currentPoint = cameFromPoint;
            }

            time.Stop();
            MessageBox.Show($"Solved in {time.ElapsedMilliseconds} ms");
            return Maze;
        }
    }
}
