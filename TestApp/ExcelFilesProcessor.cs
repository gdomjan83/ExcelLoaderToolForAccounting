

namespace TestApp {
    public class ExcelFilesProcessor {
        public String[] FilePaths { get; set; }
        public ExcelInputOutputOperations ExcelInputOutputOperations { get; set; }
        public ExcelReadOperation ExcelReadOperation { get; set; }
        public PersonDataConverter PersonDataConverter { get; set; }
        public NoteCounterData NoteCounterData { get; set; }
        public String MonthToFilter { get; set; }
        public String WorkSheetName { get; set; } = "Bérköltség";

        public ExcelFilesProcessor(NoteCounterData noteCounterData, String monthToFilter) {
            NoteCounterData = noteCounterData;
            MonthToFilter = monthToFilter;
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

        public List<PersonCSVData> TransformCompletePersonDataListToCSVList() {
            List<PersonData> inputList = CreateCompleteListFromPersonDataInAllFiles();
            List<PersonCSVData> result = PersonDataConverter.ConvertPersonDataToCSVData(inputList, NoteCounterData);
            return result;
        }

        public void WriteCSVFile(String targetPath, List<PersonCSVData> personData) {
            ExcelInputOutputOperations.WriteListToCSVFile(targetPath, personData);
        }

        private PersonDataConverter CreateExcelObjectsForFileReading(String filepath, String monthToFilter, String workSheetName, String fileName) {
            ExcelInputOutputOperations = new ExcelInputOutputOperations(filepath, workSheetName);
            ExcelReadOperation = new ExcelReadOperation(ExcelInputOutputOperations);
            return new PersonDataConverter(ExcelReadOperation, monthToFilter, fileName);
        }
    }
}
