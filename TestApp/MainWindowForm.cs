using Berbetolto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TestApp {
    public partial class MainWindowForm : Form {

        public UIController UIController { get; set; }
        public HelpWindow HelpWindow { get; set; }

        public MainWindowForm() {
            InitializeComponent();
            UIController = new UIController(this);
            WindowOperations.mainWindowForm = this;
            HelpWindow = null;
        }

        public String GetMonth() {
            return textBox1.Text;
        }

        public String GetFileName() {
            return textBox2.Text;
        }

        public RichTextBox GetTextBox() {
            return richTextBox1;
        }

        public void AddTextToTextBox(String text) {
            richTextBox1.AppendText(text);
        }

        private void Form_Load(object sender, EventArgs e) {
            this.richTextBox1.Text = "Betöltött költségkövető fájlok:\n";
        }

        private void button1_Click(object sender, EventArgs e) {
            UIController.RunApplication();
        }
        private void button5_Click(object sender, EventArgs e) {
            if (CheckIfOnlyOneWindowIsOpen()) {
                HelpWindow = new HelpWindow();
                HelpWindow.Show();
            }            
        }

        private bool CheckIfOnlyOneWindowIsOpen() {
            int formCount = System.Windows.Forms.Application.OpenForms.Count;
            return formCount == 1;
        }

        private void button2_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) {
                String file = openFileDialog.FileName;
                if (!CheckIfFileIsInList(file, ExcelInputOutputOperations.CostExcelFiles)) {
                    ExcelInputOutputOperations.CostExcelFiles.Add(file);
                    richTextBox1.AppendText("\n" + file);
                }
            }
        }

        private bool CheckIfFileIsInList(String fileName, List<String> fileList) {
            if (fileList.Count == 0 || !ListContainsString(fileName, fileList)) {
                return false;
            }
            return true;
        }

        private bool ListContainsString(String fileName, List<String> fileList) {
            bool alreadyInList = false;
            foreach (String actual in fileList) {
                if (fileName.Equals(actual)) {
                    alreadyInList = true;
                }
            }
            return alreadyInList;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) {

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }
    }
}
