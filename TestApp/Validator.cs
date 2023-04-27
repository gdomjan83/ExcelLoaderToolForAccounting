using System.Text.RegularExpressions;

namespace TestApp {
    public static class Validator {

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
                throw new ArgumentException($"Nem található pénzügyi központ. Fájl: {FolderOperation.GetFileNameFromPath(fileName)}");
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

        public static bool CheckIfAllLabelsFound(Dictionary<String, int> columnTitles) {
            List<String> keys = new List<String>(columnTitles.Keys);
            if (keys.Count == 6) {
                return true;
            }
            return false;
        }

    }
}
