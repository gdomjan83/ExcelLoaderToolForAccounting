
namespace Berbetolto {
    public partial class HelpWindow : Form {
        public HelpWindow() {
            InitializeComponent();
            richTextBox1.AppendText("" +
                "Az excelből történő béradat betöltéséhez szükséges feltételek:\n" +
                "- a költségkövető excel fájloknak különbözniük kell egymástól a nevükben.\n" +
                "- az excel fájlban a bérköltségeknek a következő elnevezésű munkalapon kell lennie: Bérköltség.\n" +
                "- a Bérköltség munkalap legelső sorában az egyes oszlopokhoz a következő elnevezéseket kell megadni: " +
                "Név, Hónap, Terhelés, Számfejtés, Bér, Járulék, Kihagyás.\n" +
                "- ha valamelyik sort nem szeretnénk könyvelni (például abban a sorban megbízási szerződés szerepel), akkor a Kihagyás oszlopba írjunk valami jelzést (a fontos, hogy ne legyen üres).\n" +
                "- a hónapok oszlopban a hónapokat úgy kell feltűntetni, hogy az első hét karakter ilyen formában legyen: 2023.03\n" +
                "Az nem probléma, ha utána mást is írunk. Például lehet így vezetni a tört hónapokat: 2023.03.15-2023.03.30.\n" +
                "- a betöltés során ne legyenek nyitva a költségkövető excel fájlok.\n" +
                "\nA betöltés menete:\n" +
                "- az első mezőbe beírjuk a könyvelni kívánt hónapot a következő formátumban: 2023.03\n" +
                "- a könyvelési dátum mezőbe beírjuk azt a dátumot, amelyik napra könyvelni szeretnénk a tételt. Formátum: 2023.04.26\n" +
                "- a Mai nap gombra kattintva rögtön megadhatjuk a mai napot könyvelési dátumnak.\n" +
                "- a tallózás gombra kattintva egyesével beállítjuk, hogy melyik költségkövető fájlokat kívánjuk használni. Egyszerre " +
                "több fájl is könyvelhető.\n" +
                "- a Legutóbbi fájlok használata gombbal gyorsan betölthetjük azokat a táblázatokat, amiket legutóbb használtuk. " +
                "gombra kattintva gyorsan behívhatók, és nem kell újra betallózni őket.\n" +
                "- a Végrehajtás gombra kattintva elvégezzük a generálást. A folyamat végén megnyílik az eredmény fájlokat tartalmazó mappa.\n" +
                "- a Visszaállítás gombbal törölni lehet az aktuális munkamenet adatait és újrakezdeni a betöltést.");
        }
    }
}
