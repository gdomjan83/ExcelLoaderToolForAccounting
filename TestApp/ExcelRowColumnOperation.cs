
using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public static class ExcelRowColumnOperation {
        private const string LABEL_MISSING_TEXT = "Az első sorban csak a következő nyolc cimke szerepelhet: " +
            "Név, Hónap, Terhelés, Számfejtés, Bér, Járulék, Adóazonosító, Kihagyás. Fájl: ";
        public static readonly String[] TITLES = { "Név", "Hónap", "Terhelés", "Számfejtés", "Bér", "Járulék", "Adóazonosító", "Kihagyás" };
        public static int GetLastRow(Workbook workbook, Worksheet worksheet) {
            int result = worksheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByRows, SearchDirection: XlSearchDirection.xlPrevious).Row;
            return result;
        }

        public static int GetLastColumn(Workbook workbook, Worksheet workSheet) {
            int result = workSheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByColumns, SearchDirection: XlSearchDirection.xlPrevious).Column;
            return result;
        }

        public static Dictionary<String, int> FindColumnTitles(ExcelReadOperation fileReadOperation) {            
            Workbook wb = fileReadOperation.ExcelInputOutputOperations.WorkbookUsed;
            Worksheet ws = fileReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
            int columnNumber = ExcelRowColumnOperation.GetLastColumn(wb, ws);
            Dictionary<String, int> columnTitles = IterateThroughFirstRowToFindLabels(fileReadOperation, columnNumber);
            if (Validator.CheckIfAllLabelsFound(TITLES, columnTitles)) {
                return columnTitles;
            }            
            throw new ArgumentException(LABEL_MISSING_TEXT + FolderOperation.GetFileNameFromPath(fileReadOperation.ExcelInputOutputOperations.FilePath));
        }

        private static Dictionary<String, int> IterateThroughFirstRowToFindLabels(ExcelReadOperation excelReadOperation, int lastColumnNumber) {
            Dictionary<String, int> columnTitles = new Dictionary<String, int>();
            for (int i = 1; i <= lastColumnNumber; i++) {
                String cellValue = excelReadOperation.ReadExcelCell(1, i);
                columnTitles = FillDictionary(cellValue, i, columnTitles);
            }
            return columnTitles;
        }

        private static Dictionary<String, int> FillDictionary(String cellValue, int columnNumber, Dictionary<String, int> columnTitles) {
            foreach(String actual in TITLES) {
                if (actual.Equals(cellValue)) {
                    columnTitles.Add(actual, columnNumber);
                }
            }
            return columnTitles;             
        }
    }
}
