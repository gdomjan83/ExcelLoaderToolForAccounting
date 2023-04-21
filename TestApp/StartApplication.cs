using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class StartApplication {

        public static void Main() {
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            String excelDirectory = System.IO.Path.Combine(currentDirectory, @"..\..\..\Resources");
            String fileLocation = System.IO.Path.Combine(currentDirectory, @"..\..\..\Resources\np_teszt.xlsx");
            String filepath = Path.GetFullPath(fileLocation);
            String[] files = Directory.GetFiles(excelDirectory);

            NoteCounterData noteCounterData = new NoteCounterData();
            //ExcelInputOutputOperations operations = new ExcelInputOutputOperations(filepath, "Bérköltség");
            //ExcelReadOperation readExcel = new ExcelReadOperation(operations);
            //String name = Path.GetFileName(filepath);
            //PersonDataConverter converter = new PersonDataConverter(readExcel, "2023.03", name);

            //List<PersonData> result = converter.SavePersonDataToList();
            //foreach (PersonData actual in result) {
            //    Console.WriteLine(actual);
            //}
            //Console.WriteLine();
            //List<PersonCSVData> csvResult = converter.ConvertPersonDataToCSVData(result, noteCounterData);
            //foreach (PersonCSVData actual in csvResult) {
            //    Console.WriteLine(actual);
            //}

            ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(files, noteCounterData, "2023.03", "Bérköltség");
            List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToCSVList();
            //foreach (PersonCSVData actual in csvResult) {
            //    Console.WriteLine(actual);
            //}
            excelFilesProcessor.WriteCSVFile("C:\\Users\\felhasználó\\Munka\\!PROJEKTEK\\SAP tananyag", csvResult);
        }
    }
}
