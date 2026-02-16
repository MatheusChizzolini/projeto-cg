using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCG
{
    public struct RGB
    {
        public byte R, G, B;
    }

    class Conversions
    {
        public RGB[,] RGBMatrix;
        public int width { get; private set; }
        public int height { get; private set; }

        public Conversions(Bitmap bitmap)
        {
            width = bitmap.Width;
            height = bitmap.Height;
            RGBMatrix = new RGB[width, height];

            GenerateRGBMatrix(bitmap);
        }

        private void GenerateRGBMatrix(Bitmap bitmap)
        {
            int tamanhoPixel = 3;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int stride = bitmapData.Stride;

            unsafe
            {
                byte* origem = (byte*)bitmapData.Scan0.ToPointer();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte* atual = origem + y * stride + x * tamanhoPixel;
                        RGBMatrix[x, y].B = atual[0];
                        RGBMatrix[x, y].G = atual[1];
                        RGBMatrix[x, y].R = atual[2];
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

    }
}
