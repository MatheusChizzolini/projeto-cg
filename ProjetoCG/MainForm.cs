namespace ProjetoCG
{
    public partial class MainForm : Form
    {
        private Image image;
        private Bitmap bitmap;
        private Conversions conversions;

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
                conversions = new Conversions(bitmap);
                pictureBox.Image = image;
                //pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void ButtonLuminanciaClick(object sender, EventArgs e)
        {
            if (image != null)
            {
                bitmap = (Bitmap)image;
                Bitmap bitmapDestino = new Bitmap(image);
                Filters.Luminancia(bitmap, bitmapDestino);
                pictureBox.Image = bitmapDestino;
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
    }
}
