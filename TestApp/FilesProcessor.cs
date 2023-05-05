
using Berbetolto;

namespace TestApp {
    public class FilesProcessor {
        public String[] FilePaths { get; set; }
        public FileInputOutputOperations FileInputOutputOperations { get; set; }
        public ExcelReadOperation ExcelReadOperation { get; set; }
        public PersonDataConverter PersonDataConverter { get; set; }
        public String MonthToFilter { get; set; }
        public String WorkSheetName { get; set; } = "Bérköltség";
        public String AccountingDate { get; set; }

        public FilesProcessor() {
            FileInputOutputOperations = new FileInputOutputOperations();
        }

        public List<PersonData> CreateCompleteListFromPersonDataInAllFiles(bool validateCostCenter) {
            List<PersonData> result = new List<PersonData>();
            foreach (String path in FilePaths) {
                try {
                    String fileName = Path.GetFileName(path);
                    PersonDataConverter = CreateExcelObjectsForFileReading(path, MonthToFilter, WorkSheetName, fileName);
                    result.AddRange(PersonDataConverter.SavePersonDataToList(validateCostCenter));
                } finally {
                    ExcelReadOperation.ExcelInputOutputOperations.CloseApplication();
                }
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
            FileInputOutputOperations.WriteListToCSVFile(targetPath, t.ToList<String>());
        }

        public void WriteCSVFile(String targetPath, List<TaxIdPerProject> personData) {
            var t = from tax in personData select tax.CSVFormating();
            FileInputOutputOperations.WriteListToCSVFile(targetPath, t.ToList<String>());
        }

        public void WriteCSVFile(String targetPath, List<FejCSVData> fejData) {
            var f = from fej in fejData select fej.CSVFormatting();
            FileInputOutputOperations.WriteListToCSVFile(targetPath, f.ToList<String>());
        }

        public void WriteTXTFile(String targetPath, List<String> filePaths) {
            FileInputOutputOperations.WriteTXTFile(targetPath, filePaths.ToArray());
        }

        public String[] ReadTXTFile(String targetPath) {
            return FileInputOutputOperations.OpenTXTFile(targetPath);
        }

        private PersonDataConverter CreateExcelObjectsForFileReading(String filePath, String monthToFilter, String workSheetName, String fileName) {
            FileInputOutputOperations.OpenExcelFileAndWorkBook(filePath, workSheetName);
            ExcelReadOperation = new ExcelReadOperation(FileInputOutputOperations);
            return new PersonDataConverter(ExcelReadOperation, monthToFilter, fileName, AccountingDate);
        }

    }
}
