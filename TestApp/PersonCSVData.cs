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
        public int Amount { get; set; }
        public String Comment { get; set; }
        public String CostCenter { get; set;}
        public String FunctionCode { get; set; }

        public PersonCSVData(string note, string creditDebitCode, string ledgerNumber, int amount, string comment, string costCenter, string functionCode) {
            Note = note;
            CreditDebitCode = creditDebitCode;
            LedgerNumber = ledgerNumber;
            Amount = amount;
            Comment = comment;
            CostCenter = costCenter;
            FunctionCode = functionCode;
        }
        public override string ToString() {
            return $"Okmány: {Note}, T_K Kód: {CreditDebitCode}, Főkönyvi szám: {LedgerNumber}, Összeg: {Amount}, Szöveg: {Comment}, Pü központ: {CostCenter}, Funkc. terület: {FunctionCode}";
        }
    }

}
