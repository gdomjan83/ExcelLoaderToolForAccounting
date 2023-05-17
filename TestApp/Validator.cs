using System.Text.RegularExpressions;

namespace TestApp {
    public static class Validator {
        private const string MISSING_COST_CENTER_TEXT = "Nem található pénzügyi központ. Fájl: ";
        private const string MISSING_NAME_TEXT = "Nem található név az egyik sorban. Fájl: ";
        private const string MISSING_TAXID_TEXT = "Nem található megfelelő adóazonosító az egyik személynél. Fájl: ";
        private static String[] months = {"január", "február", "március", "április", "május", "június", "július", "augusztus",
        "szeptember", "október", "november", "december"};

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

        public static bool CheckCostCenterFormat(String costCenter, String fileName, PersonData person) {
            String regex = "^[A-Z]\\d{9}$";
            Match matcher = Regex.Match(costCenter, regex);
            if (String.IsNullOrEmpty(costCenter)) {
                throw new ArgumentException($"{MISSING_COST_CENTER_TEXT} {FolderOperation.GetFileNameFromPath(fileName)} - {person.Name}");
            }
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

        public static List<String> CheckWhichLabelsAreMissing(String[] labels, Dictionary<String, int> columnTitles) {
            List<String> missingLabels = new List<String>();
            List<String> keys = new List<String>(columnTitles.Keys);
            foreach(String actual in labels) {
                if (!keys.Contains(actual)) {
                    missingLabels.Add(actual);
                }
            }
            return missingLabels;
        }

        public static bool CheckIfMissNotificationPresent(String cellValue) {
            return !String.IsNullOrEmpty(cellValue);
        }

        public static bool CheckIfTaxIDPresent(PersonData person) {
            String regex = "^\\d{10}$";
            Match matcher = Regex.Match(person.TaxId, regex);
            if (matcher.Success) {
                return true;
            }
            throw new ArgumentException($"{MISSING_TAXID_TEXT} {person.ProjectName} - {person.Name}");
        }

        public static String CheckIfWrittenMonthIsPresent(String input) {
            foreach(String actual in months) {
                if (input.ToLower().Contains(actual)) {
                    return actual;
                }
            }
            return String.Empty;
        }

        public static bool CheckIfNameIsPresent(String cellValue, String fileName) {
            if (String.IsNullOrEmpty(cellValue)) {
                throw new ArgumentException(MISSING_NAME_TEXT + FolderOperation.GetFileNameFromPath(fileName));
            }
            return true;
        }
    }
}
