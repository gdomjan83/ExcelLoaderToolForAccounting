using System.Diagnostics;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }
        public NoteCounterData NoteCounterData { get; set; }

        public String MonthToFilter { get; set; }

        private const String GOODBYE_TEXT = "Nyomd meg az Entert a végeredmény megtekintéséhez.";

        public UIController(NoteCounterData noteCounterData) {
            NoteCounterData = noteCounterData;
            SetupFolders();
        }        
        public void RunApplication() {
            MonthToFilter = AskInputForMonth();
            bool finished = RunExcelOperations();
            if (finished) {
                FinishTask();
            }

        }
        private void FinishTask() {
            Console.WriteLine("Nyomj Entert a végeredmény megtekintéséhez.");
            Console.ReadLine();
            Process.Start("explorer.exe", TargetFilesFolder);
        }

        private void SetupFolders() {
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            SourceFilesFolder = Path.Combine(currentDirectory, @"..\..\..\Resources");
            Directory.CreateDirectory(SourceFilesFolder);
            TargetFilesFolder = Path.Combine(currentDirectory, @"..\..\..\Result");
            Directory.CreateDirectory(TargetFilesFolder);
        }

        private String AskInputForMonth() {
            Console.WriteLine("Melyik hónapot szeretnéd könyvelni (formátum -> 2023.03):");
            String month = Console.ReadLine();
            return month;
        }

        private bool RunExcelOperations() {
            ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(NoteCounterData, MonthToFilter);
            String[] files = FolderOperation.FindFilesInSourceDirectory(SourceFilesFolder);
            if (CheckIfSourceFilesPresent(files)) {
                excelFilesProcessor.FilePaths = files;

                List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToCSVList();
                excelFilesProcessor.WriteCSVFile(TargetFilesFolder, csvResult);
                return true;
            } else {
                Console.WriteLine("Nem találhatók excel fájlok a forrás könyvtárban.");
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
