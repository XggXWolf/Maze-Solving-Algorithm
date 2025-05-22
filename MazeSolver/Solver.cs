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
            (int totalCost, Point point) currentPoint = (Math.Abs(Start.X - End.X) + Math.Abs(Start.Y - End.Y), Start);
            


            (int cost, bool visited)[,] costArray = new(int,bool)[Maze.GetLength(0), Maze.GetLength(1)];

            costArray[currentPoint.point.Y, currentPoint.point.X].visited = true;

            for (int i = 0; i < costArray.GetLength(0); i++)
            {
                for (int j = 0; j < costArray.GetLength(1); j++)
                {
                    costArray[i, j].cost = int.MaxValue;
                }
            }


            while (!IsSolved)
            {
                var top = calculateCost(currentPoint ,new Point(currentPoint.point.X, currentPoint.point.Y - 1));
                var bottom = calculateCost(currentPoint, new Point(currentPoint.point.X, currentPoint.point.Y + 1));
                var left = calculateCost(currentPoint ,new Point(currentPoint.point.X - 1, currentPoint.point.Y));
                var right = calculateCost(currentPoint, new Point(currentPoint.point.X + 1, currentPoint.point.Y));

                Debug.WriteLine("  " + top + "  " + bottom + "  " + left + "  " + right + "  ");

                var smallestCost = top;

                SetCost(ref costArray, ref top, ref smallestCost);
                SetCost(ref costArray, ref bottom, ref smallestCost);
                SetCost(ref costArray, ref left, ref smallestCost);
                SetCost(ref costArray, ref right, ref smallestCost);

                if(smallestCost.totalCost <= currentPoint.totalCost && !costArray[currentPoint.point.Y, currentPoint.point.X].visited)
                    currentPoint = smallestCost;
                else
                    currentPoint = findSmallest(costArray);

                if(currentPoint.point == End)
                {
                    IsSolved = true;
                    break;
                }

                costArray[currentPoint.point.Y, currentPoint.point.X].visited = true;

            }

            mergeArray(costArray);

            return Maze;
        }

        private void mergeArray((int cost, bool visited)[,] costArray)
        {

            for (int i = 0; i < costArray.GetLength(0); i++)
            {
                for (int j = 0; j < costArray.GetLength(1); j++)
                {
                    if (costArray[i, j].visited)
                    {
                        Maze[i, j] = 2;
                    }
                }
            }
            Maze[Start.Y, Start.X] = 3;
            Maze[End.Y, End.X] = 4;

        }

        private(int, Point) findSmallest((int cost, bool visited)[,] costArray)
        {
            ((int cost, bool visited) arr, Point coord) smallest = (costArray[0, 0], new Point(0, 0));

            for (int i = 0; i < costArray.GetLength(0); i++)
            {
                for (int j = 0; j < costArray.GetLength(1); j++)
                {
                    if (costArray[i,j].cost < smallest.arr.cost && !costArray[i,j].visited)
                    {
                        smallest = (costArray[i, j], new Point(j, i));
                    }
                }
            }

            return (smallest.arr.cost, smallest.coord);
        }

        private void SetCost(ref (int cost ,bool visited)[,] costArray, ref (int cost ,Point coord) point, ref (int cost, Point coord) smallestVar)
        {
            if (isWall(point.coord))
                point.cost = int.MaxValue;
            costArray[point.coord.Y, point.coord.X].cost = point.cost;

            if(point.cost < smallestVar.cost)
            {
                smallestVar = point;
            }
        }

        private bool isWall(Point point)
        {
            if (point.Y < 0 || point.Y > Maze.GetLength(0) - 1 || point.X < 0 || point.X > Maze.GetLength(1) - 1) return false;

            if (Maze[point.Y, point.X] == 1) return true;

            else return false;
        }

        private (int totalCost, Point point) calculateCost((int currentCost, Point point) currentPoint, Point point)
        {
            int startCost = Math.Abs(currentPoint.point.X - point.X) + Math.Abs(currentPoint.point.Y - point.Y);
            int endCost = Math.Abs(End.X - point.X) + Math.Abs(End.Y - point.Y);


            return (startCost + endCost, point);
        }
    }
}

