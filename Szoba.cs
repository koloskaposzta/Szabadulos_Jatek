using System.Collections.Generic;
namespace Szabadulos_Jatek
{

    class Szoba
    {
        string[] hasznalhatoParancsok = { "menj", "nézd" };
        public string szobaNeve { get; set; }
        public string nezdSzoba { get; set; }
        public string szobaEszak { get; set; }
        public string szobaDel { get; set; }
        public string szobaKelet { get; set; }
        public string szobaNyugat { get; set; }
        public List<Targy> szobaTargyai = new List<Targy>();
        public List<string> szobaTargyainakNeve = new List<string>();
        public Szoba(string nev, string nezd, string eszak, string del, string kelet, string nyugat, List<Targy> targyak)
        {
            szobaNeve = nev;
            nezdSzoba = nezd;
            if (eszak == "") szobaEszak = "Északra nincs kijárat";
            else szobaEszak = eszak;
            if (del == "") szobaDel = "Délre nincs kijárat";
            else szobaDel = del;
            if (kelet == "") szobaKelet = "Keletre nincs kijárat";
            else szobaKelet = kelet;
            if (nyugat == "") szobaNyugat = "Nyugatra nincs kijárat";
            else szobaNyugat = nyugat;

            szobaTargyai = targyak;
            foreach (var item in szobaTargyai)
            {
                szobaTargyainakNeve.Add(item.targyNev);
            }
        }
    }
}
