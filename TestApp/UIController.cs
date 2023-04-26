using Berbetolto;
using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public MainWindowForm MainWindowForm { get; set; }

        private const String WARNING_TEXT = "Betöltés befejezve.\nFigyelem, az M oszlop csak tájékoztatásul szerepel a CSV fájlban, SAP betöltés előtt kérem törölni!";
        private const String NO_FILE_TEXT = "\nKérlek add meg, hogy melyik fájlokból töltsem be a költségeket!";
        private const String WRONG_MONTH_TEXT = "\nNem megfelelő a megadott dátum formátum.";
        private const String WRONG_FILENAME_TEXT = "\nNem megfelelő a megadott fájl név formátum.";

        public UIController(MainWindowForm mainWindowForm) {
            MainWindowForm = mainWindowForm;
            SetupFolders();
        }        
        public void RunApplication() {            
            bool finished = RunExcelOperations();
            if (finished) {
                FinishTask();
            }
        }

        private void FinishTask() {
            MessageBox.Show(WARNING_TEXT);
            Process.Start("explorer.exe", TargetFilesFolder);
        }

        private void SetupFolders() {
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            SourceFilesFolder = Path.Combine(currentDirectory, "Resources");
            Directory.CreateDirectory(SourceFilesFolder);
            TargetFilesFolder = Path.Combine(currentDirectory, "Result");
            Directory.CreateDirectory(TargetFilesFolder);
        }

        private bool RunExcelOperations() {
            EmptyFolders();
            String[] files = FolderOperation.CopySeveralFiles(ExcelInputOutputOperations.CostExcelFiles, SourceFilesFolder).ToArray();
            return ProcessFiles(files);
        }

        private bool ProcessFiles(String[] files) {
            String month = MainWindowForm.GetMonth();
            String fileName = MainWindowForm.GetFileName();
            if (ValidateFilesAndTextInput(month, fileName, files)) {
                ExcelFilesProcessor excelFilesProcessor = AddFilesToExcelFileProcessor(files, month);
                WriteCSVDataToFile(excelFilesProcessor, fileName + ".csv");
                return true;
            } else {
                return false;
            }
        }

        private bool ValidateFilesAndTextInput(String month, String fileName, String[] files) {
            bool result = true;
            if (!Validator.CheckIfFilesPresentInDirectory(files)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(NO_FILE_TEXT);
                result = false;
            }
            if (!Validator.CheckIfMonthInCorrectForm(month)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(WRONG_MONTH_TEXT);
                result = false;
            }
            if (!Validator.CheckCSVFileName(fileName)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(WRONG_FILENAME_TEXT);
                result = false;
            }
            return result;
        }

        private void EmptyFolders() {
            FolderOperation.DeleteFiles(SourceFilesFolder);
            FolderOperation.DeleteFiles(TargetFilesFolder);
        }

        private ExcelFilesProcessor AddFilesToExcelFileProcessor(String[] files, string month) {
            ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(month);
            excelFilesProcessor.FilePaths = files;
            return excelFilesProcessor;
        }

        private void WriteCSVDataToFile(ExcelFilesProcessor excelFilesProcessor, String targetFileName) {
            List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToCSVList();
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            excelFilesProcessor.WriteCSVFile(targetFile, csvResult);
        }
    }
}
