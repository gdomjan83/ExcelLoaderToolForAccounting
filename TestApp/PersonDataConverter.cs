using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestApp {
    public class PersonDataConverter {

        public Dictionary<String, int> ColumnTitles { get; set; }

        public ExcelReadOperation ExcelReadOperation { get; set; }

        public List<PersonData> ProcessedPeople { get; set; }

        public PersonDataConverter(ExcelReadOperation excelReadOperation) {
            this.ExcelReadOperation = excelReadOperation;
            ColumnTitles = new Dictionary<String, int>();
            ProcessedPeople = new List<PersonData>();
        }

        public Dictionary<String, int> FindColumnTitles() {
            Workbook wb = ExcelReadOperation.ExcelInputOutputOperations.WorkbookUsed;
            Worksheet ws = ExcelReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
            int columnNumber = ExcelRowColumnCounter.GetLastColumn(wb, ws);
            for (int i = 1; i <= columnNumber; i++) {
                String cellValue = ExcelReadOperation.ReadExcelCell(1, i);
                switch (cellValue) {                    
                    case ("Név"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    case ("Hónap"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    case ("Terhelés"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    case ("Számfejtés"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    case ("Bér"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    case ("Járulék"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    case ("Okmány"):
                        ColumnTitles.Add(cellValue, i);
                        break;
                    default:
                        break;
                }
            }
            return ColumnTitles;
        }

        public List<PersonData> SavePersonDataToList(String monthFilter) {
            try {
                Workbook wb = ExcelReadOperation.ExcelInputOutputOperations.WorkbookUsed;
                Worksheet ws = ExcelReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
                int lastRow = ExcelRowColumnCounter.GetLastRow(wb, ws);
                int idNumber = 1;
                for (int i = 2; i <= lastRow; i++) {
                    String month = ExcelReadOperation.ReadExcelCell(i, ColumnTitles["Hónap"]);
                    if (monthFilter.Equals(month)) {
                        ProcessedPeople.Add(SavePerson(i, idNumber, month));
                        idNumber++;
                    }
                }
            } finally {
                ExcelReadOperation.ExcelInputOutputOperations.CloseApplication();
            }            
            return ProcessedPeople;
        }

        private PersonData SavePerson(int rowNumber, int id, String month) {
            PersonData person = null;
            try {                
                String idNumber = id.ToString();
                String name = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Név"]);
                String credit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Terhelés"]);
                String debit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Számfejtés"]);
                int salary = Int32.Parse(ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Bér"]));
                int tax = Int32.Parse(ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Járulék"]));
                String note = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Okmány"]);
                person = new PersonData(idNumber, name, month, credit, debit, salary, tax, note);
            } catch (Exception e) {
                Console.WriteLine("Error during Save Person.");
            }
            return person;            
        }
    }
}
