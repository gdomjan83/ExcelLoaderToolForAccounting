
namespace TestApp {
    public static class NoteCounterData {
        public static int SzakmaCounter { get; set; } = 1;
        public static int GmiFpiCounter { get; set; } = 1;
        public static int SzakmaNote { get; set; } = 100;
        public static int GmiFpiNote { get; set; } = 101;
        public static int SzakmaNoteDefault { get; set; } = 100;
        public static int GmiFpiNoteDefault { get; set; } = 101;

        public static void ResetProperties() {
            SzakmaCounter = 1;
            GmiFpiCounter = 1;
            SzakmaNote = 100;
            GmiFpiNote = 101;
        }

        public static void UpdateNoteForNewProject() {
            SzakmaNoteDefault += 10;
            GmiFpiNoteDefault += 10;
            SzakmaNote = SzakmaNoteDefault;
            GmiFpiNote = GmiFpiNoteDefault;
            SzakmaCounter = 1;
            GmiFpiCounter = 1;
        }

        public static void IncreaseSzakmaNoteByOne() {
            do {
                SzakmaNote++;
            } while (SzakmaNote <= GmiFpiNote);
            SzakmaCounter = 1;
        }

        public static void IncreaseGmiFpiNoteByOne() {
            do {
                GmiFpiNote++;
            } while (GmiFpiNote <= SzakmaNote);
            GmiFpiCounter = 1;
        }
    }
}
