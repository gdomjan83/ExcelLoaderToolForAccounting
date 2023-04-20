using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class StartApplication {

        public static void Main() {
            ExcelInputOutputOperations operations = new ExcelInputOutputOperations("C:\\Users\\felhasználó\\Desktop\\np_teszt.xlsx", "Bérköltség");
            ExcelReadOperation readExcel = new ExcelReadOperation(operations);
            PersonDataConverter converter = new PersonDataConverter(readExcel);

            //Console.WriteLine(readExcel.ReadExcelCell(1, 2));
            //foreach (String actual in op.ReadExcelRange("A1:A3")) {
            //    Console.WriteLine(actual);
            //}

            //Console.WriteLine(ExcelRowColumnCounter.GetLastRow(operations.OpenWorkbook(), operations.WorkSheetUsed));
            //operations.CloseApplication();
            //Console.WriteLine(ExcelRowColumnCounter.GetLastColumn(operations.OpenWorkbook(), operations.WorkSheetUsed));
            //operations.CloseApplication();

            converter.FindColumnTitles();
            List<PersonData> result = converter.SavePersonDataToList("2023.02");
            foreach (PersonData actual in result) {
                Console.WriteLine(actual);
            }
            //int a = 1548;
            //String b = a.ToString();
            //String c = "1200";
            //String d = c.ToString();
            //Console.WriteLine(Int32.Parse(d));
        }
    }
}
