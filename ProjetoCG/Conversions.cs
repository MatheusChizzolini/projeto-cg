using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
                byte* origem = (byte*)bitmapData.Scan0.ToPointer();
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
                byte* origem = (byte*)bitmapData.Scan0.ToPointer();
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

        public static (double, double, double) CmyPixel(Color pixel)
        {
            return (1 - (pixel.B / 255.0), 1 - (pixel.G / 255.0), 1 - (pixel.R / 255.0));
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

        public static (double H, double S, double I) RgbToHsi(byte R, byte G, byte B)
        {
            int rgbSum = R + G + B;
            if (rgbSum == 0)
                return (0, 0, 0);

            // normaliza r, g, b
            double b = (double)B / rgbSum;
            double g = (double)G / rgbSum;
            double r = (double)R / rgbSum;

            //normaliza intensidade
            double I = rgbSum / (3 * 255.0);
            I = I * 255;

            //saturação
            double S = 1 - 3 * Math.Min(r, Math.Min(g, b));
            S = Math.Clamp(S, 0, 1);
            S *= 100;

            //Hue
            double numerator = 0.5 * ((r - g) + (r - b));
            double denominator = Math.Pow(Math.Pow(r - g, 2) + (r - b) * (g - b), 0.5);

            double Hdeg;
            if (denominator == 0)
            {
                Hdeg = 0;
            }
            else
            {
                // segurança numérica: limitar para[-1, 1]
                double cosTheta = numerator / denominator;
                cosTheta = Math.Clamp(cosTheta, -1.0, 1.0);

                double h = Math.Acos(cosTheta); // em radianos [0..pi]

                if (b > g)
                    h = 2.0 * Math.PI - h; // [0..2pi)

                Hdeg = h * 180.0 / Math.PI; // graus
            }

            return (Hdeg, S, I);
        }

        public static (byte R, byte G, byte B) HsiToRgb(double H, double S, double I)
        {
            double h = H * Math.PI / 180.0;
            double s = S / 100.0;
            double i = I / 255.0;

            double r, g, b;
            double x, y, z;

            if (h < 2 * Math.PI / 3)
            {
                x = i * (1 - s);
                y = i * (1 + (s * Math.Cos(h)) /
                             Math.Cos(Math.PI / 3 - h));
                z = 3 * i - (x + y);

                b = x;
                r = y;
                g = z;
            }
            else if (h < 4 * Math.PI / 3)
            {
                h -= 2 * Math.PI / 3;

                x = i * (1 - s);
                y = i * (1 + (s * Math.Cos(h)) /
                             Math.Cos(Math.PI / 3 - h));
                z = 3 * i - (x + y);

                r = x;
                g = y;
                b = z;
            }
            else
            {
                h -= 4 * Math.PI / 3;

                x = i * (1 - s);
                y = i * (1 + (s * Math.Cos(h)) /
                             Math.Cos(Math.PI / 3 - h));
                z = 3 * i - (x + y);

                r = z;
                g = x;
                b = y;
            }

            byte R = (byte)Math.Clamp((int)(r * 255.0), 0, 255);
            byte G = (byte)Math.Clamp((int)(g * 255.0), 0, 255);
            byte B = (byte)Math.Clamp((int)(b * 255.0), 0, 255);

            return (R, G, B);
        }


        public static void ShiftHueInPlace(Bitmap bitmap, int hueValue)
        {

            int width = bitmap.Width;
            int height = bitmap.Height;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,              
                PixelFormat.Format24bppRgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = stride * height;
                byte[] buffer = new byte[bytes];

                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < height; y++)
                {
                    int row = y * stride;

                    for (int x = 0; x < width; x++)
                    {
                        int idx = row + x * 3; // 3 bytes por pixel

                        // buffer em BGR
                        byte B = buffer[idx + 0];
                        byte G = buffer[idx + 1];
                        byte R = buffer[idx + 2];

                        // RGB -> HSI 
                        var (H, S, I) = RgbToHsi(R, G, B);

                        // muda hue
                        H = (H + hueValue) % 360.0;
                        if (H < 0) H += 360.0;



                        // HSI -> RGB 
                        var (R2, G2, B2) = HsiToRgb(H, S, I);

                        buffer[idx + 0] = B2;
                        buffer[idx + 1] = G2;
                        buffer[idx + 2] = R2;
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static void ChangeIntensity(Bitmap bitmap, double delta)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            double step = (double)delta/100.0;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = stride * height;
                byte[] buffer = new byte[bytes];

                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < height; y++)
                {
                    int row = y * stride;

                    for (int x = 0; x < width; x++)
                    {
                        int idx = row + x * 3; // 3 bytes por pixel

                        // buffer em BGR
                        byte B = buffer[idx + 0];
                        byte G = buffer[idx + 1];
                        byte R = buffer[idx + 2];

                        // RGB -> HSI 
                        var (H, S, I) = RgbToHsi(R, G, B);

                        // muda a intensidade
                        double i = I / 255.0; // normaliza para [0, 1]
                        i += step;
                        I = i * 255.0; // volta para [0, 255]


                        // HSI -> RGB 
                        var (R2, G2, B2) = HsiToRgb(H, S, I);

                        buffer[idx + 0] = B2;
                        buffer[idx + 1] = G2;
                        buffer[idx + 2] = R2;
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
