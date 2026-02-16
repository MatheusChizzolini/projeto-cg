using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCG
{
    public struct Rgb
    {
        public byte R, G, B;
    }

    public struct Cmy
    {
        public double C, M, Y;
    }

    public struct Hsi
    {
        public int H;
        public double S, I;
    }

    class Conversions
    {
        public Rgb[,] RgbMatrix;
        public Cmy[,] CmyMatrix;
        public Hsi[,] HsiMatrix;
        public int width;
        public int height;

        public Conversions(Bitmap bitmap)
        {
            width = bitmap.Width;
            height = bitmap.Height;
            RgbMatrix = new Rgb[width, height];
            CmyMatrix = new Cmy[width, height];
            HsiMatrix = new Hsi[width, height];
        }

        public void UpdateMatrices(Bitmap bitmap)
        {
            GenerateRgbMatrix(bitmap);
            GenerateCmyMatrix(bitmap);
            GenerateHsiMatrix(bitmap);
        }

        private void GenerateRgbMatrix(Bitmap bitmap)
        {
            int tamanhoPixel = 3;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int stride = bitmapData.Stride;

            unsafe
            {
                byte* origem = (byte*) bitmapData.Scan0.ToPointer();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte* atual = origem + y * stride + x * tamanhoPixel;
                        RgbMatrix[x, y].B = atual[0];
                        RgbMatrix[x, y].G = atual[1];
                        RgbMatrix[x, y].R = atual[2];
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
        }
        
        private void GenerateCmyMatrix(Bitmap bitmap)
        {
            int tamanhoPixel = 3;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int stride = bitmapData.Stride;

            unsafe
            {
                byte* origem = (byte*) bitmapData.Scan0.ToPointer();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte* atual = origem + y * stride + x * tamanhoPixel;
                        CmyMatrix[x, y].Y = 1 - (atual[0] / 255.0);
                        CmyMatrix[x, y].M = 1 - (atual[1] / 255.0);
                        CmyMatrix[x, y].C = 1 - (atual[2] / 255.0);
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

        private void GenerateHsiMatrix(Bitmap bitmap)
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

                        int rgbSum = atual[0] + atual[1] + atual[2];
                        if (rgbSum == 0)
                        {
                            HsiMatrix[x, y].H = 0;
                            HsiMatrix[x, y].S = 0;
                            HsiMatrix[x, y].I = 0;
                        }
                        else
                        {
                            double b = (double)atual[0] / rgbSum;
                            double g = (double)atual[1] / rgbSum;
                            double r = (double)atual[2] / rgbSum;

                            HsiMatrix[x, y].I = rgbSum / (3 * 255.0);
                            HsiMatrix[x, y].S = 1 - 3 * Math.Min(r, Math.Min(g, b));
                            double numerator = 0.5 * ((r - g) + (r - b));
                            double denominator = Math.Pow(Math.Pow(r - g, 2) + (r - b) * (g - b), 0.5);
                            if (denominator == 0)
                            {
                                HsiMatrix[x, y].H = 0;
                            }
                            else
                            {
                                double h = Math.Acos(numerator / denominator);
                                if (b > g)
                                {
                                    h = 2 * Math.PI - h;
                                }

                                HsiMatrix[x, y].H = (int)(h * 180 / Math.PI);
                            }
                        }
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

        public Rgb[,] ConvertHsiMatrixToRgb()
        {
            Rgb[,] newRgbMatrix = new Rgb[width, height];
            for (int l = 0; l < height; l++)
            {
                for (int c = 0; c < width; c++)
                {
                    double h = HsiMatrix[c, l].H * Math.PI / 180;
                    double s = HsiMatrix[c, l].S;
                    double i = HsiMatrix[c, l].I;
                    double r, g, b;

                    double x, y, z;
                    if (h < 2 * Math.PI / 3)
                    {
                        x = i * (1 - s);
                        y = i * (1 + (((s * Math.Cos(h)) / Math.Cos(Math.PI / 3 - h))));
                        z = 3 * i - (x + y);
                        b = x;
                        r = y;
                        g = z;
                    }
                    else if (h < 4 * Math.PI / 3)
                    {
                        h -= 2 * Math.PI / 3;
                        x = i * (1 - s);
                        y = i * (1 + (((s * Math.Cos(h)) / Math.Cos(Math.PI / 3 - h))));
                        z = 3 * i - (x + y);
                        r = x;
                        g = y;
                        b = z;
                    }
                    else
                    {
                        h -= 4 * Math.PI / 3;
                        x = i * (1 - s);
                        y = i * (1 + (((s * Math.Cos(h)) / Math.Cos(Math.PI / 3 - h))));
                        z = 3 * i - (x + y);
                        r = z;
                        g = x;
                        b = y;
                    }

                    newRgbMatrix[c, l].R = (byte)Math.Max(0, Math.Min(255, r * 255));
                    newRgbMatrix[c, l].G = (byte)Math.Max(0, Math.Min(255, g * 255));
                    newRgbMatrix[c, l].B = (byte)Math.Max(0, Math.Min(255, b * 255));
                }
            }

            return newRgbMatrix;
        }
    }
}
