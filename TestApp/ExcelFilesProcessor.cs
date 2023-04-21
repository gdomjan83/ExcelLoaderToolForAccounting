using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class ExcelFilesProcessor {
        public String[] FilePaths { get; set; }
        public ExcelInputOutputOperations Operations { get; set; }
        public ExcelReadOperation ReadExcel { get; set; }
        public PersonDataConverter Converter { get; set; }
        public NoteCounterData NoteCounterData { get; set; }
        public String MonthToFilter { get; set; }
        public String WorkSheetName { get; set; }

        public ExcelFilesProcessor(String[] filePaths, NoteCounterData noteCounterData, String monthToFilter, String workSheetName) {
            FilePaths = filePaths;
            NoteCounterData = noteCounterData;
            MonthToFilter = monthToFilter;
            WorkSheetName = workSheetName;
        }

        public List<PersonData> CreateCompleteListFromPersonDataInAllFiles() {
            List<PersonData> result = new List<PersonData>();
            foreach(String path in FilePaths) {
                String fileName = Path.GetFileName(path);
                Converter = CreateExcelObjectsForFileReading(path, MonthToFilter, WorkSheetName, fileName);
                result.AddRange(Converter.SavePersonDataToList());
            }
            return result;
        }

        public List<PersonCSVData> TransformCompletePersonDataListToCSVList() {
            List<PersonData> inputList = CreateCompleteListFromPersonDataInAllFiles();
            List<PersonCSVData> result = Converter.ConvertPersonDataToCSVData(inputList, NoteCounterData);
            return result;
        }

        private PersonDataConverter CreateExcelObjectsForFileReading(String filepath, String monthToFilter, String workSheetName, String fileName) {
            Operations = new ExcelInputOutputOperations(filepath, workSheetName);
            ReadExcel = new ExcelReadOperation(Operations);
            return new PersonDataConverter(ReadExcel, monthToFilter, fileName);
        }
    }
}
