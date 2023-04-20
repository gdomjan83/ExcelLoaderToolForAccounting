using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class ExcelInputOutputOperations {
        public String FilePath { get; set; }
        public Application Application { get; set; }
        public Workbook WorkbookUsed { get; set; }
        public Worksheet WorkSheetUsed { get; set; }

        public ExcelInputOutputOperations(String filePath, String worksheet) {
            this.Application = new Application();
            this.WorkbookUsed = Application.Workbooks.Open(filePath);
            this.WorkSheetUsed = WorkbookUsed.Worksheets[worksheet];

        }
        public void CloseApplication() {
            WorkbookUsed.Close();
            Application.Quit();
        }
    }
}
