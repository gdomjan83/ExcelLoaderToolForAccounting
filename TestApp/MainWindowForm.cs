using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp {
    public partial class MainWindowForm : Form {

        public MainWindowForm() {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            UIController uiController = new UIController();
            uiController.RunApplication();
        }

        private void button2_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) {
                String file = openFileDialog.FileName;
                if (!CheckIfFileIsInList(file, ExcelInputOutputOperations.CostExcelFiles)) {
                    ExcelInputOutputOperations.CostExcelFiles.Add(file);
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
    }
}
