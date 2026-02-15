namespace ProjetoCG
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog = new OpenFileDialog();
            pictureBox = new PictureBox();
            buttonLuminancia = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.Gainsboro;
            pictureBox.Location = new Point(383, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(600, 600);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.MouseDoubleClick += OpenFileOnMouseDoubleClick;
            // 
            // buttonLuminancia
            // 
            buttonLuminancia.Location = new Point(12, 12);
            buttonLuminancia.Name = "buttonLuminancia";
            buttonLuminancia.Size = new Size(106, 23);
            buttonLuminancia.TabIndex = 1;
            buttonLuminancia.Text = "Luminância";
            buttonLuminancia.UseVisualStyleBackColor = true;
            buttonLuminancia.Click += OnClickButtonLuminancia;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1350, 729);
            Controls.Add(buttonLuminancia);
            Controls.Add(pictureBox);
            Name = "MainForm";
            Text = "Projeto Computação Gráfica";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog;
        private PictureBox pictureBox;
        private Button buttonLuminancia;
    }
}
