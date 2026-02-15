namespace ProjetoCG
{
    public partial class MainForm : Form
    {
        private Image image;
        private Bitmap bitmap;

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
                pictureBox.Image = image;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void OnClickButtonLuminancia(object sender, EventArgs e)
        {
            bitmap = (Bitmap) image;
            Bitmap bitmapDestino = new Bitmap(image);
            Filters.Luminancia(bitmap, bitmapDestino);
            pictureBox.Image = bitmapDestino;
        }
    }
}
