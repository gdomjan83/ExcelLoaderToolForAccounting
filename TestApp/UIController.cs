using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public NoteCounterData NoteCounterData { get; set; }

        public String MonthToFilter { get; set; }

        private const String GOODBYE_TEXT = "Nyomj Entert a végeredmény megtekintéséhez.";
        private const String MONTH_TEXT = "Melyik hónapot szeretnéd könyvelni (formátum -> 2023.03):";
        private const String NO_FILE_TEXT = "Nem találhatók excel fájlok a forrás könyvtárban.";
        private const String END_TEXT = "Nyomj Entert a kilépéshez.";

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
            Console.WriteLine(GOODBYE_TEXT);
            Console.ReadLine();
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
            return month;
        }

        private bool RunExcelOperations() {
            String[] files = FolderOperation.FindFilesInSourceDirectory(SourceFilesFolder);
            if (CheckIfSourceFilesPresent(files)) {
                MonthToFilter = AskInputForMonth();
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

        private bool CheckIfSourceFilesPresent(String[] files) {
            if (files.Length != 0) {
                return true;
            }
            return false;
        }
    }
}
