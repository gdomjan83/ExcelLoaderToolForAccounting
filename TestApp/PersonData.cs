
namespace TestApp {
    public class PersonData {
        public String Name { get; set; }
        public String Month { get; set; }
        public String CreditCostCenter { get; set; }
        public String DebitCostCenter { get; set; }
        public String Salary { get; set; }
        public String Tax { get; set; }
        public int Note { get; set; }
        public String ProjectName { get; set; }
        public String WorkerType { get; set; }
        public String Miss { get; set; }

        public PersonData(String name, String month, String credit, String debit, String salary, String tax, int note , String projectName, String workerType, String miss) {
            Name = name;
            Month = month;
            CreditCostCenter = credit;
            DebitCostCenter = debit;
            Salary = salary;
            Tax = tax;
            Note = note;
            ProjectName = projectName;
            WorkerType = workerType;
            Miss = miss;
        }

        public override string ToString() {
            return $"Név: {Name}, Hónap: {Month}, Terhelés: {CreditCostCenter}," +
                $" Számfejtés: {DebitCostCenter}, Bér: {Salary}, Járulék: {Tax}, Okmány: {Note}, Projekt név: {ProjectName}, Kihagyás: {Miss}";
        }
    }
}
