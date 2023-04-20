using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public class ExcelReadOperation {
        public ExcelInputOutputOperations ExcelInputOutputOperations { get; set; }

        public ExcelReadOperation(ExcelInputOutputOperations excelInputOutputOperations) {
            this.ExcelInputOutputOperations = excelInputOutputOperations;
        }
           
        public List<String> ReadExcelRange(String rangeLabels) { //rangeLabels - pl. "A1:A2"
            List<String> result = new List<String>();
            try {
                Worksheet ws = ExcelInputOutputOperations.WorkSheetUsed;
                Microsoft.Office.Interop.Excel.Range cell = ws.Range[rangeLabels];
                foreach(String actual in cell.Value) {
                    result.Add(actual);
                }                
            } catch (Exception e) {
                Console.WriteLine("Excel file can not be opened.");
            }
            return result;
        }

        public String ReadExcelCell(int rowNumber, int columnNumber) {
            String result = String.Empty;
            try {
                Worksheet ws = ExcelInputOutputOperations.WorkSheetUsed;
                Microsoft.Office.Interop.Excel.Range cell = ws.Cells[rowNumber, columnNumber];
                if ("Bakon Krisztián Attila".Equals(cell.Value)) {
                    String b = "3";
                }
                if (cell != null) {
                    String a = cell.Value.ToString();
                }
                result = cell.Value;
            } catch (Exception e) {
                Console.WriteLine("Excel file can not be opened.");
            }
            return result;
        }

        //private bool CheckIfNumber(Microsoft.Office.Interop.Excel.Range cell) {
           
        //}
    }
}
