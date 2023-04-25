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

        public static List<String> CopySeveralFiles(List<String> sourceFiles, String destinationFolder) {
            List<String> copiedFiles = new List<String>();
            foreach(String actual in sourceFiles) {
                String targetFile = CopyFile(actual, destinationFolder);
                copiedFiles.Add(targetFile);
            }
            return copiedFiles;
        }

        public static void DeleteFiles(String folder) {
            String[] files = Directory.GetFiles(folder);
            foreach(String actual in files) {
                File.Delete(actual);
            }
        }

        private static String CopyFile(String sourceFile, String destination) {
            String targetFile = Path.Combine(destination, GetFileNameFromPath(sourceFile));
            File.Copy(sourceFile, targetFile, true);
            return targetFile;
        }
        public static String GetFileNameFromPath(String path) {
            int lastIndexOfBackslash = path.LastIndexOf("\\");
            return path.Substring(lastIndexOfBackslash + 1);
        }

        private static String AppendTimeToFilepath(String filePath) {
            String time = GetCurrentDatetime();
            return filePath + "\\TET_" + time + ".csv";
        }

        private static String GetCurrentDatetime() {
            DateTime now = DateTime.Now;
            String time = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() +
                now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
            return time;
        }
    }
}
