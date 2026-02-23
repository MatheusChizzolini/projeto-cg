namespace ProjetoCG
{
    public partial class MainForm : Form
    {
        private Image? image;
        private Bitmap? bitmap;
        private Bitmap? originalBitmap;
        private Bitmap? aux;
        private Conversions? conversions;

        private int stepIntensity = 0;
        private Boolean flagUsingIntensity = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenFileOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de imagens (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                bitmap = (Bitmap)image;
                originalBitmap = new Bitmap(bitmap);
                conversions = new Conversions(bitmap);
                pictureBox.Image = image;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                flagUsingIntensity = false;
                stepIntensity = 0;

                pictureBoxH.Image = null;
                pictureBoxS.Image = null;
                pictureBoxI.Image = null;


                conversions.UpdateMatrices(bitmap);
                
            }
        }

        private void SetCurrentBitmap(Bitmap bmp)
        {

            if (!ReferenceEquals(bitmap, bmp))
            {
                bitmap?.Dispose();
                bitmap = (Bitmap)bmp.Clone();
                pictureBox.Image = bitmap;
            }
            else
            {
                pictureBox.Image = bitmap;
            }
        }

        private void ButtonLuminanciaClick(object sender, EventArgs e)
        {

            if (bitmap != null)
            {
                if (flagUsingIntensity)
                {
                    aux?.Dispose();
                    aux = null;
                    flagUsingIntensity = false;
                    stepIntensity = 0;
                }
                Filters.Luminancia(bitmap);
                SetCurrentBitmap(bitmap);
            }
        }

        private void GetColorCodesOnMouseMove(object sender, MouseEventArgs e)
        {
            if (conversions != null)
            {
                if (TryGetImagePixel(e.Location, out Point imgPt))
                {

                    int x = imgPt.X;
                    int y = imgPt.Y;

                    //Rgb corRgb = conversions.RgbMatrix[x, y];
                    //Cmy corCmy = conversions.CmyMatrix[x, y];
                    //Hsi corHsi = conversions.HsiMatrix[x, y];

                    //textBoxR.Text = $"{corRgb.R}";
                    //textBoxG.Text = $"{corRgb.G}";
                    //textBoxB.Text = $"{corRgb.B}";

                    //textBoxC.Text = $"{corCmy.C:F3}";
                    //textBoxM.Text = $"{corCmy.M:F3}";
                    //textBoxY.Text = $"{corCmy.Y:F3}";

                    //textBoxH.Text = $"{corHsi.H}º";
                    //textBoxS.Text = $"{corHsi.S:F3}";
                    //textBoxI.Text = $"{corHsi.I:F3}";

                    Color pixel = bitmap.GetPixel(x, y);
                    textBoxR.Text = $"{pixel.R}";
                    textBoxG.Text = $"{pixel.G}";
                    textBoxB.Text = $"{pixel.B}";

                    double C, M, Y;
                    (C, M, Y) = Conversions.CmyPixel(pixel);

                    textBoxC.Text = $"{C:F3}";
                    textBoxM.Text = $"{M:F3}";
                    textBoxY.Text = $"{Y:F3}";

                    double H, S, I;
                    (H, S, I) = Conversions.RgbToHsi(pixel.R, pixel.G, pixel.B);

                    textBoxH.Text = $"{(int)H}º";
                    textBoxS.Text = $"{(int)S}%";
                    textBoxI.Text = $"{(int)I}";

                    panelColor.BackColor = pixel;
                    tbX.Text = $"{x}";
                    tbY.Text = $"{y}";

                }
            }
        }

        private bool TryGetImagePixel(Point mousePoint, out Point imagePoint)
        {
            imagePoint = Point.Empty;

            if (pictureBox.Image == null)
                return false;

            int imgW = pictureBox.Image.Width;
            int imgH = pictureBox.Image.Height;

            int boxW = pictureBox.ClientSize.Width;
            int boxH = pictureBox.ClientSize.Height;

            float imgAspect = (float)imgW / imgH;
            float boxAspect = (float)boxW / boxH;

            Rectangle drawRect;

            if (imgAspect > boxAspect)
            {
                int drawHeight = (int)(boxW / imgAspect);
                int top = (boxH - drawHeight) / 2;
                drawRect = new Rectangle(0, top, boxW, drawHeight);
            }
            else
            {
                int drawWidth = (int)(boxH * imgAspect);
                int left = (boxW - drawWidth) / 2;
                drawRect = new Rectangle(left, 0, drawWidth, boxH);
            }

            if (!drawRect.Contains(mousePoint))
                return false;

            float scaleX = (float)imgW / drawRect.Width;
            float scaleY = (float)imgH / drawRect.Height;

            int imgX = (int)((mousePoint.X - drawRect.X) * scaleX);
            int imgY = (int)((mousePoint.Y - drawRect.Y) * scaleY);

            imgX = Math.Clamp(imgX, 0, imgW - 1);
            imgY = Math.Clamp(imgY, 0, imgH - 1);

            imagePoint = new Point(imgX, imgY);
            return true;
        }

        private void DecreaseHueClick(object sender, EventArgs e)
        {
            if (bitmap == null) return;

            if (flagUsingIntensity)
            {
                aux.Dispose();
                aux = null;
                flagUsingIntensity = false;
                stepIntensity = 0;
            }

            Conversions.ShiftHueInPlace(bitmap, -10);
            SetCurrentBitmap(bitmap);
        }

        private void IncreaseHueClick(object sender, EventArgs e)
        {
            if (bitmap == null) return;
            if (flagUsingIntensity)
            {
                aux.Dispose();
                aux = null;
                flagUsingIntensity = false;
                stepIntensity = 0;
            }


            Conversions.ShiftHueInPlace(bitmap, +10);
            SetCurrentBitmap(bitmap);
        }


        //private void DecreaseHueClick(object sender, EventArgs e)
        //{
        //    if (image != null && conversions != null)
        //    {
        //        bitmap = (Bitmap)image;
        //        for (int y = 0; y < conversions.height; y++)
        //        {
        //            for (int x = 0; x < conversions.width; x++)
        //            {
        //                if (conversions.HsiMatrix[x, y].H >= 0)
        //                {
        //                    int newHue = conversions.HsiMatrix[x, y].H - 10;
        //                    if (newHue < 0)
        //                    {
        //                        newHue += 360;
        //                    }

        //                    conversions.HsiMatrix[x, y].H = newHue;
        //                }
        //            }
        //        }

        //        Rgb[,] newRgbMatrix = conversions.ConvertHsiMatrixToRgb();
        //        Filters.ChangeHue(bitmap, newRgbMatrix);
        //        pictureBox.Image = bitmap;
        //        //conversions.UpdateMatrices(bitmap);
        //    }
        //}

        //private void IncreaseHueClick(object sender, EventArgs e)
        //{
        //    if (image != null && conversions != null)
        //    {
        //        bitmap = (Bitmap)image;
        //        for (int y = 0; y < conversions.height; y++)
        //        {
        //            for (int x = 0; x < conversions.width; x++)
        //            {
        //                if (conversions.HsiMatrix[x, y].H >= 0)
        //                {
        //                    int newHue = conversions.HsiMatrix[x, y].H + 10; ;
        //                    if (newHue >= 360)
        //                    {
        //                        newHue -= 360;
        //                    }

        //                    conversions.HsiMatrix[x, y].H = newHue;
        //                }
        //            }
        //        }

        //        Rgb[,] newRgbMatrix = conversions.ConvertHsiMatrixToRgb();
        //        Filters.ChangeHue(bitmap, newRgbMatrix);
        //        pictureBox.Image = bitmap;
        //        //conversions.UpdateMatrices(bitmap);
        //    }
        //}

        private void ApplyHueSegmentation()
        {
            if (originalBitmap != null && conversions != null)
            {
                bitmap = new Bitmap(originalBitmap);

                Filters.SegmentHue(bitmap, conversions.HsiMatrix,
                    (int)numericUpDownMinHue.Value,
                    (int)numericUpDownMaxHue.Value);

                pictureBox.Image = bitmap;
            }
        }


        private void SegmentHueOnCheckedChanged(object sender, EventArgs e)
        {
            if (originalBitmap != null)
            {
                if (checkBoxSegmentHue.Checked)
                {
                    conversions.UpdateMatrices(bitmap);
                    ApplyHueSegmentation();
                }
                else
                {
                    SetCurrentBitmap(originalBitmap);
                    pictureBox.Image = originalBitmap;
                }
            }
        }

        private void MinHueOnValueChanged(object sender, EventArgs e)
        {
            if (checkBoxSegmentHue.Checked)
            {
                ApplyHueSegmentation();
            }
        }

        private void MaxHueOnValueChanged(object sender, EventArgs e)
        {
            if (checkBoxSegmentHue.Checked)
            {
                ApplyHueSegmentation();
            }
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void buttonIncreseIntesityClick(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                if (!flagUsingIntensity)
                {
                    flagUsingIntensity = true;

                    aux?.Dispose();
                    aux = (Bitmap)bitmap.Clone();
                }
                stepIntensity += 10;
                var newBmp = (Bitmap)aux.Clone();
                Conversions.ChangeIntensity(newBmp, stepIntensity);

                SetCurrentBitmap(newBmp);
            }
        }

        private void buttonDecreaseIntensityClick(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                if (!flagUsingIntensity)
                {
                    flagUsingIntensity = true;

                    aux?.Dispose();
                    aux = (Bitmap)bitmap.Clone();
                }
                stepIntensity -= 10;
                var newBmp = (Bitmap)aux.Clone();
                Conversions.ChangeIntensity(newBmp, stepIntensity);

                SetCurrentBitmap(newBmp);
            }
        }


        private void GenerateMiniatures_Click(object sender, EventArgs e)
        {
            if (bitmap == null) return;


            Bitmap src = bitmap;

            Bitmap imgH = new Bitmap(src.Width, src.Height);
            Bitmap imgS = new Bitmap(src.Width, src.Height);
            Bitmap imgI = new Bitmap(src.Width, src.Height);



            Filters.minitureHSI(src, imgH, imgS, imgI);

            // Mostre nas miniaturas 
            pictureBoxH.Image = imgH;
            pictureBoxS.Image = imgS;
            pictureBoxI.Image = imgI;

            pictureBoxH.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxS.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxI.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void tbY_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
