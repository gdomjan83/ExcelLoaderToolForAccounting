using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public static class ExcelRowColumnOperation {
        public static readonly String[] TITLES = { "Név", "Hónap", "Terhelés", "Számfejtés", "Bér", "Járulék" };
        public static int GetLastRow(Workbook workbook, Worksheet worksheet) {
            int result = worksheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByRows, SearchDirection: XlSearchDirection.xlPrevious).Row;
            return result;
        }

        public static int GetLastColumn(Workbook workbook, Worksheet workSheet) {
            int result = workSheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByColumns, SearchDirection: XlSearchDirection.xlPrevious).Column;
            return result;
        }

        public static Dictionary<String, int> FindColumnTitles(ExcelReadOperation excelReadOperation) {
            Dictionary<String, int> columnTitles = new Dictionary<String, int>();
            Workbook wb = excelReadOperation.ExcelInputOutputOperations.WorkbookUsed;
            Worksheet ws = excelReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
            int columnNumber = ExcelRowColumnOperation.GetLastColumn(wb, ws);
            for (int i = 1; i <= columnNumber; i++) {
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
