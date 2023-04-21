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
            String[] files = Directory.GetFiles(excelDirectory);
            String targetDirectory = Path.Combine(currentDirectory, @"..\..\..\Result");

            Console.WriteLine("Melyik hónapot szeretnéd könyvelni (formátum -> 2023.03):");
            String month = Console.ReadLine();       

            NoteCounterData noteCounterData = new NoteCounterData();          
            ExcelFilesProcessor excelFilesProcessor = new ExcelFilesProcessor(files, noteCounterData, month);
            List<PersonCSVData> csvResult = excelFilesProcessor.TransformCompletePersonDataListToCSVList();
            excelFilesProcessor.WriteCSVFile(targetDirectory, csvResult);
            UIController uiController = new UIController(excelDirectory, targetDirectory);
            uiController.FinishTask();
        }
    }
}
