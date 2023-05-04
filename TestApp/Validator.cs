using System.Text.RegularExpressions;

namespace TestApp {
    public static class Validator {
        private const string MISSING_COST_CENTER_TEXT = "Nem található pénzügyi központ. Fájl: ";
        private const string MISSING_TAXID_TEXT = "Nem található megfelelő adóazonosító az egyik személynél. Fájl: ";

        public static bool CheckIfAmountIsZero(String amount) {
            if (String.IsNullOrEmpty(amount) || Double.Parse(amount) == 0d) {
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

        public static bool CheckCostCenterFormat(String costCenter, String fileName) {
            String regex = "^[A-Z]\\d{9}$";
            Match matcher = Regex.Match(costCenter, regex);
            if (String.IsNullOrEmpty(costCenter)) {
                throw new ArgumentException(MISSING_COST_CENTER_TEXT + FolderOperation.GetFileNameFromPath(fileName));
            }
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

        public static bool CheckIfAccountingDateInCorrectForm(String date) {
            String regex = "^\\d{4}\\.\\d{2}\\.\\d{2}$";
            Match matcher = Regex.Match(date, regex);
            if (matcher.Success) {
                return true;
            }
            return false;
        }

        public static bool CheckIfAllLabelsFound(String[] labels,  Dictionary<String, int> columnTitles) {
            List<String> keys = new List<String>(columnTitles.Keys);
            if (keys.Count == labels.Length) {
                return true;
            }
            return false;
        }

        public static bool CheckIfMissNotificationPresent(String cellValue) {
            return !String.IsNullOrEmpty(cellValue);
        }

        public static bool CheckIfTaxIDPresent(String taxId, String fileName) {
            String regex = "^\\d{10}$";
            Match matcher = Regex.Match(taxId, regex);
            if (matcher.Success) {
                return true;
            }
            throw new ArgumentException(MISSING_TAXID_TEXT + FolderOperation.GetFileNameFromPath(fileName));
        }
    }
}
