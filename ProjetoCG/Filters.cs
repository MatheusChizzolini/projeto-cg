using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCG
{
    class Filters
    {
        public static void Luminancia(Bitmap bitmapOrigem)
        {
            int altura = bitmapOrigem.Height;
            int largura = bitmapOrigem.Width;
            int tamanhoPixel = 3;

            BitmapData bitmapDataOrigem = bitmapOrigem.LockBits(
                new Rectangle(0, 0, largura, altura),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int stride = bitmapDataOrigem.Stride;

            unsafe
            {
                byte* origem = (byte*) bitmapDataOrigem.Scan0.ToPointer();
                int r, g, b;
                for (int y = 0; y < altura; y++)
                {
                    for (int x = 0; x < largura; x++)
                    {
                        byte* atual = origem + y * stride + x * tamanhoPixel;

                        b = atual[0];
                        g = atual[1];
                        r = atual[2];
                        int grayScale = (int)(r * 0.299 + g * 0.587 + b * 0.114);

                        atual[0] = (byte)grayScale;
                        atual[1] = (byte)grayScale;
                        atual[2] = (byte)grayScale;
                    }
                }
            }

            bitmapOrigem.UnlockBits(bitmapDataOrigem);
        }

        public static void ChangeHue(Bitmap bitmapOrigem,  Rgb[,] rbgMatrix)
        {
            int altura = bitmapOrigem.Height;
            int largura = bitmapOrigem.Width;
            int tamanhoPixel = 3;

            BitmapData bitmapDataOrigem = bitmapOrigem.LockBits(
                new Rectangle(0, 0, largura, altura),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int stride = bitmapDataOrigem.Stride;

            unsafe
            {
                byte* origem = (byte*)bitmapDataOrigem.Scan0.ToPointer();
                for (int y = 0; y < altura; y++)
                {
                    for (int x = 0; x < largura; x++)
                    {
                        byte* atual = origem + y * stride + x * tamanhoPixel;

                        atual[0] = rbgMatrix[x, y].B;
                        atual[1] = rbgMatrix[x, y].G;
                        atual[2] = rbgMatrix[x, y].R;
                    }
                }
            }

            bitmapOrigem.UnlockBits(bitmapDataOrigem);
        }

        public static void SegmentHue(Bitmap bitmap, Hsi[,] hsiMatrix, int minHue, int maxHue)
        {
            int altura = bitmap.Height;
            int largura = bitmap.Width;
            int tamanhoPixel = 3;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, largura, altura),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            int stride = bitmapData.Stride;

            unsafe
            {
                byte* origem = (byte*)bitmapData.Scan0.ToPointer();
                int r, g, b;
                for (int y = 0; y < altura; y++)
                {
                    for (int x = 0; x < largura; x++)
                    {
                        int h = hsiMatrix[x, y].H;
                        if (minHue <= maxHue)
                        {
                            if (h < minHue || h > maxHue)
                            {
                                byte* atual = origem + y * stride + x * tamanhoPixel;

                                b = atual[0];
                                g = atual[1];
                                r = atual[2];
                                int grayScale = (int)(r * 0.299 + g * 0.587 + b * 0.114);

                                atual[0] = (byte)grayScale;
                                atual[1] = (byte)grayScale;
                                atual[2] = (byte)grayScale;
                            }
                        }
                        else
                        {
                            if (!(h >= minHue || h <= maxHue))
                            {
                                byte* atual = origem + y * stride + x * tamanhoPixel;

                                b = atual[0];
                                g = atual[1];
                                r = atual[2];

                                int grayScale = (int)(0.299 * r + 0.587 * g + 0.114 * b);

                                atual[0] = (byte)grayScale;
                                atual[1] = (byte)grayScale;
                                atual[2] = (byte)grayScale;
                            }
                        }
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
        }
    }
}
