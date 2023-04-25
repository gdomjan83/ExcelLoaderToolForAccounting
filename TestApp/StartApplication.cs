using System.Windows.Forms;

namespace TestApp {
    public class StartApplication : Form {
        
        [STAThreadAttribute]
        public static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindowForm());

            //konzolos verzióhoz kell ez a rész. Ahhoz törölni kell [STAThreadAttribute] annotációt, és a Form öröklődést, és átállítani a project properties-ben Console applikációra.
            //UIController uiController = new UIController(UIController.UIVersion.Console);
            //uiController.RunApplication();
        }
    }
}
