using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MazeSolver
{
    internal class Image
    {
        // This part was made by ChatGPT as i do not know how to use bitmaps yet.
        public static int[,] ConvertImageToBinaryArray(Bitmap image, double scale)
        {
            using Bitmap originalBitmap = new Bitmap(image);

            Bitmap bitmapToProcess = scale != 1d
                   ? ResizeBitmapNearestNeighbor(originalBitmap, scale)
                   : new Bitmap(originalBitmap);

            int width = bitmapToProcess.Width;
            int height = bitmapToProcess.Height;

            int[,] result = new int[height, width];

            // Lock the bitmap's bits  
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bitmapToProcess.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;
            int stride = bmpData.Stride;
            int bytes = Math.Abs(stride) * height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array  
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int y = 0; y < height; y++)
            {
                int rowStart = y * stride;
                for (int x = 0; x < width; x++)
                {
                    int pixelIndex = rowStart + x * 3;
                    byte blue = rgbValues[pixelIndex];
                    byte green = rgbValues[pixelIndex + 1];
                    byte red = rgbValues[pixelIndex + 2];

                    if (IsWhite(red, green, blue))
                        result[y, x] = 0;
                    else
                        result[y, x] = 1;
                }
            }

            bitmapToProcess.UnlockBits(bmpData);
            bitmapToProcess.Dispose();

            return result;
        }

        public static Bitmap ResizeBitmapNearestNeighbor(Bitmap original, int targetWidth, int targetHeight, out double scalingFactor)
        {
            float scaleX = (float)targetWidth / original.Width;
            float scaleY = (float)targetHeight / original.Height;
            float scale = Math.Min(scaleX, scaleY); // allow downscaling

            int newWidth = (int)(original.Width * scale);
            int newHeight = (int)(original.Height * scale);

            Bitmap resized = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            scalingFactor = (double)scale;
            return resized;
        }

        public static Bitmap ResizeBitmapNearestNeighbor(Bitmap original, int targetWidth, int targetHeight)
        {
            double temp;
            Bitmap output = ResizeBitmapNearestNeighbor(original, targetWidth, targetHeight, out temp);

            return output;
        }


        public static Bitmap ResizeBitmapNearestNeighbor(Bitmap original, double scale)
        {

            int newWidth = Math.Max(1, (int)(original.Width * scale));
            int newHeight = Math.Max(1, (int)(original.Height * scale));


            Bitmap resized = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return resized;
        }



        public static Bitmap ConvertBinaryArrayToBitmap(int[,] binaryArray)
        {
            int height = binaryArray.GetLength(0);
            int width = binaryArray.GetLength(1);

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = binaryArray[y, x] switch
                    {
                        0 => Color.White,
                        1 => Color.Black,
                        2 => Color.LightGray,
                        3 => Color.Green,
                        4 => Color.Red,
                        _ => Color.Magenta // fallback for unexpected values
                    };
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

        private static bool IsWhite(byte r, byte g, byte b, int tolerance = 30)
        {
            return r >= 255 - tolerance &&
                   g >= 255 - tolerance &&
                   b >= 255 - tolerance;
        }
    }
}
