using System.Diagnostics;

namespace MazeSolver
{
    public enum CellType
    {
        Path = 0,
        Wall = 1,
        Visited = 2,
        SolutionPath = 3
    }

    public struct Node
    {
        public int startCost;
        public int endCost;
        public int totalCost;
        public Point point;

        public Node(int startCost, int endCost, int totalCost, Point point)
        {
            this.startCost = startCost;
            this.endCost = endCost;
            this.totalCost = totalCost;
            this.point = new Point(point.X, point.Y);
        }
    }

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

            var cells = new PriorityQueue<Node, (int,int)>();
            var visited = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();


            int startingSqCost = Math.Abs(Start.X - End.X) + Math.Abs(Start.Y - End.Y);
            Node currentPoint = new(0, startingSqCost, startingSqCost, Start);
            cells.Enqueue(currentPoint, (currentPoint.totalCost,currentPoint.endCost));

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

                foreach (var neighbor in neighbors)
                {
                    if (!isWall(neighbor) && !visited.Contains(neighbor))
                    {
                        var calculatedNeighbor = calculatePoint(currentPoint, neighbor);
                        cells.Enqueue(calculatedNeighbor, (calculatedNeighbor.totalCost,calculatedNeighbor.endCost));
                        cameFrom[neighbor] = currentPoint.point;
                    }
                }

                Maze[currentPoint.point.Y, currentPoint.point.X] = (int)CellType.Visited;

                if (currentPoint.point == End)
                {
                    IsSolved = true;
                    break;
                }

            }

            timer.Stop();

            if (IsSolved)
            {
                Point current = End;

                while (cameFrom.ContainsKey(current) && current != Start)
                {
                    current = cameFrom[current];
                    if (current != Start)
                    {
                        Maze[current.Y, current.X] = (int)CellType.SolutionPath;
                    }
                }

                MessageBox.Show($"Solved in {timer.ElapsedMilliseconds} ms");

            }
            else
            {
                MessageBox.Show($"Maze has no solution. Finished in {timer.ElapsedMilliseconds} ms");
            }



            return Maze;
        }

        private Node calculatePoint(Node currentPoint, Point newPoint)
        {
            int startCost = currentPoint.startCost + 1;
            int endCost = Math.Abs(newPoint.X - End.X) + Math.Abs(newPoint.Y - End.Y);

            double priority = 0.001 * endCost;

            int totalCost = startCost + endCost + (int)(priority * 1000);

            return new(startCost, endCost, totalCost, newPoint);
        }

        private bool isWall(Point point)
        {
            if (point.X < 0 || point.X > Maze.GetLength(1) - 1 || point.Y < 0 || point.Y > Maze.GetLength(0) - 1) return true;

            if (Maze[point.Y, point.X] == 1) return true;

            return false;
        }

    }
}

