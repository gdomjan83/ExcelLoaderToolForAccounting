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
        public static readonly String SALARY_LEDGER_NUMBER = "541100";
        public static readonly String TAX_LEDGER_NUMBER = "561000";
        public static readonly String CREDIT_CODE = "40";
        public static readonly String DEBIT_CODE = "50";

        public Dictionary<String, int> ColumnTitles { get; set; }

        public ExcelReadOperation ExcelReadOperation { get; set; }

        public List<PersonData> ProcessedPeople { get; set; } = new List<PersonData>();

        public PersonDataConverter(ExcelReadOperation excelReadOperation) {
            ExcelReadOperation = excelReadOperation;
            ColumnTitles = ExcelRowColumnOperation.FindColumnTitles(excelReadOperation);
        }

        public List<PersonData> SavePersonDataToList(String monthFilter) {
            try {
                Workbook wb = ExcelReadOperation.ExcelInputOutputOperations.WorkbookUsed;
                Worksheet ws = ExcelReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
                int lastRow = ExcelRowColumnOperation.GetLastRow(wb, ws);
                int idNumber = 1;
                for (int i = 2; i <= lastRow; i++) {
                    idNumber = FilterMonthAndSavePersonToList(monthFilter, i, idNumber);
                }
            } finally {
                ExcelReadOperation.ExcelInputOutputOperations.CloseApplication();
            }            
            return ProcessedPeople;
        }

        public List<PersonCSVData> ConvertPersonDataToCSVData(List<PersonData> inputData) {

        }
        
        private List<PersonCSVData> TransformPersonData(PersonData personData) {
            
        }

        private List<PersonData> ChangeNoteNumbers(List<PersonData> inputData) {
            int gmiSzakmaCounter;
            int gmiFpiCouner;
            int gmiSzakmaNote = 100;
            int gmiFpiNote = 200;
            for(int i = 0; i < inputData.Count; i++) {
                

            }
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
            String note = "0";
            return new PersonData(idNumber, name, month, credit, debit, salary, tax, note);
        }

        private int FilterMonthAndSavePersonToList(String monthToFilter, int currentRow, int currentId) {
            String currentMonth = ExcelReadOperation.ReadExcelCell(currentRow, ColumnTitles["Hónap"]);
            String currentMonthCleaned = currentMonth.Length > 7 ? currentMonth.Substring(0, 7) : currentMonth;
            if (monthToFilter.Equals(currentMonthCleaned)) {
                ProcessedPeople.Add(SavePerson(currentRow, currentId, currentMonthCleaned));
                currentId++;
            }
            return currentId;
        }
    }
}
