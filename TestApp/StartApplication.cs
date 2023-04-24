using System.Windows.Forms;

namespace TestApp {
    public class StartApplication : Form {
        
        [STAThreadAttribute]
        public static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindowForm());


            //UIController uiController = new UIController();
            //uiController.RunApplication();
        }
    }
}
