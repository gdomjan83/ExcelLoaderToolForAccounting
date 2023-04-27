
namespace TestApp {
    public static class NoteCounterData {
        public static int GmiSzakmaCounter { get; set; } = 1;
        public static int GmiFpiCounter { get; set; } = 1;
        public static int GmiSzakmaNote { get; set; } = 10;
        public static int GmiFpiNote { get; set; } = 20;
        public static int GmiSzakmaNoteDefault { get; set; } = 10;
        public static int GmiFpiNoteDefault { get; set; } = 20;

        public static void ResetProperties() {
            GmiSzakmaCounter = 1;
            GmiFpiCounter = 1;
            GmiSzakmaNote = 10;
            GmiFpiNote = 20;
            GmiSzakmaNoteDefault = 10;
            GmiFpiNoteDefault = 20;
        }
    }
}
