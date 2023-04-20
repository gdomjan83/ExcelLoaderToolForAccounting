using Microsoft.Office.Interop.Excel;
using System.Collections;

namespace TestApp {
    public static class ExcelRowColumnCounter {
        public static int GetLastRow(Workbook workbook, Worksheet worksheet) {
            int result = worksheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByRows, SearchDirection: XlSearchDirection.xlPrevious).Row;
            return result;
        }

        public static int GetLastColumn(Workbook workbook, Worksheet workSheet) {
            int result = workSheet.Cells.Find("*", SearchOrder: XlSearchOrder.xlByColumns, SearchDirection: XlSearchDirection.xlPrevious).Column;
            return result;
        }
    }
}
