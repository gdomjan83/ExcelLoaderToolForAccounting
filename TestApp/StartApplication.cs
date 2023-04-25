

namespace TestApp {
    public class StartApplication : Form {
        
        [STAThreadAttribute]
        public static void Main() {
            //konzolos verzió elérhető a main branch-ről.
            //Ez a verzió a windows formon keresztül működik, grafikus kezelőfelülettel.

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindowForm());
        }
    }
}
