﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class FejCSVData {
        public String Note { get; set; }
        public String Source { get; set; }
        public String AccountingType { get; set; }
        public String AccountingDate { get; set; }
        public String Period { get; set; }
        public String Type { get; set; }
        public String SerialNumber { get; set; }
        public String Currency { get; set; }

        public FejCSVData(String note, String accountingDate, String type, String serialNumber) {
            Note = note;
            Source = "1000";
            AccountingType = "SU";
            AccountingDate = accountingDate;
            Period = GetPeriod(AccountingDate);
            Type = type;
            SerialNumber = serialNumber;
            Currency = "HUF";
        }

        public override bool Equals(object? obj) {
            return obj is FejCSVData data &&
                   Note == data.Note;
        }

        public  String CSVFormatting() {
            return $"{Note};{Source};{AccountingType};{AccountingDate};{AccountingDate};{Period};;;{Type};{SerialNumber};{Currency}";
        }

        private String GetPeriod(String date) {
            if ("1".Equals(date[4])) {
                return date.Substring(4, 2);
            }
            return date[5].ToString();
        }

        public override int GetHashCode() {
            return HashCode.Combine(Note);
        }
    }
}
