
namespace TestApp {
    public class StartApplication {

        public static void Main() {
            NoteCounterData noteCounterData = new NoteCounterData();          
            UIController uiController = new UIController(noteCounterData);
            uiController.RunApplication();
        }
    }
}
