using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class UIController {
        public String SourceFilesFolder { get; set; }
        public String TargetFilesFolder { get; set; }

        private const String GOODBYE_TEXT = "Nyomd meg az Entert a végeredmény megtekintéséhez.";

        public UIController(String sourceFilesFolder, String targetFilesFolder) {
            SourceFilesFolder = sourceFilesFolder;
            TargetFilesFolder = targetFilesFolder;
        }

        public void FinishTask() {
            Console.WriteLine("Nyomj Entert a végeredmény megtekintéséhez.");
            Console.ReadLine();
            Process.Start("explorer.exe", TargetFilesFolder);
        }
    }
}
