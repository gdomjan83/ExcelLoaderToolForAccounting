using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class PersonData {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Month { get; set; }
        public String Credit { get; set; }
        public String Debit { get; set; }
        public int Salary { get; set; }
        public int Tax { get; set; }
        public String Note { get; set; }

        public PersonData(String id, String name, String month, String credit, String debit, int salary, int tax, String note ) {
            this.Id = id;
            this.Name = name;
            this.Month = month;
            this.Credit = credit;
            this.Debit = debit;
            this.Salary = salary;
            this.Tax = tax;
            this.Note = note;
        }

        public override string ToString() {
            return $"Id: {Id}, Név: {Name}, Hónap: {Month}, Terhelés: {Credit}, Számfejtés: {Debit}, Bér: {Salary}, Járulék: {Tax}, Okmány: {Note}";
        }
    }
}
