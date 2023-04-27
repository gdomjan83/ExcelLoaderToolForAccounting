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
            button4 = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(473, 387);
            button1.Name = "button1";
            button1.Size = new Size(135, 30);
            button1.TabIndex = 0;
            button1.Text = "Végrehajtás";
            button1.UseVisualStyleBackColor = true;
            button1.Click += generateButton_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(16, 387);
            button2.Name = "button2";
            button2.Size = new Size(135, 30);
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
            richTextBox1.Size = new Size(522, 172);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(51, 29);
            label1.Name = "label1";
            label1.Size = new Size(290, 15);
            label1.TabIndex = 3;
            label1.Text = "Melyik hónapot szeretné könyvelni? (például: 2023.03)";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(51, 55);
            textBox1.MaxLength = 7;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(202, 23);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(51, 93);
            label2.Name = "label2";
            label2.Size = new Size(196, 15);
            label2.TabIndex = 5;
            label2.Text = "Könyvelési nap (például: 2023.04.26)";
            label2.Click += label2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(51, 122);
            textBox2.MaxLength = 10;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(202, 23);
            textBox2.TabIndex = 6;
            // 
            // button5
            // 
            button5.Location = new Point(533, 25);
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
            button3.Location = new Point(169, 387);
            button3.Name = "button3";
            button3.Size = new Size(168, 30);
            button3.TabIndex = 8;
            button3.Text = "Legutóbbi fájlok használata";
            button3.UseVisualStyleBackColor = true;
            button3.Click += loadFilesButton_Click;
            // 
            // button4
            // 
            button4.Location = new Point(259, 122);
            button4.Name = "button4";
            button4.Size = new Size(61, 23);
            button4.TabIndex = 9;
            button4.Text = "Mai nap";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(552, 431);
            label3.Name = "label3";
            label3.Size = new Size(44, 15);
            label3.TabIndex = 10;
            label3.Text = "Verzió: ";
            label3.Click += label3_Click;
            // 
            // MainWindowForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 455);
            Controls.Add(label3);
            Controls.Add(button4);
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
        private Button button4;
        private Label label3;
    }
}