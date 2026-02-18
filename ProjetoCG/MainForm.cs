namespace ProjetoCG
{
    public partial class MainForm : Form
    {
        private Image? image;
        private Bitmap? bitmap;
        private Bitmap? originalBitmap;
        private Conversions? conversions;

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
                conversions.UpdateMatrices(bitmap);
            }
        }

        private void ButtonLuminanciaClick(object sender, EventArgs e)
        {
            if (image != null)
            {
                bitmap = (Bitmap)image;
                Filters.Luminancia(bitmap);
                pictureBox.Image = bitmap;
            }
        }

        private void GetColorCodesOnMouseMove(object sender, MouseEventArgs e)
        {
            if (conversions != null)
            {
                int x = e.X, y = e.Y;
                if (x >= 0 && x < conversions.width && y >= 0 && y < conversions.height)
                {
                    Rgb corRgb = conversions.RgbMatrix[x, y];
                    Cmy corCmy = conversions.CmyMatrix[x, y];
                    Hsi corHsi = conversions.HsiMatrix[x, y];

                    textBoxR.Text = $"{corRgb.R}";
                    textBoxG.Text = $"{corRgb.G}";
                    textBoxB.Text = $"{corRgb.B}";

                    textBoxC.Text = $"{corCmy.C:F3}";
                    textBoxM.Text = $"{corCmy.M:F3}";
                    textBoxY.Text = $"{corCmy.Y:F3}";

                    textBoxH.Text = $"{corHsi.H}º";
                    textBoxS.Text = $"{corHsi.S:F3}";
                    textBoxI.Text = $"{corHsi.I:F3}";
                }
            }
        }

        private void DecreaseHueClick(object sender, EventArgs e)
        {
            if (image != null && conversions != null)
            {
                bitmap = (Bitmap)image;
                for (int y = 0; y < conversions.height; y++)
                {
                    for (int x = 0; x < conversions.width; x++)
                    {
                        if (conversions.HsiMatrix[x, y].H >= 0)
                        {
                            int newHue = conversions.HsiMatrix[x, y].H - 10;
                            if (newHue < 0)
                            {
                                newHue += 360;
                            }

                            conversions.HsiMatrix[x, y].H = newHue;
                        }
                    }
                }

                Rgb[,] newRgbMatrix = conversions.ConvertHsiMatrixToRgb();
                Filters.ChangeHue(bitmap, newRgbMatrix);
                pictureBox.Image = bitmap;
                conversions.UpdateMatrices(bitmap);
            }
        }

        private void IncreaseHueClick(object sender, EventArgs e)
        {
            if (image != null && conversions != null)
            {
                bitmap = (Bitmap)image;
                for (int y = 0; y < conversions.height; y++)
                {
                    for (int x = 0; x < conversions.width; x++)
                    {
                        if (conversions.HsiMatrix[x, y].H >= 0)
                        {
                            int newHue = conversions.HsiMatrix[x, y].H + 10; ;
                            if (newHue >= 360)
                            {
                                newHue -= 360;
                            }

                            conversions.HsiMatrix[x, y].H = newHue;
                        }
                    }
                }

                Rgb[,] newRgbMatrix = conversions.ConvertHsiMatrixToRgb();
                Filters.ChangeHue(bitmap, newRgbMatrix);
                pictureBox.Image = bitmap;
                conversions.UpdateMatrices(bitmap);
            }
        }

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
                    ApplyHueSegmentation();
                }
                else
                {
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
    }
}
