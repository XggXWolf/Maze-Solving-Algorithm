namespace MazeSolver
{
    partial class Form1
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
            convertImage = new Button();
            imgPath = new TextBox();
            pictureBox1 = new PictureBox();
            startCoord = new Label();
            endCoord = new Label();
            solveButton = new Button();
            trackBar1 = new TrackBar();
            scaleLabel = new Label();
            startBox = new TextBox();
            endBox = new TextBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // convertImage
            // 
            convertImage.Location = new Point(12, 107);
            convertImage.Name = "convertImage";
            convertImage.Size = new Size(170, 23);
            convertImage.TabIndex = 0;
            convertImage.Text = "Convert";
            convertImage.UseVisualStyleBackColor = true;
            convertImage.Click += convertImage_Click;
            // 
            // imgPath
            // 
            imgPath.Location = new Point(12, 27);
            imgPath.Name = "imgPath";
            imgPath.Size = new Size(170, 23);
            imgPath.TabIndex = 1;
            imgPath.Text = "..\\..\\..\\maze2.png";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ControlDark;
            pictureBox1.Location = new Point(188, 27);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(576, 364);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            // 
            // startCoord
            // 
            startCoord.AutoSize = true;
            startCoord.Location = new Point(12, 168);
            startCoord.Name = "startCoord";
            startCoord.Size = new Size(34, 15);
            startCoord.TabIndex = 3;
            startCoord.Text = "Start:";
            // 
            // endCoord
            // 
            endCoord.AutoSize = true;
            endCoord.Location = new Point(12, 202);
            endCoord.Name = "endCoord";
            endCoord.Size = new Size(30, 15);
            endCoord.TabIndex = 4;
            endCoord.Text = "End:";
            // 
            // solveButton
            // 
            solveButton.Location = new Point(12, 136);
            solveButton.Name = "solveButton";
            solveButton.Size = new Size(170, 23);
            solveButton.TabIndex = 5;
            solveButton.Text = "Solve";
            solveButton.UseVisualStyleBackColor = true;
            solveButton.Click += solveButton_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(12, 56);
            trackBar1.Minimum = 1;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(170, 45);
            trackBar1.TabIndex = 6;
            trackBar1.Value = 1;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // scaleLabel
            // 
            scaleLabel.AutoSize = true;
            scaleLabel.Location = new Point(12, 86);
            scaleLabel.Name = "scaleLabel";
            scaleLabel.Size = new Size(81, 15);
            scaleLabel.TabIndex = 7;
            scaleLabel.Text = "Scale : 0.0625x";
            // 
            // startBox
            // 
            startBox.Location = new Point(52, 165);
            startBox.Name = "startBox";
            startBox.Size = new Size(28, 23);
            startBox.TabIndex = 8;
            startBox.Text = "0,0";
            // 
            // endBox
            // 
            endBox.Location = new Point(52, 199);
            endBox.Name = "endBox";
            endBox.Size = new Size(28, 23);
            endBox.TabIndex = 9;
            endBox.Text = "0,0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 234);
            label2.MaximumSize = new Size(100, 0);
            label2.Name = "label2";
            label2.Size = new Size(99, 45);
            label2.TabIndex = 10;
            label2.Text = "You can also left - right click to set start and end";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(endBox);
            Controls.Add(startBox);
            Controls.Add(scaleLabel);
            Controls.Add(trackBar1);
            Controls.Add(solveButton);
            Controls.Add(endCoord);
            Controls.Add(startCoord);
            Controls.Add(pictureBox1);
            Controls.Add(imgPath);
            Controls.Add(convertImage);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button convertImage;
        private TextBox imgPath;
        private PictureBox pictureBox1;
        private Label startCoord;
        private Label endCoord;
        private Button solveButton;
        private TrackBar trackBar1;
        private Label scaleLabel;
        private TextBox startBox;
        private TextBox endBox;
        private Label label2;
    }
}
