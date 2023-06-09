﻿using Berbetolto;
using Microsoft.Office.Interop.Excel;
using System.Text;

namespace TestApp {
    public class PersonDataConverter {
        public const String SALARY_LEDGER_NUMBER = "541100";
        public const String TAX_LEDGER_NUMBER = "561000";
        public const String CREDIT_CODE = "40";
        public const String DEBIT_CODE = "50";
        public const String BER_SZAKMA = "BérProjektSzakma";
        public const String BER_KOZPONTI = "BérProjektGMIFPI";
        public const int LINES_IN_ONE_NOTE = 80;
        private const string ZERO_AMOUNT_TEXT = "Nulla bér és járulék";

        public Dictionary<String, int> ColumnTitles { get; set; }
        public String AccountingDate { get; set; }
        public ExcelReadOperation ExcelReadOperation { get; set; }
        public List<PersonData> ProcessedPeople { get; set; } = new List<PersonData>();
        public List<PersonData> MissedPeople { get; set; } = new List<PersonData>();
        public String MonthToFilter { get; set; }
        public String FileName { get; set; }

        public PersonDataConverter(ExcelReadOperation excelReadOperation, String monthToFilter, String fileName, String accountingDate) {
            ExcelReadOperation = excelReadOperation;          
            MonthToFilter = monthToFilter;
            FileName = fileName;
            AccountingDate = accountingDate;
        }

        public List<PersonData> SavePersonDataToList(bool validateCostCenter) {
            Workbook wb = ExcelReadOperation.FileInputOutputOperations.WorkbookUsed;
            Worksheet ws = ExcelReadOperation.FileInputOutputOperations.WorkSheetUsed;
            ColumnTitles = ExcelRowColumnOperation.FindColumnTitles(ExcelReadOperation);
            int lastRow = ExcelRowColumnOperation.GetLastRow(wb, ws);
            int idNumber = 1;
            for (int i = 2; i <= lastRow; i++) {
                idNumber = FilterMonthAndSavePersonToList(MonthToFilter, i, idNumber, validateCostCenter);
            }
            return ProcessedPeople;
        }

        public List<PersonCSVData> ConvertPersonDataToTETCSVData(List<PersonData> inputData) {
            List<PersonCSVData> result = new List<PersonCSVData>();
            for (int i = 0; i < inputData.Count; i++) {
                AddNewPersonCSVData(result, inputData[i]);
            }
            return result;
        }

        public List<FejCSVData> ConvertPersonDataToFejCSVData(List<PersonData> inputData) {
            List<FejCSVData> result = new List<FejCSVData>();
            foreach (PersonData actual in inputData) {
                AddFejDataToList(actual, result);
            }
            return result;
        }
        public List<PersonData> ChangeNoteNumbers(List<PersonData> personDataCollection) {
            List<PersonData> result = new List<PersonData>();
            List<PersonData> tempList = new List<PersonData>();
            int listSize = personDataCollection.Count;
            for (int i = 0; i < listSize; i++) {
                UpdateNoteDataForEveryPerson(personDataCollection, tempList, result, i);
            }
            return result;
        }       
        

        public String GetMissedPeopleText() {
            StringBuilder sb = new StringBuilder("\n\nKihagyásra jelölt, nem könyvelt személyek:");
            foreach(PersonData actual in MissedPeople) {
                sb.Append("\n - " + actual.ToString());
            }
            sb.Append("\n");
            return sb.ToString();
        }

        public List<TaxIdPerProject> GenerateTaxIdListFromPersonList(List<PersonData> people) {
            List<TaxIdPerProject> result = new List<TaxIdPerProject>();
            foreach(PersonData actual in people) {
                FilterThoseWhoAreSetToMissSaveOthers(actual, result);
            }
            return result;
        }

        private void UpdateNoteDataForEveryPerson(List<PersonData> allPersonDataFromEveryProject, List<PersonData> temporaryList, List<PersonData> resultList, int currentPerson) {
            int listSize = allPersonDataFromEveryProject.Count;
            UpdateNoteDataForPerson(allPersonDataFromEveryProject[currentPerson], BER_SZAKMA);
            temporaryList.Add(allPersonDataFromEveryProject[currentPerson]);
            if ((currentPerson < listSize - 1) && (allPersonDataFromEveryProject[currentPerson].ProjectName != allPersonDataFromEveryProject[currentPerson + 1].ProjectName)) {
                SecondIterationForCentralWorkerNoteUpdates(temporaryList, resultList);
                UpdateNoteForNewProject();
            }
            if (currentPerson == listSize - 1) {
                SecondIterationForCentralWorkerNoteUpdates(temporaryList, resultList);
            }
        }

        private void SecondIterationForCentralWorkerNoteUpdates(List<PersonData> temporaryPersonList, List<PersonData> resultList) {
            UpdateNoteForNewProject();
            for (int j = 0; j < temporaryPersonList.Count; j++) {
                UpdateNoteDataForPerson(temporaryPersonList[j], BER_KOZPONTI);
            }
            resultList.AddRange(temporaryPersonList);
            temporaryPersonList.Clear();
        }

        private void UpdateNoteForNewProject() {
            if (NoteCounterData.Counter != 1) {
                NoteCounterData.CurrentNote++;
                NoteCounterData.Counter = 1;
            }
        }

        private void FilterThoseWhoAreSetToMissSaveOthers(PersonData person, List<TaxIdPerProject> result) {
            if (!Validator.CheckIfMissNotificationPresent(person.Miss)) {
                Validator.CheckIfTaxIDPresent(person);
                TaxIdPerProject taxData = new TaxIdPerProject(person.TaxId, person.ProjectName);
                result.Add(taxData);
            }
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
            if (!Validator.CheckIfMissNotificationPresent(inputPersonData.Miss)) {
                ValidateAndAddPersonData(resultList, inputPersonData);
            } else {
                MissedPeople.Add(inputPersonData);
            }
        }

        private void ValidateAndAddPersonData(List<PersonCSVData> resultList, PersonData inputPersonData) {
            if (!Validator.CheckIfAmountIsZero(inputPersonData.Salary)) {
                resultList.Add(TransformCreditSalary(inputPersonData));
                resultList.Add(TransformDebitSalary(inputPersonData));
            }
            if (!Validator.CheckIfAmountIsZero(inputPersonData.Tax)) {
                resultList.Add(TransformCreditTax(inputPersonData));
                resultList.Add(TransformDebitTax(inputPersonData));
            }
        }

        private void AddFejDataToList(PersonData personData, List<FejCSVData> fejDataResult) {
            FejCSVData newFejData = new FejCSVData(personData.Note, AccountingDate, personData.WorkerType, "V bér");
            if (!fejDataResult.Contains(newFejData)) {
                fejDataResult.Add(newFejData);
            }            
        }

        private void UpdateNoteDataForPerson(PersonData person, String debitType) {
            if (debitType.Equals(person.WorkerType)) {
                person.Note = NoteCounterData.CurrentNote;
                ManageCounter();
            }
        }

        private void ManageCounter() {
            if (NoteCounterData.Counter == LINES_IN_ONE_NOTE) {
                NoteCounterData.CurrentNote++;
                NoteCounterData.Counter = 1;
            } else {
                NoteCounterData.Counter++;
            }
        }

        private bool CheckIfCostCenterIsSzakma(String costCenter) {
            if (String.Compare(costCenter, "L") < 0) {
                return true;
            }
            return false;
        }

        private PersonData SavePerson(PersonData person, String fileName, bool validateCostCenter) {            
            return ValidateCostCenter(person, fileName, validateCostCenter);
        }

        private PersonData ValidateCostCenter(PersonData person, String fileName, bool validateCostCenter) {
            if (validateCostCenter && (!Validator.CheckCostCenterFormat(person.CreditCostCenter, fileName, person) 
                || !Validator.CheckCostCenterFormat(person.DebitCostCenter, fileName, person))) {
                throw new ArgumentException($"Hibás pénzügyi központ formátum a következő fájlban: {FolderOperation.GetFileNameFromPath(fileName)} - {person.Name}");
            }
            return person;
        }

        private PersonData CreatePersonObject(int rowNumber, String month, String fileName) {
            String name = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["név"]);
            String credit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["terhelés"]);
            String debit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["számfejtés"]);
            String salary = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["bér"]);
            String tax = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["járulék"]);
            int note = 0;
            String miss = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["kihagyás"]);
            String type = CheckIfCostCenterIsSzakma(debit) ? BER_SZAKMA : BER_KOZPONTI;
            String taxId = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["adóazonosító"]);
            return new PersonData(name, month, credit, debit, salary, tax, note, fileName, type, miss, taxId);
        }

        private int FilterMonthAndSavePersonToList(String monthToFilter, int currentRow, int currentId, bool validateCostCenter) {
            String currentMonth = ExcelReadOperation.ReadExcelCell(currentRow, ColumnTitles["hónap"]);
            String currentMonthCleaned = GetCleanedMonth(currentMonth);
            if (monthToFilter.Equals(currentMonthCleaned)) {
                PersonData person = CreatePersonObject(currentRow, currentMonthCleaned, FileName);
                Validator.CheckIfNameIsPresent(person.Name, FileName);
                ValidateIfMissOrHasNoSalaryAndTaxValueAndAddToProperList(person, validateCostCenter);
                currentId++;
            }
            return currentId;
        }

        private void ValidateIfMissOrHasNoSalaryAndTaxValueAndAddToProperList(PersonData person, bool validateCostCenter) {
            if (!Validator.CheckIfSalaryAndTaxHasValue(person)) {
                person.Miss = ZERO_AMOUNT_TEXT;
            }
            if (Validator.CheckIfMissNotificationPresent(person.Miss)) {
                MissedPeople.Add(person);
            } else  {
                ProcessedPeople.Add(SavePerson(person, FileName, validateCostCenter));
            }
        }

        private String GetCleanedMonth(String monthInCell) {
            String result = String.Empty;
            String month = Validator.CheckIfWrittenMonthIsPresent(monthInCell);
            if (!String.IsNullOrEmpty(month)) {
                result = monthInCell.Substring(0, 5) + ConvertStringMonth(month);
            } else {
                result = ConvertNumericMonth(monthInCell);
            }
            return result; ;
        }

        private String ConvertNumericMonth(String monthInCell) {
            if (monthInCell.Length > 7) {
                StringBuilder sb = AppendCharactersToString(monthInCell);
                return sb.ToString().Substring(0, 7);
            } else {
                return monthInCell;
            }
        }

        private String ConvertStringMonth(String input) {
            switch (input) {
                default:
                case ("január"): return "01";
                case ("február"): return "02";
                case ("március"): return "03";
                case ("április"): return "04";
                case ("május"): return "05";
                case ("június"): return "06";
                case ("július"): return "07";
                case ("augusztus"): return "08";
                case ("szeptember"): return "09";
                case ("október"): return "10";
                case ("november"): return "11";
                case ("december"): return "12";
            }
        }

        private StringBuilder AppendCharactersToString(String monthInCell) {
            StringBuilder sb = new StringBuilder();
            foreach (Char actual in monthInCell.ToCharArray()) {
                if (actual != ' ') {
                    sb.Append(actual);
                }
            }
            return sb;
        }
    }
}
