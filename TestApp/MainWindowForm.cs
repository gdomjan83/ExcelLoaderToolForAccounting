using Berbetolto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestApp {

    public enum GeneratorState {
        Salary, TaxId
    }
    public partial class MainWindowForm : Form {
        private const string FILE_NOT_FOUND_TEXT = "\nNem található a következő fájl: ";
        private const string LOADED_FILES_TEXT = "\nBetöltött fájlok:";
        private const string NO_COSTFILE_FOUND_TEXT = "\nNincsenek korábbi használatból elmentett fájlok.";
        private const string FILE_LOADED_TEXT = " fájl betöltve.";
        private const string VERSION = "0.9";
        public GeneratorState RadioButtonState { get; set; }
        public UIController UIController { get; set; }
        public HelpWindow HelpWindow { get; set; }

        public MainWindowForm() {
            InitializeComponent();
            UIController = new UIController(this);
            WindowOperations.mainWindowForm = this;
            HelpWindow = null;
            label3.Text += VERSION;
            AddTextToTextBox("Kérem töltse be a használni kívánt excel fájlokat!");
            RadioButtonState = GeneratorState.Salary;
        }

        public String GetMonth() {
            return textBox1.Text;
        }

        public String GetAccountingDate() {
            return textBox2.Text;
        }

        public RichTextBox GetTextBox() {
            return richTextBox1;
        }

        public void AddTextToTextBox(String text) {
            richTextBox1.AppendText(text);
        }

        private void Form_Load(object sender, EventArgs e) {

        }

        private void generateButton_Click(object sender, EventArgs e) {
            UIController.RunApplication();
        }

        private void loadFilesButton_Click(object sender, EventArgs e) {
            FileInputOutputOperations.CostExcelFiles.Clear();
            String[] result = UIController.LoadLastSave();
            List<String> existingFiles = new List<String>();
            if (result.Length > 0) {
                CheckForExistingPaths(result, existingFiles);
                richTextBox1.AppendText(LOADED_FILES_TEXT);
                ListFileNames(FileInputOutputOperations.CostExcelFiles);
            } else {
                richTextBox1.AppendText(NO_COSTFILE_FOUND_TEXT);
            }
        }

        private void CheckForExistingPaths(String[] paths, List<String> existingFiles) {
            foreach (String actual in paths) {
                if (File.Exists(actual)) {
                    existingFiles.Add(actual);
                } else {
                    richTextBox1.AppendText(FILE_NOT_FOUND_TEXT + actual);
                }
            }
            FileInputOutputOperations.CostExcelFiles = existingFiles;
        }

        private void ListFileNames(List<String> filesPaths) {
            foreach (String actual in filesPaths) {
                richTextBox1.AppendText("\n" + actual);
            }
        }

        private void helpButton_Click(object sender, EventArgs e) {
            if (CheckIfOnlyOneWindowIsOpen()) {
                HelpWindow = new HelpWindow();
                HelpWindow.Show();
            }
        }

        private bool CheckIfOnlyOneWindowIsOpen() {
            int formCount = Application.OpenForms.Count;
            return formCount == 1;
        }

        private void browseFileButton_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) {
                String file = openFileDialog.FileName;
                if (!CheckIfFileIsInList(file, FileInputOutputOperations.CostExcelFiles)) {
                    FileInputOutputOperations.CostExcelFiles.Add(file);
                    richTextBox1.AppendText("\n" + file + FILE_LOADED_TEXT);
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
        private void todayButton_Click(object sender, EventArgs e) {
            String today = DateTime.Now.ToString();
            String trimmed = today.Substring(0, 5) + today.Substring(6, 3) + today.Substring(10, 2);
            textBox2.Text = trimmed;
        }
        private void resetButton_Click(object sender, EventArgs e) {
            NoteCounterData.ResetProperties();
            FileInputOutputOperations.CostExcelFiles.Clear();
            UIController.TotalAccountingPerProjects.Clear();
            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
            richTextBox1.AppendText(LOADED_FILES_TEXT);
        }
        private void salaryGeneratorButton_CheckedChanged(object sender, EventArgs e) {
            RadioButtonState = GeneratorState.Salary;
            textBox2.Clear();
            textBox2.ReadOnly = false;
            button4.Enabled = true;
        }
        private void taxGeneratorButton_CheckedChanged(object sender, EventArgs e) {
            RadioButtonState = GeneratorState.TaxId;
            textBox2.Clear();
            textBox2.ReadOnly = true;
            button4.Enabled = false;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e) {

        }
        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

    }
}
