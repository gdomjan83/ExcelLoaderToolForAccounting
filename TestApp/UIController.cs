using Berbetolto;
using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public MainWindowForm MainWindowForm { get; set; }

        private const String WARNING_TEXT = "Betöltés befejezve.\nFigyelem, az M oszlop csak tájékoztatásul szerepel a TET CSV fájlban, SAP betöltés előtt kérem törölni!";
        private const String NO_FILE_TEXT = "\nKérem adja meg, hogy melyik fájlokból töltsem be a költségeket!";
        private const String WRONG_MONTH_TEXT = "\nNem megfelelő a megadott dátum formátum.";
        private const String WRONG_FILENAME_TEXT = "\nNem megfelelő a könyvelési dátum formátuma.";
        private const String TET_FILE_NAME = "TET.CSV";
        private const String FEJ_FILE_NAME = "FEJ.CSV";

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
            String date = MainWindowForm.GetAccountingDate();
            if (ValidateFilesAndTextInput(month, date, files)) {
                String correctDate = CreateDateFromString(date);
                ExcelFilesProcessor excelFilesProcessor = AddFilesToExcelFileProcessor(files, month, correctDate);
                List<PersonData> updatedDataWithCorrectNotes = GeneratePersonDataList(excelFilesProcessor);
                WriteTETCSVDataToFile(excelFilesProcessor, TET_FILE_NAME, updatedDataWithCorrectNotes);
                WriteFEJCSVDataToFile(excelFilesProcessor, FEJ_FILE_NAME, updatedDataWithCorrectNotes);
                return true;
            } else {
                return false;
            }
        }

        private List<PersonData> GeneratePersonDataList(ExcelFilesProcessor excelFilesProcessor) {
            List<PersonData> data = excelFilesProcessor.CreateCompleteListFromPersonDataInAllFiles();
            return excelFilesProcessor.PersonDataConverter.ChangeNoteNumbers(data);
        }
        
        private String CreateDateFromString(String date) {
            String removed = date.Remove(4, 1);
            return removed.Remove(6, 1);
        }

        private bool ValidateFilesAndTextInput(String month, String date, String[] files) {
            bool result = true;
            if (!Validator.CheckIfFilesPresentInDirectory(files)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(NO_FILE_TEXT);
                result = false;
            }
            if (!Validator.CheckIfMonthInCorrectForm(month)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(WRONG_MONTH_TEXT);
                result = false;
            }
            if (!Validator.CheckIfAccountingDateInCorrectForm(date)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(WRONG_FILENAME_TEXT);
                result = false;
            }
            return result;
        }

        private void EmptyFolders() {
            FolderOperation.DeleteFiles(SourceFilesFolder);
            FolderOperation.DeleteFiles(TargetFilesFolder);
        }

        private ExcelFilesProcessor AddFilesToExcelFileProcessor(String[] files, string month, String accountingDate) {
            ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(month, accountingDate);
            excelFilesProcessor.FilePaths = files;
            return excelFilesProcessor;
        }

        private void WriteTETCSVDataToFile(ExcelFilesProcessor excelFilesProcessor, String targetFileName, List<PersonData> personData) {
            List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToTETCSVList(personData);
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            excelFilesProcessor.WriteCSVFile(targetFile, csvResult);
        }

        private void WriteFEJCSVDataToFile(ExcelFilesProcessor excelFilesProcessor, String targetFileName, List<PersonData> personData) {
            List<FejCSVData> fejData = excelFilesProcessor.TransformCompletePersonDataListToFEJCSVList(personData);
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            excelFilesProcessor.WriteCSVFile(targetFile, fejData);
        }
    }
}
