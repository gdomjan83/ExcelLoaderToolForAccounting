using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class PersonCSVData {
        public String Note { get; set; }
        public String CreditDebitCode { get; set; }
        public String LedgerNumber { get; set; }
        public String Amount { get; set; }
        public String Comment { get; set; }
        public String CostCenter { get; set;}
        public String FunctionCode { get; set; }

        public String ProjectName { get; set; }

        public PersonCSVData(String note, String creditDebitCode, String ledgerNumber, String amount, String comment, String costCenter, String functionCode, String projectName) {
            Note = note;
            CreditDebitCode = creditDebitCode;
            LedgerNumber = ledgerNumber;
            Amount = amount;
            Comment = comment;
            CostCenter = costCenter;
            FunctionCode = functionCode;
            ProjectName = projectName;
        }
        public override string ToString() {
            return $"Okmány: {Note}, T_K Kód: {CreditDebitCode}, Főkönyvi szám: {LedgerNumber}, Összeg: {Amount}," +
                $" Szöveg: {Comment}, Pü központ: {CostCenter}, Funkc. terület: {FunctionCode}, Projekt név: {ProjectName}";
        }
    }
}
