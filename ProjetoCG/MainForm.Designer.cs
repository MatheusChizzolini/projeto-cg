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
            textBoxR = new TextBox();
            textBoxG = new TextBox();
            textBoxB = new TextBox();
            labelR = new Label();
            labelG = new Label();
            labelB = new Label();
            labelC = new Label();
            labelM = new Label();
            labelY = new Label();
            textBoxC = new TextBox();
            textBoxM = new TextBox();
            textBoxY = new TextBox();
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
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(340, 21);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(640, 640);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.MouseDoubleClick += OpenFileOnMouseDoubleClick;
            pictureBox.MouseMove += pictureBox_MouseMove;
            // 
            // buttonLuminancia
            // 
            buttonLuminancia.Location = new Point(15, 150);
            buttonLuminancia.Name = "buttonLuminancia";
            buttonLuminancia.Size = new Size(106, 23);
            buttonLuminancia.TabIndex = 1;
            buttonLuminancia.Text = "Luminância";
            buttonLuminancia.UseVisualStyleBackColor = true;
            buttonLuminancia.Click += OnClickButtonLuminancia;
            // 
            // textBoxR
            // 
            textBoxR.BackColor = Color.Gainsboro;
            textBoxR.BorderStyle = BorderStyle.FixedSingle;
            textBoxR.Font = new Font("Segoe UI", 10F);
            textBoxR.Location = new Point(38, 21);
            textBoxR.Name = "textBoxR";
            textBoxR.ReadOnly = true;
            textBoxR.Size = new Size(60, 25);
            textBoxR.TabIndex = 2;
            textBoxR.TextAlign = HorizontalAlignment.Center;
            // 
            // textBoxG
            // 
            textBoxG.BackColor = Color.Gainsboro;
            textBoxG.BorderStyle = BorderStyle.FixedSingle;
            textBoxG.Font = new Font("Segoe UI", 10F);
            textBoxG.Location = new Point(141, 21);
            textBoxG.Name = "textBoxG";
            textBoxG.ReadOnly = true;
            textBoxG.Size = new Size(60, 25);
            textBoxG.TabIndex = 3;
            textBoxG.TextAlign = HorizontalAlignment.Center;
            // 
            // textBoxB
            // 
            textBoxB.BackColor = Color.Gainsboro;
            textBoxB.BorderStyle = BorderStyle.FixedSingle;
            textBoxB.Font = new Font("Segoe UI", 10F);
            textBoxB.Location = new Point(239, 21);
            textBoxB.Name = "textBoxB";
            textBoxB.ReadOnly = true;
            textBoxB.Size = new Size(60, 25);
            textBoxB.TabIndex = 4;
            textBoxB.TextAlign = HorizontalAlignment.Center;
            // 
            // labelR
            // 
            labelR.AutoSize = true;
            labelR.Font = new Font("Segoe UI", 10F);
            labelR.Location = new Point(15, 23);
            labelR.Name = "labelR";
            labelR.Size = new Size(17, 19);
            labelR.TabIndex = 5;
            labelR.Text = "R";
            // 
            // labelG
            // 
            labelG.AutoSize = true;
            labelG.Font = new Font("Segoe UI", 10F);
            labelG.Location = new Point(116, 23);
            labelG.Name = "labelG";
            labelG.Size = new Size(19, 19);
            labelG.TabIndex = 6;
            labelG.Text = "G";
            // 
            // labelB
            // 
            labelB.AutoSize = true;
            labelB.Font = new Font("Segoe UI", 10F);
            labelB.Location = new Point(216, 23);
            labelB.Name = "labelB";
            labelB.Size = new Size(17, 19);
            labelB.TabIndex = 7;
            labelB.Text = "B";
            // 
            // labelC
            // 
            labelC.AutoSize = true;
            labelC.Font = new Font("Segoe UI", 10F);
            labelC.Location = new Point(14, 63);
            labelC.Name = "labelC";
            labelC.Size = new Size(18, 19);
            labelC.TabIndex = 8;
            labelC.Text = "C";
            // 
            // labelM
            // 
            labelM.AutoSize = true;
            labelM.Font = new Font("Segoe UI", 10F);
            labelM.Location = new Point(113, 63);
            labelM.Name = "labelM";
            labelM.Size = new Size(22, 19);
            labelM.TabIndex = 9;
            labelM.Text = "M";
            // 
            // labelY
            // 
            labelY.AutoSize = true;
            labelY.Font = new Font("Segoe UI", 10F);
            labelY.Location = new Point(216, 63);
            labelY.Name = "labelY";
            labelY.Size = new Size(17, 19);
            labelY.TabIndex = 10;
            labelY.Text = "Y";
            // 
            // textBoxC
            // 
            textBoxC.BackColor = Color.Gainsboro;
            textBoxC.BorderStyle = BorderStyle.FixedSingle;
            textBoxC.Font = new Font("Segoe UI", 10F);
            textBoxC.Location = new Point(38, 61);
            textBoxC.Name = "textBoxC";
            textBoxC.ReadOnly = true;
            textBoxC.Size = new Size(60, 25);
            textBoxC.TabIndex = 11;
            textBoxC.TextAlign = HorizontalAlignment.Center;
            // 
            // textBoxM
            // 
            textBoxM.BackColor = Color.Gainsboro;
            textBoxM.BorderStyle = BorderStyle.FixedSingle;
            textBoxM.Font = new Font("Segoe UI", 10F);
            textBoxM.Location = new Point(141, 61);
            textBoxM.Name = "textBoxM";
            textBoxM.ReadOnly = true;
            textBoxM.Size = new Size(60, 25);
            textBoxM.TabIndex = 12;
            textBoxM.TextAlign = HorizontalAlignment.Center;
            // 
            // textBoxY
            // 
            textBoxY.BackColor = Color.Gainsboro;
            textBoxY.BorderStyle = BorderStyle.FixedSingle;
            textBoxY.Font = new Font("Segoe UI", 10F);
            textBoxY.Location = new Point(239, 61);
            textBoxY.Name = "textBoxY";
            textBoxY.ReadOnly = true;
            textBoxY.Size = new Size(60, 25);
            textBoxY.TabIndex = 13;
            textBoxY.TextAlign = HorizontalAlignment.Center;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(textBoxY);
            Controls.Add(textBoxM);
            Controls.Add(textBoxC);
            Controls.Add(labelY);
            Controls.Add(labelM);
            Controls.Add(labelC);
            Controls.Add(labelB);
            Controls.Add(labelG);
            Controls.Add(labelR);
            Controls.Add(textBoxB);
            Controls.Add(textBoxG);
            Controls.Add(textBoxR);
            Controls.Add(buttonLuminancia);
            Controls.Add(pictureBox);
            Name = "MainForm";
            Text = "Projeto Computação Gráfica";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenFileDialog openFileDialog;
        private PictureBox pictureBox;
        private Button buttonLuminancia;
        private TextBox textBoxR;
        private TextBox textBoxG;
        private TextBox textBoxB;
        private Label labelR;
        private Label labelG;
        private Label labelB;
        private TextBox textBoxC;
        private TextBox textBoxM;
        private TextBox textBoxY;
        private Label labelC;
        private Label labelM;
        private Label labelY;
    }
}
