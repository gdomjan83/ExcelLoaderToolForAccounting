using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text;

namespace TestApp {
    public class ExcelInputOutputOperations {
        public String FilePath { get; set; }
        public Microsoft.Office.Interop.Excel.Application Application { get; set; }
        public Workbook WorkbookUsed { get; set; }
        public Worksheet WorkSheetUsed { get; set; }
        public Process[] AlreadyOpenedExcelProcesses { get; set; }
        public int OurProcessId { get; set; }

        public String[] FilesInDirectory { get; set; }

        public ExcelInputOutputOperations(String filePath, String worksheet) {
            try {
                AlreadyOpenedExcelProcesses = FindCurrentExcelProcesses();
                Application = new Microsoft.Office.Interop.Excel.Application();
                OurProcessId = FindThisExcelProcess();
                WorkbookUsed = Application.Workbooks.Open(filePath);
                WorkSheetUsed = WorkbookUsed.Worksheets[worksheet];
            } catch (Exception e) {
                Console.WriteLine("Excel file can not be opened.");
                CloseApplication();
            }
        }
        public void CloseApplication() {
            WorkbookUsed?.Close();               
            Application.Quit();
            KillProcessById(OurProcessId);            
        }

        public void WriteListToCSVFile(String filePath, List<PersonCSVData> personCSVDatas) {
            String currentFileName = FolderOperation.CreateNewFile(filePath);
            using (StreamWriter sw = new StreamWriter(new FileStream(currentFileName, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8)) {
                foreach (PersonCSVData actual in personCSVDatas) {
                    sw.WriteLine(actual.CSVFormating());
                }
                sw.Close();
            }
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
