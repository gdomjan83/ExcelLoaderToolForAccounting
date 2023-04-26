﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Berbetolto {
    public partial class HelpWindow : Form {
        public HelpWindow() {
            InitializeComponent();
            richTextBox1.AppendText("" +
                "Az excelből történő béradat betöltéséhez szükséges feltételek:\n" +
                "- a költségkövető excel fájloknak különbözniük kell egymástól a nevükben.\n" +
                "- az excel fájlban a bérköltségeknek a következő elnevezésű munkalapon kell lennie: Bérköltség\n" +
                "- a Bérköltség munkalap legelső sorában az egyes oszlopokhoz a következő elnevezéseket kell megadni: " +
                "Név, Hónap, Terhelés, Számfejtés, Bér, Járulék\n" +
                "- a hónapok oszlopban a hónapokat úgy kell feltűntetni, hogy az első hét karakter ilyen formában legyen: 2023.03 " +
                "Az nem probléma, ha utána mást is írunk. Például lehet így vezetni a tört hónapokat: 2023.03.15-2023.03.30\n" +
                "\nA betöltés menete:\n" +
                "- az első mezőbe beírjuk a könyvelni kívánt hónapot a következő formátumban: 2023.03\n" +
                "- a CSV fájl mezőbe beírjuk, hogy milyen elnevezésű CSV fájlt kívánunk kapni. Például: TET20230426011000F\n" +
                "- a tallózás gombra kattintva egyesével beállítjuk, hogy melyik költségkövető fájlokat kívánjuk használni. Egyszerre " +
                "több fájl is választható\n" +
                "- a Fájl generálása gombra kattintva elvégezzük a generálást.");
        }
    }
}
