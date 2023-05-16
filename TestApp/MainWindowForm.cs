using Berbetolto;
using Berbetolto.Properties;

namespace TestApp {

    public enum GeneratorState {
        Salary, TaxId
    }

    public enum ProgressState {
        InProgress, Finished, Default, Error
    }
    public partial class MainWindowForm : Form {
        private const string FILE_NOT_FOUND_TEXT = "\nNem található a következő fájl: ";
        private const string LOADED_FILES_TEXT = "\nBetöltött fájlok:";
        private const string NO_COSTFILE_FOUND_TEXT = "\nNincsenek korábbi használatból elmentett fájlok.";
        private const string FILE_LOADED_TEXT = " fájl betöltve.";
        private const string DEFAULT_TEXT = "Kérem töltse be a használni kívánt excel fájlokat!";
        private const string FILE_ALREADY_LOADED_TEXT = "\nIlyen nevű fájl már betöltésre került. Válasszon másik fájlt!";
        private const string VERSION = "0.953";
        public GeneratorState RadioButtonState { get; set; }
        public UIController UIController { get; set; }
        public HelpWindow HelpWindow { get; set; }

        public MainWindowForm() {
            InitializeComponent();
            UIController = new UIController(this);
            WindowOperations.mainWindowForm = this;
            HelpWindow = null;
            label3.Text += VERSION;
            AddTextToTextBox(DEFAULT_TEXT);
            RadioButtonState = GeneratorState.Salary;
            SetImage(ProgressState.Default);
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
            richTextBox1.ScrollToCaret();
        }

        public void SetImage(ProgressState progressState) {
            switch (progressState) {
                default:
                case ProgressState.InProgress:
                    pictureBox1.BackgroundImage = Resources.task_progress_1;
                    break;
                case ProgressState.Finished:
                    pictureBox1.BackgroundImage = Resources.finished_icon_0;
                    break;
                case ProgressState.Default:
                    pictureBox1.BackgroundImage = null;
                    break;
                case ProgressState.Error:
                    pictureBox1.BackgroundImage = Resources.error_icon;
                    break;
            }
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
                AddTextToTextBox(LOADED_FILES_TEXT);
                ListFileNames(FileInputOutputOperations.CostExcelFiles);
            } else {
                AddTextToTextBox(NO_COSTFILE_FOUND_TEXT);
            }
        }

        private void CheckForExistingPaths(String[] paths, List<String> existingFiles) {
            foreach (String actual in paths) {
                if (File.Exists(actual)) {
                    existingFiles.Add(actual);
                } else {
                    AddTextToTextBox(FILE_NOT_FOUND_TEXT + actual);
                }
            }
            FileInputOutputOperations.CostExcelFiles = existingFiles;
        }

        private void ListFileNames(List<String> filesPaths) {
            foreach (String actual in filesPaths) {
                AddTextToTextBox("\n - " + actual);
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
                    AddTextToTextBox("\n - " + file + FILE_LOADED_TEXT);
                } else {
                    AddTextToTextBox(FILE_ALREADY_LOADED_TEXT);
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
            AddTextToTextBox(DEFAULT_TEXT);
            SetImage(ProgressState.Default);
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

        private void pictureBox1_Click(object sender, EventArgs e) {

        }
    }
}
