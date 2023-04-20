using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public class ExcelReadOperation {
        public ExcelInputOutputOperations ExcelInputOutputOperations { get; set; }

        public ExcelReadOperation(ExcelInputOutputOperations excelInputOutputOperations) {
            ExcelInputOutputOperations = excelInputOutputOperations;
        }
           
        public List<String> ReadExcelRange(String rangeLabels) { //rangeLabels - pl. "A1:A2"
            List<String> result = new List<String>();
            try {
                result = ParseRangeAndReturnValues(rangeLabels, result);
            } catch (Exception e) {
                Console.WriteLine("Cell value error.");
                ExcelInputOutputOperations.CloseApplication();
            } 
            return result;
        }

        public String ReadExcelCell(int rowNumber, int columnNumber) {
            String result = String.Empty;
            try {
                result = ParseCellAndReturnValue(rowNumber, columnNumber);
            } catch (Exception e) {
                Console.WriteLine("Cell value error.");
                ExcelInputOutputOperations.CloseApplication();
            } 
            return result;
        }

        private List<String> ParseRangeAndReturnValues(String rangeLabels, List<String> resultList) {            
            Worksheet ws = ExcelInputOutputOperations.WorkSheetUsed;
            Microsoft.Office.Interop.Excel.Range cell = ws.Range[rangeLabels];
            foreach (String actual in cell.Value) {
                resultList.Add(actual);
            }
            return resultList;
        }

        private String ParseCellAndReturnValue(int rowNumber, int columnNumber) {
            String result = String.Empty;
            Worksheet ws = ExcelInputOutputOperations.WorkSheetUsed;
            Microsoft.Office.Interop.Excel.Range cell = ws.Cells[rowNumber, columnNumber];
            if (cell.Value != null) {
                result = cell.Value.ToString();
            }
            return result;
        }
    }
}
