using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public class ExcelReadOperation {
        public FileInputOutputOperations FileInputOutputOperations { get; set; }

        public ExcelReadOperation(FileInputOutputOperations excelInputOutputOperations) {
            FileInputOutputOperations = excelInputOutputOperations;
        }
           
        public List<String> ReadExcelRange(String rangeLabels) { //rangeLabels - pl. "A1:A2"
            List<String> result = new List<String>();
            try {
                result = ParseRangeAndReturnValues(rangeLabels, result);
            } catch (Exception e) {
                throw new ArgumentException($"Nem sikerült a cella értékének kiolvasása." +
                    $" Fájl: {FolderOperation.GetFileNameFromPath(FileInputOutputOperations.FilePath)} Cellák: {rangeLabels}");                
            } 
            return result;
        }

        public String ReadExcelCell(int rowNumber, int columnNumber) {
            String result = String.Empty;
            try {
                result = ParseCellAndReturnValue(rowNumber, columnNumber);
            } catch (Exception e) {
                throw new ArgumentException($"Nem sikerült a cella értékének kiolvasása." +
                    $" Fájl: {FolderOperation.GetFileNameFromPath(FileInputOutputOperations.FilePath)} Sor: {rowNumber}, Oszlop: {columnNumber}");
            } 
            return result;
        }

        private List<String> ParseRangeAndReturnValues(String rangeLabels, List<String> resultList) {            
            Worksheet ws = FileInputOutputOperations.WorkSheetUsed;
            Microsoft.Office.Interop.Excel.Range cell = ws.Range[rangeLabels];
            foreach (String actual in cell.Value) {
                resultList.Add(actual);
            }            
            return resultList;
        }

        private String ParseCellAndReturnValue(int rowNumber, int columnNumber) {
            String result = String.Empty;
            Worksheet ws = FileInputOutputOperations.WorkSheetUsed;
            Microsoft.Office.Interop.Excel.Range cell = ws.Cells[rowNumber, columnNumber];
            if (cell.Value != null) {
                result = cell.Value.ToString();
            }
            return result;
        }
    }
}
