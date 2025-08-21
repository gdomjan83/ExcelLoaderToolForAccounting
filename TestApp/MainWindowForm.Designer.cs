namespace TestApp {
    partial class MainWindowForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            button1 = new Button();
            button2 = new Button();
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            button5 = new Button();
            button3 = new Button();
            label3 = new Label();
            button6 = new Button();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(596, 554);
            button1.Name = "button1";
            button1.Size = new Size(167, 30);
            button1.TabIndex = 0;
            button1.Text = "Létrehozás";
            button1.UseVisualStyleBackColor = true;
            button1.Click += generateButton_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(596, 191);
            button2.Name = "button2";
            button2.Size = new Size(168, 30);
            button2.TabIndex = 1;
            button2.Text = "Tallózás";
            button2.UseVisualStyleBackColor = true;
            button2.Click += browseFileButton_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(51, 180);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(522, 427);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(51, 93);
            label1.Name = "label1";
            label1.Size = new Size(290, 15);
            label1.TabIndex = 3;
            label1.Text = "Melyik hónapot szeretné könyvelni? (például: 2023.03)";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(51, 122);
            textBox1.MaxLength = 7;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(229, 23);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(369, 93);
            label2.Name = "label2";
            label2.Size = new Size(369, 15);
            label2.TabIndex = 5;
            label2.Text = "Könyvelési nap (például: 2025.07.31 - általában a hónap utolsó napja)";
            label2.Click += label2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(369, 122);
            textBox2.MaxLength = 10;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(202, 23);
            textBox2.TabIndex = 6;
            // 
            // button5
            // 
            button5.Location = new Point(639, 50);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 7;
            button5.Text = "Súgó";
            button5.UseVisualStyleBackColor = true;
            button5.Click += helpButton_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(596, 250);
            button3.Name = "button3";
            button3.Size = new Size(168, 30);
            button3.TabIndex = 8;
            button3.Text = "Legutóbbi fájlok használata";
            button3.UseVisualStyleBackColor = true;
            button3.Click += loadFilesButton_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(639, 26);
            label3.Name = "label3";
            label3.Size = new Size(44, 15);
            label3.TabIndex = 10;
            label3.Text = "Verzió: ";
            label3.Click += label3_Click;
            // 
            // button6
            // 
            button6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button6.Location = new Point(597, 496);
            button6.Name = "button6";
            button6.Size = new Size(166, 30);
            button6.TabIndex = 11;
            button6.Text = "Visszaállítás";
            button6.UseVisualStyleBackColor = true;
            button6.Click += resetButton_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(51, 50);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(119, 19);
            radioButton1.TabIndex = 12;
            radioButton1.TabStop = true;
            radioButton1.Text = "FEJ-TET generálás";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += salaryGeneratorButton_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(204, 50);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(163, 19);
            radioButton2.TabIndex = 13;
            radioButton2.Text = "Adóazonosítók generálása";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += taxGeneratorButton_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(51, 26);
            label4.Name = "label4";
            label4.Size = new Size(255, 15);
            label4.TabIndex = 14;
            label4.Text = "Milyen típusú generálást szeretne végrehajtani?";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(630, 337);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 100);
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // MainWindowForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 643);
            Controls.Add(pictureBox1);
            Controls.Add(label4);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(button6);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(button5);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "MainWindowForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bérbetöltő";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private RichTextBox richTextBox1;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Button button5;
        private Button button3;
        private Label label3;
        private Button button6;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label4;
        private PictureBox pictureBox1;
    }
}