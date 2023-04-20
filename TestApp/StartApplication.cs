using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class StartApplication {

        public static void Main() {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fileLocation = System.IO.Path.Combine(currentDirectory, @"..\..\..\Resources\rrf_teszt.xlsx");
            string filepath = Path.GetFullPath(fileLocation);

            ExcelInputOutputOperations operations = new ExcelInputOutputOperations(filepath, "Bérköltség");
            ExcelReadOperation readExcel = new ExcelReadOperation(operations);
            PersonDataConverter converter = new PersonDataConverter(readExcel);

            List<PersonData> result = converter.SavePersonDataToList("2023.03");
            foreach (PersonData actual in result) {
                Console.WriteLine(actual);
            }
        }
    }
}
