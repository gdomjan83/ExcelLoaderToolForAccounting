
using System.Collections;

namespace TestApp {
    public class PersonCSVData {
        public int Note { get; set; }
        public String CreditDebitCode { get; set; }
        public String LedgerNumber { get; set; }
        public String Amount { get; set; }
        public String Comment { get; set; }
        public String CostCenter { get; set;}
        public String FunctionCode { get; set; }
        public String ProjectName { get; set; }

        public PersonCSVData(int note, String creditDebitCode, String ledgerNumber, String amount, String comment, String costCenter, String functionCode, String projectName) {
            Note = note;
            CreditDebitCode = creditDebitCode;
            LedgerNumber = ledgerNumber;
            Amount = amount;
            Comment = comment;
            CostCenter = costCenter;
            FunctionCode = functionCode;
            ProjectName = projectName;
        }

        public String CSVFormating() {
            return $"{Note};{CreditDebitCode};{LedgerNumber};{LedgerNumber};;{Amount};;;{Comment};{CostCenter};{FunctionCode};;{ProjectName}";
        }
        public override String ToString() {
            return $"Okmány: {Note}, T_K Kód: {CreditDebitCode}, Főkönyvi szám: {LedgerNumber}, Összeg: {Amount}," +
                $" Szöveg: {Comment}, Pü központ: {CostCenter}, Funkc. terület: {FunctionCode}, Projekt név: {ProjectName}";
        }

        //public static IComparer SortBasedOnNotes() {
        //    return (IComparer)new SortBasedOnNotesHelper();
        //}

        //private class SortBasedOnNotesHelper : IComparer {
        //    int IComparer.Compare(object a, object b) {
        //        PersonCSVData obj = (PersonCSVData)a;
        //        PersonCSVData otherObj = (PersonCSVData)b;

        //        if (obj.Note > otherObj.Note) {
        //            return 1;
        //        }
        //        if (obj.Note < otherObj.Note) {
        //            return -1;
        //        }
        //        return 0;
        //    }
        //}
    }
}
