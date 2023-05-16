using Berbetolto;
using System.Diagnostics;
using System.Globalization;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public String SaveFileFolder { get; set; }
        public MainWindowForm MainWindowForm { get; set; }
        public FilesProcessor FilesProcessor { get; set; }
        public Dictionary<String, double> TotalAccountingPerProjects { get; set; } = new Dictionary<String, double>();

        private const String WARNING_TEXT = "Betöltés befejezve.\nFigyelem, az M oszlop csak tájékoztatásul szerepel a TET.csv fájlban, SAP betöltés előtt kérem törölni!\n" +
            "A FEJ.CSV-ben kérem ne felejtse el kitölteni a Vegyes sorszámokat tartalmazó oszlopot.";
        private const String NO_FILE_TEXT = "\nKérem adja meg, hogy melyik fájlokból kerüljenek betöltésre a költségek!";
        private const String WRONG_MONTH_TEXT = "\nNem megfelelő a szűréshez megadott dátum formátum.";
        private const String WRONG_ACCOUNTING_DATE_TEXT = "\nNem megfelelő a könyvelési dátum formátuma.";
        private const String TET_FILE_NAME = "TET.csv";
        private const String FEJ_FILE_NAME = "FEJ.csv";
        private const String SAVE_FILE_NAME = "costfiles.txt";
        private const String TAX_FILE_NAME = "adoazonositok.csv";
        private const String AMOUNTS_TEXT = "\nLekönyvelt összegek projektenként: \n";
        private const String DECIMAL_WARNING_TEXT = "Figyelem, nem egész szám!";

        public UIController(MainWindowForm mainWindowForm) {
            MainWindowForm = mainWindowForm;
            SetupFolders();
            FilesProcessor = new FilesProcessor();

        }        
        public void RunApplication() {
            try {
                bool finished = RunExcelOperations();                
                if (finished) {                    
                    FinishTask();
                }
            } catch (ArgumentException ae) {
                MainWindowForm.SetImage(ProgressState.Error);
                MainWindowForm.AddTextToTextBox("\nHIBA: " + ae.Message);
            } catch (IOException ioe) {
                MainWindowForm.SetImage(ProgressState.Error);
                MainWindowForm.AddTextToTextBox("\nFÁJLKEZELÉSI HIBA: " + ioe.Message);
            } catch (Exception e) {
                MainWindowForm.SetImage(ProgressState.Error);
                MainWindowForm.AddTextToTextBox("\nKRITIKUS HIBA: " + e.Message);
            }
        }

        public String[] LoadLastSave() {
            return FilesProcessor.FileInputOutputOperations.OpenTXTFile(SaveFileFolder);
        }

        private bool CheckIfThereWereMissedPeople(List<PersonData> missedPeople) {
            return missedPeople.Count > 0;
        }

        private void FinishTask() {
            MainWindowForm.SetImage(ProgressState.Finished);
            if (MainWindowForm.RadioButtonState == GeneratorState.Salary) {
                WriteMissedPeople(FilesProcessor.PersonDataConverter.MissedPeople);
                WriteProjectTotals();
                MessageBox.Show(WARNING_TEXT);
                NoteCounterData.ResetProperties();
            }
            MainWindowForm.AddTextToTextBox("\nGenerálás befejezve.");
            Process.Start("explorer.exe", TargetFilesFolder);
        }

        private void WriteMissedPeople(List<PersonData> missedPeople) {
            if (CheckIfThereWereMissedPeople(missedPeople)) {
                MainWindowForm.AddTextToTextBox(FilesProcessor.PersonDataConverter.GetMissedPeopleText());
            }
        }

        private void WriteProjectTotals() {
            MainWindowForm.AddTextToTextBox(AMOUNTS_TEXT);
            var formatter = new NumberFormatInfo { NumberGroupSeparator = " " };
            foreach (KeyValuePair<String, double> actual in TotalAccountingPerProjects) {
                double amount = actual.Value / 2;
                String amountFormatted = amount.ToString("n", formatter);
                String warning = amount != (int)amount ? DECIMAL_WARNING_TEXT : "";
                MainWindowForm.AddTextToTextBox($"{actual.Key}: {amountFormatted} Ft {warning}\n");
            }
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
            TotalAccountingPerProjects.Clear();
            String[] files = FolderOperation.CopySeveralFiles(FileInputOutputOperations.CostExcelFiles, SourceFilesFolder).ToArray();
            return ProcessFiles(files);
        }

        private bool ProcessFiles(String[] files) {
            if (files.Length > 0) {
                SaveUsedFilePaths();
            }
            String month = MainWindowForm.GetMonth();
            String date = MainWindowForm.GetAccountingDate();
            if (ValidateFilesAndTextInput(month, date, files)) {
                String correctDate = String.IsNullOrEmpty(date) ? "9999.12.31" : CreateDateFromString(date);
                MainWindowForm.SetImage(ProgressState.InProgress);
                UpdateFilesProcessorProperties(FilesProcessor, month, correctDate);
                AddFilePathsToExcelFileProcessor(files);
                GenerateFiles(month, correctDate, files);                
                return true;
            } else {
                return false;
            }
        }

        private void GenerateFiles(String month, String date, String[] files) {
            if (MainWindowForm.RadioButtonState == GeneratorState.Salary) {
                List<PersonData> updatedDataWithCorrectNotes = GeneratePersonDataList();
                WriteTETCSVDataToFile(TET_FILE_NAME, updatedDataWithCorrectNotes);
                WriteFEJCSVDataToFile(FEJ_FILE_NAME, updatedDataWithCorrectNotes);
            } else if (MainWindowForm.RadioButtonState == GeneratorState.TaxId) {                 
                List<PersonData> data = FilesProcessor.CreateCompleteListFromPersonDataInAllFiles(false);
                List<TaxIdPerProject> taxData = FilesProcessor.PersonDataConverter.GenerateTaxIdListFromPersonList(data);
                WriteTaxCSVDataToFile(TAX_FILE_NAME, ConvertTaxListToSetAndBack(taxData));                    
            }
        }

        private List<TaxIdPerProject> ConvertTaxListToSetAndBack(List<TaxIdPerProject> taxIdList) {
            HashSet<TaxIdPerProject> setOfTaxObject = taxIdList.ToHashSet<TaxIdPerProject>();
            List<TaxIdPerProject> taxObjectsNoDuplicates = setOfTaxObject.ToList<TaxIdPerProject>();
            return taxObjectsNoDuplicates.OrderBy(p => p.ProjectName).ToList<TaxIdPerProject>();
        }

        private void UpdateFilesProcessorProperties(FilesProcessor filesProcessor, String month, String correctDate) {
            filesProcessor.MonthToFilter = month;
            filesProcessor.AccountingDate = correctDate;
        }

        private List<PersonData> GeneratePersonDataList() {
            List<PersonData> data = FilesProcessor.CreateCompleteListFromPersonDataInAllFiles(true);
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
            if (MainWindowForm.RadioButtonState == GeneratorState.Salary && !Validator.CheckIfAccountingDateInCorrectForm(date)) {
                WindowOperations.mainWindowForm.AddTextToTextBox(WRONG_ACCOUNTING_DATE_TEXT);
                result = false;
            }
            if (!result) {
                MainWindowForm.SetImage(ProgressState.Error);
            }
            return result;
        }

        private void EmptyFolders() {
            FolderOperation.DeleteFiles(SourceFilesFolder);
            FolderOperation.DeleteFiles(TargetFilesFolder);
        }

        private void AddFilePathsToExcelFileProcessor(String[] files) {
            FilesProcessor.FilePaths = files;           
        }

        private void WriteTETCSVDataToFile(String targetFileName, List<PersonData> personData) {
            List<PersonCSVData> csvResult = FilesProcessor.TransformCompletePersonDataListToTETCSVList(personData);
            List<PersonCSVData> orderedList = csvResult.OrderBy(p => p.Note).ToList<PersonCSVData>();
            CountTotals(orderedList);
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            FilesProcessor.WriteCSVFile(targetFile, orderedList);
        }

        private void WriteTaxCSVDataToFile(String targetFileName, List<TaxIdPerProject> personData) {
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            FilesProcessor.WriteCSVFile(targetFile, personData);
        }

        private void CountTotals(List<PersonCSVData> personList) {            
            foreach (PersonCSVData actual in personList) {
                double amount = Double.Parse(actual.Amount) + (TotalAccountingPerProjects.ContainsKey(actual.ProjectName) ? TotalAccountingPerProjects[actual.ProjectName] : 0);
                TotalAccountingPerProjects[actual.ProjectName] = amount;
            }            
        }

        private void WriteFEJCSVDataToFile(String targetFileName, List<PersonData> personData) {
            List<FejCSVData> fejData = FilesProcessor.TransformCompletePersonDataListToFEJCSVList(personData);
            List<FejCSVData> orderedList = fejData.OrderBy(f => f.Note).ToList<FejCSVData>();
            String targetFile = Path.Combine(TargetFilesFolder, targetFileName);
            FilesProcessor.WriteCSVFile(targetFile, orderedList);
        }

        private void SaveUsedFilePaths() {
            FilesProcessor.WriteTXTFile(SaveFileFolder, FileInputOutputOperations.CostExcelFiles);
        }
    }
}
