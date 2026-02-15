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
        public static void Luminancia(Bitmap bitmapOrigem, Bitmap bitmapDestino)
        {
            int altura = bitmapOrigem.Height;
            int largura = bitmapOrigem.Width;
            int tamanhoPixel = 3;

            BitmapData bitmapDataOrigem = bitmapOrigem.LockBits(
                new Rectangle(0, 0, largura, altura),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            BitmapData bitmapDataDestino = bitmapDestino.LockBits(
                new Rectangle(0, 0, largura, altura),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            int stride = bitmapDataOrigem.Stride;

            unsafe
            {
                byte* origem = (byte*) bitmapDataOrigem.Scan0.ToPointer();
                byte* destino = (byte*) bitmapDataDestino.Scan0.ToPointer();
                int r, g, b;
                for (int y = 0; y < altura; y++)
                {
                    for (int x = 0; x < largura; x++)
                    {
                        byte* atual = origem + y * stride + x * tamanhoPixel;
                        byte* novoPixel = destino + y * stride + x * tamanhoPixel;
                        b = atual[0];
                        g = atual[1];
                        r = atual[2];
                        int grayScale = (int)(r * 0.299 + g * 0.587 + r * 0.114);
                        novoPixel[0] = (byte) grayScale;
                        novoPixel[1] = (byte) grayScale;
                        novoPixel[2] = (byte) grayScale;
                    }
                }
            }

            bitmapOrigem.UnlockBits(bitmapDataOrigem);
            bitmapDestino.UnlockBits(bitmapDataDestino);
        }
    }
}
