
using Berbetolto;

namespace TestApp {
    public class ExcelFilesProcessor {
        public String[] FilePaths { get; set; }
        public ExcelInputOutputOperations ExcelInputOutputOperations { get; set; }
        public ExcelReadOperation ExcelReadOperation { get; set; }
        public PersonDataConverter PersonDataConverter { get; set; }
        public String MonthToFilter { get; set; }
        public String WorkSheetName { get; set; } = "Bérköltség";
        public String AccountingDate { get; set; }

        public ExcelFilesProcessor(String monthToFilter, String accountingDate) {
            MonthToFilter = monthToFilter;
            AccountingDate = accountingDate;
        }

        public List<PersonData> CreateCompleteListFromPersonDataInAllFiles() {
            List<PersonData> result = new List<PersonData>();
            foreach(String path in FilePaths) {
                String fileName = Path.GetFileName(path);
                PersonDataConverter = CreateExcelObjectsForFileReading(path, MonthToFilter, WorkSheetName, fileName);
                result.AddRange(PersonDataConverter.SavePersonDataToList());
            }
            return result;
        }

        public List<PersonCSVData> TransformCompletePersonDataListToTETCSVList(List<PersonData> personInputList) {
            return PersonDataConverter.ConvertPersonDataToTETCSVData(personInputList);
        }

        public  List<FejCSVData> TransformCompletePersonDataListToFEJCSVList(List<PersonData> personInputList) {
            return PersonDataConverter.ConvertPersonDataToFejCSVData(personInputList);
        }

        public void WriteCSVFile(String targetPath, List<PersonCSVData> personData) {
            var t = from tet in personData select tet.CSVFormating();
            ExcelInputOutputOperations.WriteListToCSVFile(targetPath, t.ToList<String>());
        }

        public void WriteCSVFile(String targetPath, List<FejCSVData> fejData) {
            var f = from fej in fejData select fej.CSVFormatting();
            ExcelInputOutputOperations.WriteListToCSVFile(targetPath, f.ToList<String>());
        }

        private PersonDataConverter CreateExcelObjectsForFileReading(String filepath, String monthToFilter, String workSheetName, String fileName) {
            ExcelInputOutputOperations = new ExcelInputOutputOperations(filepath, workSheetName);
            ExcelReadOperation = new ExcelReadOperation(ExcelInputOutputOperations);
            return new PersonDataConverter(ExcelReadOperation, monthToFilter, fileName, AccountingDate);
        }

    }
}
