﻿using Berbetolto;
using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public class PersonDataConverter {
        public const String SALARY_LEDGER_NUMBER = "541100";
        public const String TAX_LEDGER_NUMBER = "561000";
        public const String CREDIT_CODE = "40";
        public const String DEBIT_CODE = "50";
        public const int LINES_IN_ONE_NOTE = 80;
        public Dictionary<String, int> ColumnTitles { get; set; }

        public ExcelReadOperation ExcelReadOperation { get; set; }

        public List<PersonData> ProcessedPeople { get; set; } = new List<PersonData>();

        public String MonthToFilter { get; set; }
        public String FileName { get; set; }

        public PersonDataConverter(ExcelReadOperation excelReadOperation, String monthToFilter, String fileName) {
            ExcelReadOperation = excelReadOperation;
            ColumnTitles = ExcelRowColumnOperation.FindColumnTitles(excelReadOperation);
            MonthToFilter = monthToFilter;
            FileName = fileName;
        }

        public List<PersonData> SavePersonDataToList() {
            try {
                Workbook wb = ExcelReadOperation.ExcelInputOutputOperations.WorkbookUsed;
                Worksheet ws = ExcelReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
                int lastRow = ExcelRowColumnOperation.GetLastRow(wb, ws);
                int idNumber = 1;
                for (int i = 2; i <= lastRow; i++) {
                    idNumber = FilterMonthAndSavePersonToList(MonthToFilter, i, idNumber);
                }
            } finally {
                ExcelReadOperation.ExcelInputOutputOperations.CloseApplication();
            }
            return ProcessedPeople;
        }

        public List<PersonCSVData> ConvertPersonDataToCSVData(List<PersonData> inputData, NoteCounterData noteCounterData) {
            List<PersonCSVData> result = new List<PersonCSVData>();
            List<PersonData> updatedPersonData = ChangeNoteNumbers(inputData, noteCounterData);
            foreach (PersonData actual in updatedPersonData) {
                AddNewPersonCSVData(result, actual);
            }
            return result;
        }

        private PersonCSVData TransformCreditSalary(PersonData person) {
            String comment = $"{person.DebitCostCenter} {MonthToFilter} {person.Name}";
            return new PersonCSVData(person.Note, CREDIT_CODE, SALARY_LEDGER_NUMBER, person.Salary,
                comment, person.CreditCostCenter, person.CreditCostCenter.Substring(0, 3), person.ProjectName);
        }

        private PersonCSVData TransformCreditTax(PersonData person) {
            String comment = $"{person.DebitCostCenter} {MonthToFilter} {person.Name}";
            return new PersonCSVData(person.Note, CREDIT_CODE, TAX_LEDGER_NUMBER, person.Tax,
                comment, person.CreditCostCenter, person.CreditCostCenter.Substring(0, 3), person.ProjectName);
        }

        private PersonCSVData TransformDebitSalary(PersonData person) {
            String comment = $"{person.CreditCostCenter} {MonthToFilter} {person.Name}";
            return new PersonCSVData(person.Note, DEBIT_CODE, SALARY_LEDGER_NUMBER, person.Salary,
                comment, person.DebitCostCenter, person.DebitCostCenter.Substring(0, 3), person.ProjectName);
        }

        private PersonCSVData TransformDebitTax(PersonData person) {
            String comment = $"{person.CreditCostCenter} {MonthToFilter} {person.Name}";
            return new PersonCSVData(person.Note, DEBIT_CODE, TAX_LEDGER_NUMBER, person.Tax,
                comment, person.DebitCostCenter, person.DebitCostCenter.Substring(0, 3), person.ProjectName);
        }

        private void AddNewPersonCSVData(List<PersonCSVData> resultList, PersonData inputPersonData) {
            if (!Validator.CheckIfAmountIsZero(inputPersonData.Salary)) {
                resultList.Add(TransformCreditSalary(inputPersonData));
                resultList.Add(TransformDebitSalary(inputPersonData));
            }
            if (!Validator.CheckIfAmountIsZero(inputPersonData.Tax)) {
                resultList.Add(TransformCreditTax(inputPersonData));
                resultList.Add(TransformDebitTax(inputPersonData));
            }
        }

        private List<PersonData> ChangeNoteNumbers(List<PersonData> inputData, NoteCounterData noteCounterData) {
            for (int i = 0; i < inputData.Count; i++) {
                UpdateNoteDataForPerson(inputData[i], noteCounterData);
            }
            return inputData;
        }

        private void UpdateNoteDataForPerson(PersonData person, NoteCounterData counterData) {
            if (CheckIfCostCenterIsSzakma(person.DebitCostCenter)) {
                person.Note = counterData.GmiSzakmaNote.ToString();
                counterData.GmiSzakmaCounter++;
            } else {
                person.Note = counterData.GmiFpiNote.ToString();
                counterData.GmiFpiCounter++;
            }
            if (counterData.GmiSzakmaCounter % LINES_IN_ONE_NOTE == 0) {
                counterData.GmiSzakmaNote++;
            }
            if (counterData.GmiFpiCounter % LINES_IN_ONE_NOTE == 0) {
                counterData.GmiFpiNote++;
            }
        }

        private bool CheckIfCostCenterIsSzakma(String costCenter) {
            if (String.Compare(costCenter, "L") < 0) {
                return true;
            }
            return false;
        }

        private PersonData SavePerson(int rowNumber, int id, String month, String fileName) {
            PersonData person = null;
            try {
                person = CreateNewPerson(rowNumber, id, month, fileName);
            } catch (Exception e) {
                MessageBox.Show("Error during SavePerson method.");
            }
            return person;            
        }

        private PersonData CreateNewPerson(int rowNumber, int id, String month, String fileName) {
            String idNumber = id.ToString();
            String name = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Név"]);
            String credit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Terhelés"]);
            String debit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Számfejtés"]);
            String salary = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Bér"]);
            String tax = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Járulék"]);
            String note = "0";
            if (!Validator.CheckCostCenterFormat(credit) || !Validator.CheckCostCenterFormat(debit)) {
                MessageBox.Show($"Hibás pénzügyi központ formátum a következő fájlban: {fileName} - {name}");
            }
            return new PersonData(idNumber, name, month, credit, debit, salary, tax, note, fileName);
        }

        private int FilterMonthAndSavePersonToList(String monthToFilter, int currentRow, int currentId) {
            String currentMonth = ExcelReadOperation.ReadExcelCell(currentRow, ColumnTitles["Hónap"]);
            String currentMonthCleaned = currentMonth.Length > 7 ? currentMonth.Substring(0, 7) : currentMonth;
            if (monthToFilter.Equals(currentMonthCleaned)) {
                ProcessedPeople.Add(SavePerson(currentRow, currentId, currentMonthCleaned, FileName));
                currentId++;
            }
            return currentId;
        }
    }
}
