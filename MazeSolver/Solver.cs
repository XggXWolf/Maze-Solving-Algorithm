using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MazeSolver
{
    internal class Solver
    {
        public Point Start { get; }
        public Point End { get; }
        public int[,] Maze { get; }

        private bool IsSolved;

        public Solver(Point start, Point end, int[,] maze)
        {
            this.Start = start;
            this.End = end;
            this.Maze = maze;
        }

        public int[,] Solve()
        {
            Stopwatch timer = new();
            timer.Start();

            var cells = new PriorityQueue<(int startCost, int endCost, int totalCost, Point point), int>();
            var visited = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();


            int startingSqCost = Math.Abs(Start.X - End.X) + Math.Abs(Start.X - End.X);
            (int startCost, int endCost, int totalCost, Point point) currentPoint = (0, startingSqCost, startingSqCost, Start);
            cells.Enqueue(currentPoint, currentPoint.totalCost);

            while (!IsSolved && cells.Count > 0)
            {
                currentPoint = cells.Dequeue();

                if (visited.Contains(currentPoint.point))
                    continue;
                
                visited.Add(currentPoint.point);

                var neighbors = new[]
                {
                        new Point(currentPoint.point.X, currentPoint.point.Y - 1),
                        new Point(currentPoint.point.X, currentPoint.point.Y + 1),
                        new Point(currentPoint.point.X - 1, currentPoint.point.Y),
                        new Point(currentPoint.point.X + 1, currentPoint.point.Y)
                };

                foreach(var neighbor in neighbors)
                {
                    if(!isWall(neighbor) && !visited.Contains(neighbor))
                    {
                        var calculatedNeighbor = calculatePoint(currentPoint, neighbor);
                        cells.Enqueue(calculatedNeighbor, calculatedNeighbor.totalCost);
                        cameFrom[neighbor] = currentPoint.point;
                    }
                }

                Maze[currentPoint.point.Y, currentPoint.point.X] = 2;

                if(currentPoint.point == End)
                {
                    IsSolved = true;
                    break;
                }
            }

            if (IsSolved)
            {
                Point current = End;

                while (cameFrom.ContainsKey(current) && current != Start)
                {
                    current = cameFrom[current];
                    if (current != Start)
                    {
                        Maze[current.Y, current.X] = 3;
                    }
                }
            }
            timer.Stop();

            MessageBox.Show($"Solved in {timer.ElapsedMilliseconds} ms");

            return Maze;
        }

        private (int startCost, int endCost, int totalCost, Point point) calculatePoint((int startCost, int endCost, int totalCost, Point point) currentPoint, Point newPoint)
        {
            int startCost = currentPoint.startCost + 1;
            int endCost = Math.Abs(newPoint.X - End.X) + Math.Abs(newPoint.Y - End.Y);
            int totalCost = startCost + endCost;

            if (isWall(newPoint))
            {
                totalCost = int.MaxValue;
            }

            return (startCost, endCost, totalCost, newPoint);
        }

        private bool isWall(Point point)
        {
            if (point.X < 0 || point.X > Maze.GetLength(1) - 1 || point.Y < 0 || point.Y > Maze.GetLength(0) - 1) return true;

            if (Maze[point.Y, point.X] == 1) return true;

            return false;
        }

    }
}

