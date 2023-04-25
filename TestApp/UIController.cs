using Berbetolto;
using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public enum UIVersion {
            Console, Windows
        }
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public NoteCounterData NoteCounterData { get; set; }

        public String MonthToFilter { get; set; }

        public UIVersion UIVersionToRun { get; set; }

        private const String GOODBYE_TEXT = "Nyomj Entert a végeredmény megtekintéséhez (Result könyvtár megnyitása).";            
        private const String MONTH_TEXT = "Melyik hónapot szeretnéd könyvelni (formátum -> 2023.03):";
        private const String WARNING_TEXT = "Betöltés befejezve.\nFigyelem, az M oszlop csak tájékoztatásul szerepel a CSV fájlban, SAP betöltés előtt kérem törölni!";
        private const String NO_FILE_TEXT = "Nem találhatók excel fájlok a forrás (Resources) könyvtárban.\n";
        private const String END_TEXT = "Nyomj Entert a kilépéshez.";
        private const String WRONG_MONTH_TEXT = "Nem megfelelő a megadott dátum formátum. Kérlek próbáld újra:";

        public UIController(UIVersion uiVersionToRun) {
            NoteCounterData = new NoteCounterData();
            UIVersionToRun = uiVersionToRun;
            SetupFolders();
        }        
        public void RunApplication() {            
            bool finished = RunExcelOperations(UIVersionToRun);
            if (finished) {
                FinishTask();
            }
        }

        private void FinishTask() {
            if (UIVersionToRun == UIVersion.Console) {
                Console.WriteLine(WARNING_TEXT);
                Console.WriteLine(GOODBYE_TEXT);
                Console.ReadLine();
            } else {
                MessageBox.Show(WARNING_TEXT);
            }            
            Process.Start("explorer.exe", TargetFilesFolder);
        }

        private void SetupFolders() {
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            SourceFilesFolder = Path.Combine(currentDirectory, @"Resources");
            Directory.CreateDirectory(SourceFilesFolder);
            TargetFilesFolder = Path.Combine(currentDirectory, @"Result");
            Directory.CreateDirectory(TargetFilesFolder);
        }

        private String AskInputForMonth() {
            Console.WriteLine(MONTH_TEXT);
            String month = Console.ReadLine();
            return ValidateInput(month);
        }

        private String ValidateInput(String month) {
            while (!Validator.CheckIfMounthInCorrectForm(month)) {
                Console.WriteLine(WRONG_MONTH_TEXT);
                month = Console.ReadLine();
            }
            return month;
        }

        private bool RunExcelOperations(UIVersion versionToRun) {
            String[] files = FolderOperation.FindFilesInSourceDirectory(SourceFilesFolder);
            if (Validator.CheckIfFilesPresentInDirectory(files)) {
                MonthToFilter = (UIVersionToRun == UIVersion.Console) ? AskInputForMonth() : GetMonthToFilterForWindowVersion();
                ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(NoteCounterData, MonthToFilter);
                excelFilesProcessor.FilePaths = files;

                List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToCSVList();
                excelFilesProcessor.WriteCSVFile(TargetFilesFolder, csvResult);
                return true;
            } else {
                Console.WriteLine(NO_FILE_TEXT);
                Console.WriteLine(END_TEXT);
                Console.ReadLine();
                return false;
            }
        }

        private String GetMonthToFilterForWindowVersion() {
            return "2023.03";
        }
    }
}
