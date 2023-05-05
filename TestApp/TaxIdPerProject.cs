using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berbetolto {
    public class TaxIdPerProject {

        public String TaxId { get; set; }
        public String ProjectName { get; set; }

        public TaxIdPerProject(String taxId, String projectName) {
            TaxId = taxId;
            ProjectName = projectName;
        }

        public String CSVFormating() {
            return $"{TaxId};{ProjectName}";
        }

        public override bool Equals(object? obj) {
            return obj is TaxIdPerProject project &&
                   TaxId == project.TaxId &&
                   ProjectName == project.ProjectName;
        }

        public override int GetHashCode() {
            return HashCode.Combine(TaxId, ProjectName);
        }
    }
}
