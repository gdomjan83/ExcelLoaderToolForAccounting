using Berbetolto;
using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public NoteCounterData NoteCounterData { get; set; }

        public String MonthToFilter { get; set; }

        private const String WARNING_TEXT = "Betöltés befejezve.\nFigyelem, az M oszlop csak tájékoztatásul szerepel a CSV fájlban, SAP betöltés előtt kérem törölni!";
        private const String NO_FILE_TEXT = "Nem találhatók excel fájlok a forrás (Resources) könyvtárban.\n";
        private const String END_TEXT = "Nyomj Entert a kilépéshez.";
        private const String WRONG_MONTH_TEXT = "Nem megfelelő a megadott dátum formátum. Kérlek próbáld újra:";

        public UIController() {
            NoteCounterData = new NoteCounterData();
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
            SourceFilesFolder = Path.Combine(currentDirectory, @"Resources");
            Directory.CreateDirectory(SourceFilesFolder);
            TargetFilesFolder = Path.Combine(currentDirectory, @"Result");
            Directory.CreateDirectory(TargetFilesFolder);
        }

        private String ValidateInput(String month) {
            while (!Validator.CheckIfMounthInCorrectForm(month)) {
                Console.WriteLine(WRONG_MONTH_TEXT);
                month = Console.ReadLine();
            }
            return month;
        }

        private bool RunExcelOperations() {
            String[] files = FolderOperation.FindFilesInSourceDirectory(SourceFilesFolder);
            if (Validator.CheckIfFilesPresentInDirectory(files)) {
                MonthToFilter = GetMonthToFilterForWindowVersion();
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
