using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Berbetolto {
    public static class Validator {

        public static bool CheckIfAmountIsZero(String amount) {
            if (String.IsNullOrEmpty(amount) || Int32.Parse(amount) == 0) {
                return true;
            }
            return false;
        }

        public static bool CheckIfMonthInCorrectForm(String month) {
            String regex = "^\\d{4}\\.\\d{2}$";
            Match matcher = Regex.Match(month, regex);
            if (matcher.Success) {
                return true;
            }
            return false;
        }

        public static bool CheckIfFilesPresentInDirectory(String[] files) {
            if (files.Length != 0) {
                return true;
            }
            return false;
        }

        public static bool CheckCostCenterFormat(String costCenter) {
            String regex = "^[A-Z]\\d{9}$";
            Match matcher = Regex.Match(costCenter, regex);
            if (matcher.Success) {
                return true;
            }
            return false;
        }

        public static bool CheckCSVFileName(String fileName) {
            String regex = "^TET\\d{14}F$";
            Match matcher = Regex.Match(fileName, regex);
            if (matcher.Success) {
                return true;
            }
            return false;
        }
    }
}
