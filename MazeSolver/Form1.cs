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


    public partial class Form1 : Form
    {
        bool limitImageSize = false;

        int[,] mazeArr;
        int selectedAlgorithm = 0;

        Point originalStart;
        Point originalEnd;

        Size pictureBoxOriginalSize;

        Point Start;
        Point End;

        double scaleFactor;


        public Form1()
        {
            InitializeComponent();
            pictureBoxOriginalSize = pictureBox1.Size;
        }


        private void Print2DArray(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                Debug.WriteLine("");
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Debug.Write($"{arr[i, j]}");
                }
            }
        }

        private void convertImage_Click(object sender, EventArgs e)
        {
            Convert();
        }

        private void Convert()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox1.Size = pictureBoxOriginalSize;
            panel1.Size = pictureBoxOriginalSize + new Size(6, 6);

            Debug.WriteLine(panel1.Size);



            Start = Point.Empty;
            End = Point.Empty;

            string path = @imgPath.Text;
            Bitmap img = new Bitmap(path);

            scaleFactor = Math.Pow(2, trackBar1.Value - 5);
            if (limitImageSize && (img.Width * scaleFactor > pictureBox1.Width || img.Height * scaleFactor > pictureBox1.Height))
            {
                double maxWidthScale = (double)pictureBox1.Width / img.Width;
                double maxHeightScale = (double)pictureBox1.Height / img.Height;
                double maxAllowedScale = Math.Min(maxWidthScale, maxHeightScale);

                scaleFactor = Math.Min(scaleFactor, maxAllowedScale);

            }

            mazeArr = Image.ConvertImageToBinaryArray(img, scaleFactor);

            Bitmap bitmap = Image.ConvertBinaryArrayToBitmap(mazeArr);

            if (limitImageSize || (img.Width * scaleFactor < pictureBox1.Width || img.Height * scaleFactor < pictureBox1.Height))
            {
                bitmap = Image.ResizeBitmapNearestNeighbor(bitmap, pictureBox1.Width, pictureBox1.Height, out scaleFactor);
            }


            if (!limitImageSize)
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;


            pictureBox1.Image = bitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {


            if (pictureBox1.Image == null) return;

            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {

                int size = 5;
                int x = e.X - size / 2;
                int y = e.Y - size / 2;



                if (e.Button == MouseButtons.Left)
                {
                    if (!originalStart.IsEmpty)
                    {
                        using (Brush brush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(brush, originalStart.X, originalStart.Y, size, size);
                            Debug.WriteLine(originalStart);
                            Debug.WriteLine(x);

                        }
                    }

                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        g.FillRectangle(brush, x, y, size, size);
                        startBox.Text = String.Format($"{(int)(x / scaleFactor)},{(int)(y / scaleFactor)}");
                        Debug.WriteLine(scaleFactor);
                    }

                    originalStart = new Point(x, y);
                }
                else
                {

                    if (!originalEnd.IsEmpty)
                    {
                        using (Brush brush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(brush, originalEnd.X, originalEnd.Y, size, size);

                        }
                    }

                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        g.FillRectangle(brush, x, y, size, size);
                        endBox.Text = String.Format($"{(int)(x / scaleFactor)},{(int)(y / scaleFactor)}");
                    }

                    originalEnd = new Point(x, y);
                }

            }

            pictureBox1.Invalidate();
            pictureBox1.Refresh();
        }

        private void solveButton_Click(object sender, EventArgs e)
        {


            string[] startString = startBox.Text.Split(',');
            Start.X = int.Parse(startString[0]);
            Start.Y = int.Parse(startString[1]);

            string[] endString = endBox.Text.Split(',');
            End.X = int.Parse(endString[0]);
            End.Y = int.Parse(endString[1]);

            ISolver solver;

            switch (selectedAlgorithm)
            {
                case 1:
                    solver = new SolverAStar(Start, End, mazeArr);
                    break;
                default:
                    solver = new SolverBFS(Start, End, mazeArr);
                    break;
            }

            Debug.WriteLine(solver.GetType());
            int[,] solvedArray = solver.Solve();
            Bitmap bitmap = Image.ConvertBinaryArrayToBitmap(solvedArray);
            Bitmap resizedBitmap = Image.ResizeBitmapNearestNeighbor(bitmap, pictureBox1.Width, pictureBox1.Height);

            bitmap.Save("solution.png", System.Drawing.Imaging.ImageFormat.Png);
            Process.Start(new ProcessStartInfo("solution.png") { UseShellExecute = true });


            pictureBox1.Image = resizedBitmap;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            scaleLabel.Text = String.Format($"Scale: {Math.Pow(2, trackBar1.Value - 5)}x");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                panel1.Size = pictureBoxOriginalSize + new Size(6, 6);
                limitImageSize = true;


            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                panel1.Size = pictureBoxOriginalSize + new Size(6, 6);
                limitImageSize = false;

            }

            if (pictureBox1.Image != null)
            {
                Debug.WriteLine(pictureBox1.Size);
                Convert();
            }
        }

        private void AlgorithmList_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAlgorithm = AlgorithmList.SelectedIndex;
            Debug.WriteLine(selectedAlgorithm);
        }
    }
}
