using System.Diagnostics;

namespace MazeSolver
{

    struct Node
    {
        public double startCost;
        public double endCost;
        public double totalCost;
        public Point point;

        public Node(double startCost, double endCost, double totalCost, Point point)
        {
            this.startCost = startCost;
            this.endCost = endCost;
            this.totalCost = totalCost;
            this.point = new Point(point.X, point.Y);
        }
    }

    internal class SolverAStar : ISolver
    {


        public Point Start { get; }
        public Point End { get; }
        public int[,] Maze { get; }

        private bool IsSolved;

        public SolverAStar(Point start, Point end, int[,] maze)
        {
            this.Start = start;
            this.End = end;
            this.Maze = maze;
        }

        public int[,] Solve()
        {
            int loopCount = 0;
            Stopwatch timer = new();
            timer.Start();

            var cells = new PriorityQueue<Node, (double, double, double)>();
            var visited = new HashSet<Point>();
            var cameFrom = new Dictionary<Point, Point>();


            double startingSqCost = Euclidean(Start, End);
            Node currentPoint = new(0, startingSqCost, startingSqCost, Start);
            cells.Enqueue(currentPoint, (currentPoint.totalCost, currentPoint.endCost, currentPoint.startCost));

            while (!IsSolved && cells.Count > 0)
            {
                loopCount++;
                currentPoint = cells.Dequeue();

                if (visited.Contains(currentPoint.point))
                    continue;

                visited.Add(currentPoint.point);

                var neighbors = new[]
                {
                        new Point(currentPoint.point.X - 1, currentPoint.point.Y),
                        new Point(currentPoint.point.X + 1, currentPoint.point.Y),
                        new Point(currentPoint.point.X, currentPoint.point.Y - 1),
                        new Point(currentPoint.point.X, currentPoint.point.Y + 1),
                };

                foreach (var neighbor in neighbors)
                {
                    if (!IsWall(neighbor) && !visited.Contains(neighbor))
                    {
                        var calculatedNeighbor = calculatePoint(currentPoint, neighbor);
                        cells.Enqueue(calculatedNeighbor, (calculatedNeighbor.totalCost, calculatedNeighbor.endCost, calculatedNeighbor.startCost));
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

                MessageBox.Show($"Solved in {timer.ElapsedMilliseconds} ms, {loopCount} loops");

            }
            else
            {
                MessageBox.Show($"Maze has no solution. Finished in {timer.ElapsedMilliseconds} ms, {loopCount} loops");
            }



            return Maze;
        }

        private Node calculatePoint(Node currentPoint, Point newPoint)
        {
            double startCost = Euclidean(newPoint, currentPoint.point);
            double endCost = Euclidean(newPoint, End);



            double totalCost = startCost + endCost;

            return new(startCost, endCost, totalCost, newPoint);
        }

        private static double Euclidean(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }


        private bool IsWall(Point point)
        {
            if (point.X < 0 || point.X > Maze.GetLength(1) - 1 || point.Y < 0 || point.Y > Maze.GetLength(0) - 1) return true;

            if (Maze[point.Y, point.X] == 1) return true;

            return false;
        }

    }
}

