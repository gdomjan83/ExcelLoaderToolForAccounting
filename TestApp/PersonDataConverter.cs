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
                FillDictionary(cellValue, i);
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
                    idNumber = FilterMonthAndSavePersonToList(monthFilter, i, idNumber);
                }
            } finally {
                ExcelReadOperation.ExcelInputOutputOperations.CloseApplication();
            }            
            return ProcessedPeople;
        }

        private PersonData SavePerson(int rowNumber, int id, String month) {
            PersonData person = null;
            try {
                person = CreateNewPerson(rowNumber, id, month);
            } catch (Exception e) {
                Console.WriteLine("Error during Save Person.");
            }
            return person;            
        }

        private PersonData CreateNewPerson(int rowNumber, int id, String month) {
            String idNumber = id.ToString();
            String name = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Név"]);
            String credit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Terhelés"]);
            String debit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Számfejtés"]);
            int salary = Int32.Parse(ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Bér"]));
            int tax = Int32.Parse(ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Járulék"]));
            String note = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Okmány"]);
            return new PersonData(idNumber, name, month, credit, debit, salary, tax, note);
        }

        private int FilterMonthAndSavePersonToList(String monthToFilter, int currentRow, int currentId) {
            String currentMonth = ExcelReadOperation.ReadExcelCell(currentRow, ColumnTitles["Hónap"]);
            if (monthToFilter.Equals(currentMonth)) {
                ProcessedPeople.Add(SavePerson(currentRow, currentId, currentMonth));
                return currentId++;
            }
            return currentId;
        }

        private void FillDictionary(String cellValue, int columnNumber) {
            switch (cellValue) {
                case ("Név"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                case ("Hónap"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                case ("Terhelés"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                case ("Számfejtés"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                case ("Bér"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                case ("Járulék"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                case ("Okmány"):
                    ColumnTitles.Add(cellValue, columnNumber);
                    break;
                default:
                    break;
            }
        }
    }
}
