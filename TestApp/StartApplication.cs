using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TestApp {
    public class StartApplication {

        public static void Main() {
            String currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            String excelDirectory = Path.Combine(currentDirectory, @"..\..\..\Resources");
            String fileLocation = Path.Combine(currentDirectory, @"..\..\..\Resources\np_teszt.xlsx");
            String[] files = Directory.GetFiles(excelDirectory);
            String targetDirectory = "C:\\Users\\felhasználó\\Munka\\!PROJEKTEK\\SAP tananyag";

            NoteCounterData noteCounterData = new NoteCounterData();          
            ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(files, noteCounterData, "2023.03", "Bérköltség");
            List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToCSVList();
            excelFilesProcessor.WriteCSVFile(targetDirectory, csvResult);
            UIController uiController = new UIController(excelDirectory, targetDirectory);
            uiController.FinishTask();
        }
    }
}
