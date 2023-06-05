
namespace Berbetolto {
    public partial class HelpWindow : Form {
        public HelpWindow() {
            InitializeComponent();
            richTextBox1.AppendText("" +
                "Az alkalmazásból kétféle generálás végezhető:\n" +
                "1. Elkészíthetjük a FEJ és TET fájlokat az SAP vegyes betöltéshez.\n" +
                "2. Generálhatunk egy listát, ami azokat az adóazonosítókat tartalmazza, amikhez abban a hónapban bérkartonokat kell kérni.\n" +
                "\nA költségkövető fájlokból történő betöltéséhez szükséges feltételek:\n" +
                "- a költségkövető excel fájloknak különbözniük kell egymástól a nevükben.\n" +
                "- az excel fájlban a bérköltségeknek a következő elnevezésű munkalapon kell lennie: Bérköltség.\n" +
                "- a Bérköltség munkalap legelső sorában az egyes oszlopokhoz a következő elnevezéseket kell megadni: " +
                "Név, Hónap, Terhelés, Számfejtés, Bér, Járulék, Adóazonosító, Kihagyás. Más szöveg ne szerepeljen az első sorban!\n" +
                "- ha valamelyik sort nem szeretnénk könyvelni (például abban a sorban megbízási szerződés szerepel), akkor a Kihagyás oszlopba " +
                "írjuk be a kihagyás indokát (pl. megbízás, célfeladat)\n" +
                "- a hónapok oszlopban a hónapokat kétféle formátumban lehet megadni: a) 2023.03 vagy b) 2023. március\n" +
                "Az nem probléma, ha az előzőek után még további karakterek is szerepelnek." +
                " Például lehet így vezetni a töredék hónapokat: 2023.03.15-2023.03.30, vagy 2023. március 15-30.\n" +
                "\nA betöltés menete:\n" +
                "- kiválasztjuk, hogy milyen generálást kívánunk végezni,\n" +
                "- az első mezőbe beírjuk a könyvelni kívánt hónapot a következő formátumban: 2023.03\n" +
                "- a könyvelési dátum mezőbe beírjuk azt a dátumot, amelyik napra könyvelni szeretnénk a tételt. Formátum: 2023.04.26 " +
                "(Csak akkor tudjuk megadni, ha FEJ-TET fájlokat akarunk generálni).\n" +
                "- a Mai nap gombra kattintva rögtön megadhatjuk a mai napot könyvelési dátumnak. " +
                "(Nem elérhető a gomb, ha adóazonosító listát generálunk).\n" +
                "- a Tallózás gombra kattintva egyesével beállítjuk, hogy melyik költségkövető fájlokat kívánjuk használni. Egyszerre " +
                "több fájlból is tölthetünk be adatokat.\n" +
                "- a Tallózás gomb használata helyett megtehetjük azt is, hogy az egyes fájlokat behúzzuk az ablakba.\n" +
                "- a Legutóbbi fájlok használata gombbal gyorsan betölthetjük azokat a táblázatokat, amelyeket legutóbb használtunk. " +
                "Így nem kell egyesével újra betallózni őket.\n" +
                "- a Létrehozás gombra kattintva elvégezzük a generálást.\n" +
                "- az eredményül kapott fájlok megtalálhatók az alkalmazás mappájában, az Eredmény nevű almappában.\n" +
                "- a Visszaállítás gombbal törölni lehet az aktuális munkamenet adatait és újrakezdeni a betöltést.\n" +
                "\nAz alkalmazás a következő ellenőrzéseket végzi a futtatáskor:\n" +
                "- Megfelelően adtuk-e meg a leszűrni kívánt hónapot (helyes formátum: 2023.03).\n" +
                "- Megfelelően adtuk-e meg a könyvelési napot (helyes formátum: 2023.04.26).\n" +
                "- A betallózott fájlok megtalálhatók-e a helyükön.\n" +
                "- Az egyes táblázatokban az első sorban kizárólag az a nyolc elnevezés szerepel, amelyekkel beazonosíthatók a használt oszlopok.\n" +
                "- Minden sorban megtalálható a terhelés és a számfejtés pénzügyi központja a megfelelő formátumban (helyes formátum: M217000062).\n" +
                "- Minden sorban szerepel-e az adott dolgozó adóazonosító jele.\n" +
                "- A bér és járulék cellákban nem szerepel-e szöveg.\n" +
                "- Összeadja az egyes projektekben könyvelt összegeket. Jelzi, ha ez nem egész szám. Ez arra utal, hogy valamelyik cellában tizedesjegyek " +
                "szerepelnek. Az SAP betöltés előtt ezt javítani szükséges.");
            richTextBox1.SelectionStart = 0;
        }
    }
}
