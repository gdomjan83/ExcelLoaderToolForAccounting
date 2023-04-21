using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public static class FolderOperation {

        public static String[] FindFilesInSourceDirectory(String sourcePath) {
            return Directory.GetFiles(sourcePath);
        }

        public static String CreateNewFile(String filePath) {
            String currentFileName = AppendTimeToFilepath(filePath);
            using (FileStream fs = File.Create(currentFileName)) {
                fs.Close();
            }
            return currentFileName;
        }

        private static String AppendTimeToFilepath(String filePath) {
            DateTime now = DateTime.Now;
            String time = now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
            return filePath + "\\TET_" + time + ".csv";
        }
    }
}
