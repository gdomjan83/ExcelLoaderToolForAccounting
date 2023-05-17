using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text;

namespace TestApp {
    public class FileInputOutputOperations {
        public String FilePath { get; set; }
        public Microsoft.Office.Interop.Excel.Application Application { get; set; }
        public Workbook WorkbookUsed { get; set; }
        public Worksheet WorkSheetUsed { get; set; }
        public Process[] AlreadyOpenedExcelProcesses { get; set; }
        public int OurProcessId { get; set; }
        public static List<String> CostExcelFiles { get; set; } = new List<String>();

        public void OpenExcelFileAndWorkBook(String filePath, String worksheet) {
            try {
                AlreadyOpenedExcelProcesses = FindCurrentExcelProcesses();
                Application = new Microsoft.Office.Interop.Excel.Application();
                OurProcessId = FindThisExcelProcess();
                FilePath = filePath;
                WorkbookUsed = Application.Workbooks.Open(FilePath);
                WorkSheetUsed = WorkbookUsed.Worksheets[worksheet];
            } catch (Exception e) {
                throw new IOException($"Nem sikerült az Excel file megnyitása: {FolderOperation.GetFileNameFromPath(FilePath)}.\n" +
                    $"A hiba lehetséges oka: nem található Bérköltség elnevezésű munkalap.");
            }
        }

        public void CloseApplication() {
            WorkbookUsed?.Close();               
            Application.Quit();
            KillProcessById(OurProcessId);            
        }

        public void WriteListToCSVFile(String filePath, List<String> personCSVDatas) {
            String currentFileName = FolderOperation.CreateNewFile(filePath);
            using (StreamWriter sw = new StreamWriter(new FileStream(currentFileName, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8)) {
                foreach (String actual in personCSVDatas) {
                    sw.WriteLine(actual);
                }
                sw.Close();
            }
        }

        public void WriteTXTFile(String filePath, String[] filePaths) {
            String currentFileName = FolderOperation.CreateNewFile(filePath);
            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8)) {
                foreach (String actual in filePaths) {
                    sw.WriteLine(actual);
                }
                sw.Close();
            }
        }

        public String[] OpenTXTFile(String filePath) {
            if (File.Exists(filePath)) {
                return File.ReadAllLines(filePath);
            }
            return new String[0];
        }

        private Process[] FindCurrentExcelProcesses() {
            return Process.GetProcessesByName("excel");
        }

        private int FindThisExcelProcess() {
            Process[] currentProcesses = FindCurrentExcelProcesses();
            bool found;
            foreach(Process actual in currentProcesses) {
                found = CompareProcessArrays(actual, AlreadyOpenedExcelProcesses);
                if (!found) {
                    OurProcessId = actual.Id;
                }
            }
            return OurProcessId;               
        }

        private void KillProcessById(int processId) {
            AlreadyOpenedExcelProcesses = FindCurrentExcelProcesses();
            foreach (Process proc in AlreadyOpenedExcelProcesses) {
                if (proc.Id == processId) {
                    proc.Kill();
                }
            }
        }

        private bool CompareProcessArrays(Process currentProcess, Process[] originalProcesses) {
            bool found = false;
            foreach (Process actual in originalProcesses) {
                if (actual.Id == currentProcess.Id) {
                    found = true;
                }
            }
            return found;
        }
    }
}
