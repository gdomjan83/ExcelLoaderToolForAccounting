using Microsoft.Office.Interop.Excel;

namespace TestApp {
    public class PersonDataConverter {
        public const String SALARY_LEDGER_NUMBER = "541100";
        public const String TAX_LEDGER_NUMBER = "561000";
        public const String CREDIT_CODE = "40";
        public const String DEBIT_CODE = "50";
        public const String BER_SZAKMA = "BérProjektSzakma";
        public const String BER_KOZPONTI = "BérProjektGMIFPI";
        public const int LINES_IN_ONE_NOTE = 80;
        public Dictionary<String, int> ColumnTitles { get; set; }
        public String AccountingDate { get; set; }
        public ExcelReadOperation ExcelReadOperation { get; set; }
        public List<PersonData> ProcessedPeople { get; set; } = new List<PersonData>();
        public String MonthToFilter { get; set; }
        public String FileName { get; set; }

        public PersonDataConverter(ExcelReadOperation excelReadOperation, String monthToFilter, String fileName, String accountingDate) {
            ExcelReadOperation = excelReadOperation;          
            MonthToFilter = monthToFilter;
            FileName = fileName;
            AccountingDate = accountingDate;
        }

        public List<PersonData> SavePersonDataToList() {
            try {
                Workbook wb = ExcelReadOperation.ExcelInputOutputOperations.WorkbookUsed;
                Worksheet ws = ExcelReadOperation.ExcelInputOutputOperations.WorkSheetUsed;
                ColumnTitles = ExcelRowColumnOperation.FindColumnTitles(ExcelReadOperation);
                int lastRow = ExcelRowColumnOperation.GetLastRow(wb, ws);
                int idNumber = 1;
                for (int i = 2; i <= lastRow; i++) {
                    idNumber = FilterMonthAndSavePersonToList(MonthToFilter, i, idNumber);
                }
            } finally {
                //ExcelReadOperation.ExcelInputOutputOperations.CloseApplication();
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

        public List<PersonData> ChangeNoteNumbers(List<PersonData> personDataCollection) {
            List<PersonData> result = new List<PersonData>(personDataCollection);
            int listSize = result.Count;
            for (int i = 0; i < listSize; i++) {
                UpdateNoteDataForPerson(result[i]);
                if ((i < listSize - 1) && (result[i].ProjectName != result[i + 1].ProjectName)) {
                    NoteCounterData.GmiFpiCounter = 1;
                    NoteCounterData.GmiSzakmaCounter = 1;
                    NoteCounterData.GmiFpiNoteDefault += 20;
                    NoteCounterData.GmiSzakmaNoteDefault += 20;
                    NoteCounterData.GmiFpiNote = NoteCounterData.GmiFpiNoteDefault;
                    NoteCounterData.GmiSzakmaNote = NoteCounterData.GmiSzakmaNoteDefault;
                }
            }
            return result;
        }        

        private void AddFejDataToList(PersonData personData, List<FejCSVData> fejDataResult) {
            FejCSVData newFejData = new FejCSVData(personData.Note, AccountingDate, personData.WorkerType, "V bér");
            if (!fejDataResult.Contains(newFejData)) {
                fejDataResult.Add(newFejData);
            }            
        }

        private void UpdateNoteDataForPerson(PersonData person) {
            if (CheckIfCostCenterIsSzakma(person.DebitCostCenter)) {
                person.Note = NoteCounterData.GmiSzakmaNote;
                ManageSzakmaCounter();
            } else {
                person.Note = NoteCounterData.GmiFpiNote;
                ManageFPICounter();
            }
        }

        private void ManageSzakmaCounter() {
            if (NoteCounterData.GmiSzakmaCounter == LINES_IN_ONE_NOTE) {
                NoteCounterData.GmiSzakmaNote++;
                NoteCounterData.GmiSzakmaCounter = 1;
            } else {
                NoteCounterData.GmiSzakmaCounter++;
            }
        }

        private void ManageFPICounter() {
            if (NoteCounterData.GmiFpiCounter == LINES_IN_ONE_NOTE) {
                NoteCounterData.GmiFpiNote++;
                NoteCounterData.GmiFpiCounter = 1;
            } else {
                NoteCounterData.GmiFpiCounter++;
            }
        }

        private bool CheckIfCostCenterIsSzakma(String costCenter) {
            if (String.Compare(costCenter, "L") < 0) {
                return true;
            }
            return false;
        }

        private PersonData SavePerson(int rowNumber, int id, String month, String fileName) {            
            return CreateNewPerson(rowNumber, id, month, fileName);
        }

        private PersonData CreateNewPerson(int rowNumber, int id, String month, String fileName) {
            PersonData person = CreatePersonObject(rowNumber, id, month, fileName);
            if (!Validator.CheckCostCenterFormat(person.CreditCostCenter, fileName) || !Validator.CheckCostCenterFormat(person.DebitCostCenter, fileName)) {
                throw new ArgumentException($"Hibás pénzügyi központ formátum a következő fájlban: {FolderOperation.GetFileNameFromPath(fileName)} - {person.Name}");
            }
            return person;
        }

        private PersonData CreatePersonObject(int rowNumber, int id, String month, String fileName) {
            String idNumber = id.ToString();
            String name = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Név"]);
            String credit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Terhelés"]);
            String debit = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Számfejtés"]);
            String salary = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Bér"]);
            String tax = ExcelReadOperation.ReadExcelCell(rowNumber, ColumnTitles["Járulék"]);
            int note = 0;
            String type = CheckIfCostCenterIsSzakma(debit) ? BER_SZAKMA : BER_KOZPONTI;
            return new PersonData(idNumber, name, month, credit, debit, salary, tax, note, fileName, type);
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
