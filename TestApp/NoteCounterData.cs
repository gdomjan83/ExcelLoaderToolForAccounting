
namespace TestApp {
    public static class NoteCounterData {
        public static int Counter { get; set; } = 1;
        public static int CurrentNote { get; set; } = 10;

        public static void ResetProperties() {
            Counter = 1;
            CurrentNote = 10;
        }
    }
}
