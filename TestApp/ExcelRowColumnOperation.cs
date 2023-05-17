
using Microsoft.Office.Interop.Excel;
using System.Text;

namespace TestApp {
    public static class ExcelRowColumnOperation {
        private const string LABEL_MISSING_TEXT = "Az első sorban hiányoznak a következő cimkék: ";
        private const int LAST_SEARCHED_COLUMN = 100;
        public static readonly String[] TITLES = { "név", "hónap", "terhelés", "számfejtés", "bér", "járulék", "adóazonosító", "kihagyás" };
        public static int GetLastRow(Workbook workbook, Worksheet worksheet) {
            int result = worksheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByRows, SearchDirection: XlSearchDirection.xlPrevious).Row;
            return result;
        }

        public static int GetLastColumn(Workbook workbook, Worksheet workSheet, ExcelReadOperation fileReadOperation) {
            for (int i = LAST_SEARCHED_COLUMN; i > 0; i--) {
                String cellValue = fileReadOperation.ReadExcelCell(1, i);
                if (!String.IsNullOrEmpty(cellValue)) {
                    return i;
                }
            }
            throw new ArgumentException(LABEL_MISSING_TEXT + FolderOperation.GetFileNameFromPath(fileReadOperation.FileInputOutputOperations.FilePath));
        }

        public static Dictionary<String, int> FindColumnTitles(ExcelReadOperation fileReadOperation) {            
            Workbook wb = fileReadOperation.FileInputOutputOperations.WorkbookUsed;
            Worksheet ws = fileReadOperation.FileInputOutputOperations.WorkSheetUsed;
            int columnNumber = ExcelRowColumnOperation.GetLastColumn(wb, ws, fileReadOperation);
            Dictionary<String, int> columnTitles = IterateThroughFirstRowToFindLabels(fileReadOperation, columnNumber);
            if (Validator.CheckIfAllLabelsFound(TITLES, columnTitles)) {
                return columnTitles;
            }
            String missingLabelsText = GetMissingLabels(TITLES, columnTitles);
            throw new ArgumentException(LABEL_MISSING_TEXT + missingLabelsText + "Fájl: " + FolderOperation.GetFileNameFromPath(fileReadOperation.FileInputOutputOperations.FilePath));
        }

        private static String GetMissingLabels(String[] titles, Dictionary<String, int> columnTitles) {
            List<String> missingLabels = Validator.CheckWhichLabelsAreMissing(TITLES, columnTitles);
            return ConcatenateMissingLabels(missingLabels);
        }

        private static String ConcatenateMissingLabels(List<String> missingLabels) {
            StringBuilder sb = new StringBuilder();
            foreach(String actual in missingLabels) {
                sb.Append(actual + ", ");
            }
            sb.Append("\n");
            return sb.ToString();
        }
       
        private static Dictionary<String, int> IterateThroughFirstRowToFindLabels(ExcelReadOperation excelReadOperation, int lastColumnNumber) {
            Dictionary<String, int> columnTitles = new Dictionary<String, int>();
            for (int i = 1; i <= lastColumnNumber; i++) {
                String cellValue = excelReadOperation.ReadExcelCell(1, i).Trim().ToLower();
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
