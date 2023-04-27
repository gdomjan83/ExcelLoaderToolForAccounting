using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public String SaveFileFolder { get; set; }
        public MainWindowForm MainWindowForm { get; set; }
        public FilesProcessor FilesProcessor { get; set; }

        private const String WARNING_TEXT = "Betöltés befejezve.\nFigyelem, az M oszlop csak tájékoztatásul szerepel a TET.csv fájlban, SAP betöltés előtt kérem törölni!";
        private const String NO_FILE_TEXT = "\nKérem adja meg, hogy melyik fájlokból töltsem be a költségeket!";
        private const String WRONG_MONTH_TEXT = "\nNem megfelelő a megadott dátum formátum.";
        private const String WRONG_FILENAME_TEXT = "\nNem megfelelő a könyvelési dátum formátuma.";
        private const String TET_FILE_NAME = "TET.csv";
        private const String FEJ_FILE_NAME = "FEJ.csv";
        private const String SAVE_FILE_NAME = "costfiles.txt";

        public UIController(MainWindowForm mainWindowForm) {
            MainWindowForm = mainWindowForm;
            SetupFolders();
            FilesProcessor = new FilesProcessor();

        }        
        public void RunApplication() {            
            bool finished = RunExcelOperations();
            if (finished) {
                FinishTask();
            }
        }

        private void FinishTask() {
            MessageBox.Show(WARNING_TEXT);
            NoteCounterData.ResetProperties();
            Process.Start("explorer.exe", TargetFilesFolder);
        }

        private void SetupFolders() {
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            SourceFilesFolder = Path.Combine(currentDirectory, "Resources");
            Directory.CreateDirectory(SourceFilesFolder);
            TargetFilesFolder = Path.Combine(currentDirectory, "Result");
            Directory.CreateDirectory(TargetFilesFolder);
            SaveFileFolder = Path.Combine(currentDirectory, SAVE_FILE_NAME); ;
        }

        private bool RunExcelOperations() {
            EmptyFolders();
            String[] files = FolderOperation.CopySeveralFiles(FileInputOutputOperations.CostExcelFiles, SourceFilesFolder).ToArray();
            return ProcessFiles(files);
        }

        private bool ProcessFiles(String[] files) {
            String month = MainWindowForm.GetMonth();
            String date = MainWindowForm.GetAccountingDate();
            if (ValidateFilesAndTextInput(month, date, files)) {
                String correctDate = CreateDateFromString(date);
                UpdateFilesProcessorProperties(FilesProcessor, month, correctDate);
                AddFilesToExcelFileProcessor(files);
                List<PersonData> updatedDataWithCorrectNotes = GeneratePersonDataList();
                WriteTETCSVDataToFile(TET_FILE_NAME, updatedDataWithCorrectNotes);
                WriteFEJCSVDataToFile(FEJ_FILE_NAME, updatedDataWithCorrectNotes);
                SaveUsedFiles();
                return true;
            } else {
                return false;
            }
        }
        public String[] LoadLastSave(FilesProcessor filesProcessor) {
            return filesProcessor.FileInputOutputOperations.OpenTXTFile(SaveFileFolder);
        }

        private void UpdateFilesProcessorProperties(FilesProcessor filesProcessor, String month, String correctDate) {
            filesProcessor.MonthToFilter = month;
            filesProcessor.AccountingDate = correctDate;
        }

        private List<PersonData> GeneratePersonDataList() {
            List<PersonData> data = FilesProcessor.CreateCompleteListFromPersonDataInAllFiles();
            return FilesProcessor.PersonDataConverter.ChangeNoteNumbers(data);
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

        private void AddFilesToExcelFileProcessor(String[] files) {
            FilesProcessor.FilePaths = files;           
        }

        private void WriteTETCSVDataToFile(String targetFileName, List<PersonData> personData) {
            List<PersonCSVData> csvResult = FilesProcessor.TransformCompletePersonDataListToTETCSVList(personData);
            List<PersonCSVData> orderedList = csvResult.OrderBy(p => p.Note).ToList<PersonCSVData>();
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            FilesProcessor.WriteCSVFile(targetFile, orderedList);
        }

        private void WriteFEJCSVDataToFile(String targetFileName, List<PersonData> personData) {
            List<FejCSVData> fejData = FilesProcessor.TransformCompletePersonDataListToFEJCSVList(personData);
            List<FejCSVData> orderedList = fejData.OrderBy(f => f.Note).ToList<FejCSVData>();
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            FilesProcessor.WriteCSVFile(targetFile, orderedList);
        }

        private void SaveUsedFiles() {
            FilesProcessor.WriteTXTFile(SaveFileFolder, FileInputOutputOperations.CostExcelFiles);
        }
    }
}
