
namespace TestApp {
    public static class FolderOperation {

        public static String[] FindFilesInSourceDirectory(String sourcePath) {
            return Directory.GetFiles(sourcePath);
        }

        public static String CreateNewFile(String filePath) {
            using (FileStream fs = File.Create(filePath)) {
                fs.Close();
            }
            return filePath;
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
        public static String GetFileNameFromPath(String path) {
            int lastIndexOfBackslash = path.LastIndexOf("\\");
            return path.Substring(lastIndexOfBackslash + 1);
        }

        public static String CopyFile(String sourceFile, String destination) {
            String targetFile = Path.Combine(destination, GetFileNameFromPath(sourceFile));
            File.Copy(sourceFile, targetFile, true);
            return targetFile;
        }
    }
}
