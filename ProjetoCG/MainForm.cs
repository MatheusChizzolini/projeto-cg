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
                // pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void OnClickButtonLuminancia(object sender, EventArgs e)
        {
            if (image != null)
            {
                bitmap = (Bitmap)image;
                Bitmap bitmapDestino = new Bitmap(image);
                Filters.Luminancia(bitmap, bitmapDestino);
                pictureBox.Image = bitmapDestino;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (conversions != null)
            {
                int x = e.X, y = e.Y;
                if (x >= 0 && x < conversions.width && y >= 0 && y < conversions.height)
                {
                    RGB corRgb = conversions.RGBMatrix[x, y];

                    label.Text = $"RGB({corRgb.R}, {corRgb.G}, {corRgb.B})";
                }
            }
        }

    }
}
