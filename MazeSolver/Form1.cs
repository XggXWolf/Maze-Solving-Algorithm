using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace MazeSolver
{
    public partial class Form1 : Form
    {
        int[,] mazeArr;
        private bool isStarted;
        Point Start;
        Point End;

        double scaleFactor;


        public Form1()
        {
            InitializeComponent();
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
            string path = @imgPath.Text;
            Bitmap img = new Bitmap(path);

            double scalingFactor = Math.Pow(2, trackBar1.Value - 5);
            if(img.Width * scalingFactor > pictureBox1.Width || img.Height * scalingFactor > pictureBox1.Height)
            {
                double maxWidthScale = (double)pictureBox1.Width / img.Width;
                double maxHeightScale = (double)pictureBox1.Height / img.Height;
                double maxAllowedScale = Math.Min(maxWidthScale, maxHeightScale);

                scalingFactor = Math.Min(scalingFactor, maxAllowedScale);

            }

            mazeArr = Image.ConvertImageToBinaryArray(img, scalingFactor);

            Bitmap bitmap = Image.ConvertBinaryArrayToBitmap(mazeArr);
            
            Bitmap resizedBitmap = Image.ResizeBitmapNearestNeighbor(bitmap, pictureBox1.Width, pictureBox1.Height, out scaleFactor);
            

            pictureBox1.Image = resizedBitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null || isStarted) return;

            using (Graphics g = Graphics.FromImage(pictureBox1.Image))
            {

                int size = 5;
                int x = e.X - size / 2;
                int y = e.Y - size / 2;

                if (e.Button == MouseButtons.Left)
                {
                    if (!Start.IsEmpty)
                    {
                        using (Brush brush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(brush, Start.X, Start.Y, size, size);

                        }
                    }

                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        g.FillRectangle(brush, x, y, size, size);
                        startBox.Text = String.Format($"{(int)(x / scaleFactor)},{(int)(y / scaleFactor)}");
                    }

                }
                else
                {

                    if (!End.IsEmpty)
                    {
                        using (Brush brush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(brush, End.X, End.Y, size, size);

                        }
                    }

                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        g.FillRectangle(brush, x, y, size, size);
                        endBox.Text = String.Format($"{(int)(x / scaleFactor)},{(int)(y / scaleFactor)}");
                    }
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


            Solver solver = new Solver(Start, End, mazeArr);
            int[,] solvedArray = solver.Solve();
            Bitmap bitmap = Image.ConvertBinaryArrayToBitmap(solvedArray);
            Bitmap resizedBitmap = Image.ResizeBitmapNearestNeighbor(bitmap, pictureBox1.Width, pictureBox1.Height);
            resizedBitmap.Save("solved.png", System.Drawing.Imaging.ImageFormat.Png);

            pictureBox1.Image = resizedBitmap;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            scaleLabel.Text = String.Format($"Scale: {Math.Pow(2, trackBar1.Value - 5)}x");
        }
    }
}
