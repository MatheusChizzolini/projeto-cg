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


        private static byte ClampByte(double v)
        {
            if (v < 0) return 0;
            if (v > 255) return 255;
            return (byte)Math.Round(v);
        }

        private static byte HtoGray(double H) // H em graus 0..360
        {
            H %= 360.0;
            if (H < 0) H += 360.0;
            return ClampByte(H * 255.0 / 360.0);
        }

        private static byte StoGray(double S)
        {
            // aceita S em 0..1 ou 0..100
            if (S <= 1.0) return ClampByte(S * 255.0);
            if (S <= 100.0) return ClampByte(S * 255.0 / 100.0);
            return ClampByte(S);
        }

        private static byte ItoGray(double I)
        {
            // aceita I em 0..1 ou 0..255
            if (I <= 1.0) return ClampByte(I * 255.0);
            return ClampByte(I);
        }

        internal static void minitureHSI(Bitmap src, Bitmap imgH, Bitmap imgS, Bitmap imgI)
        {

            int width = src.Width;
            int height = src.Height;

            int miniW = imgH.Width;
            int miniH = imgH.Height;

            if (imgS.Width != miniW || imgS.Height != miniH || imgI.Width != miniW || imgI.Height != miniH)
                throw new ArgumentException("imgH, imgS e imgI devem ter o mesmo tamanho.");

            // fator de amostragem: pega 1 pixel a cada k pixels do src (para caber na miniatura)
            int stepX = Math.Max(1, width / miniW);
            int stepY = Math.Max(1, height / miniH);

            BitmapData bitmapDataSrc = src.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            BitmapData bitmapDataH = imgH.LockBits(
                new Rectangle(0, 0, miniW, miniH),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            BitmapData bitmapDataS = imgS.LockBits(
                new Rectangle(0, 0, miniW, miniH),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            BitmapData bitmapDataI = imgI.LockBits(
                new Rectangle(0, 0, miniW, miniH),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            const int pxSize = 3; // BGR

            unsafe
            {
                byte* pSrc = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* pH = (byte*)bitmapDataH.Scan0.ToPointer();
                byte* pS = (byte*)bitmapDataS.Scan0.ToPointer();
                byte* pI = (byte*)bitmapDataI.Scan0.ToPointer();

                for (int y = 0; y < miniH; y++)
                {
                    int srcY = y * stepY;
                    if (srcY >= height) srcY = height - 1;

                    byte* rowH = pH + y * bitmapDataH.Stride;
                    byte* rowS = pS + y * bitmapDataS.Stride;
                    byte* rowI = pI + y * bitmapDataI.Stride;

                    for (int x = 0; x < miniW; x++)
                    {
                        int srcX = x * stepX;
                        if (srcX >= width) srcX = width - 1;

                        int idxSrc = srcX * pxSize + srcY * bitmapDataSrc.Stride;

                        // 24bppRgb em memória = B, G, R
                        byte b = pSrc[idxSrc + 0];
                        byte g = pSrc[idxSrc + 1];
                        byte r = pSrc[idxSrc + 2];

                        var (H, S, I) = Conversions.RgbToHsi(r, g, b);

                        byte gh = HtoGray(H);
                        byte gs = StoGray(S);
                        byte gi = ItoGray(I);

                        int idxDst = x * pxSize;

                        // escreve H (cinza)
                        rowH[idxDst + 0] = gh;
                        rowH[idxDst + 1] = gh;
                        rowH[idxDst + 2] = gh;

                        // escreve S (cinza)
                        rowS[idxDst + 0] = gs;
                        rowS[idxDst + 1] = gs;
                        rowS[idxDst + 2] = gs;

                        // escreve I (cinza)
                        rowI[idxDst + 0] = gi;
                        rowI[idxDst + 1] = gi;
                        rowI[idxDst + 2] = gi;
                    }
                }
            }

            src.UnlockBits(bitmapDataSrc);
            imgH.UnlockBits(bitmapDataH);
            imgS.UnlockBits(bitmapDataS);
            imgI.UnlockBits(bitmapDataI);
        }
    }
}

