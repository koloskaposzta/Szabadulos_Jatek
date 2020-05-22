using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Szabadulos_Jatek
{
    class Targy
    {
        public string targyNev { get; set; }
        public string nezdUtan { get; set; }
        public string[] hasznalhatoParancsok { get; set; }
        public string nyisdUtan { get; set; }
        public string nyisdkulcsUtan { get; set; }
        public string huzdUtan { get; set; }
        public string tordUtan { get; set; }
        public string tordfeszvasUtan { get; set; }
        public string veddfelUtan { get; set; }
        public string teddleUtan { get; set; }
    }
    class Szekreny : Targy
    {
        public Szekreny(string nezd, string nyisd, string huzd)
        {
            hasznalhatoParancsok = new string[3] { "nézd", "húzd", "nyisd" };
            nezdUtan = nezd;
            nyisdUtan = nyisd;
            huzdUtan = huzd;
            targyNev = "szekrény";
        }
    }
    class Agy : Targy
    {
        public Agy(string nezd)
        {
            hasznalhatoParancsok = new string[2] { "nézd", "húzd" };
            nezdUtan = nezd;
            targyNev = "ágy";
        }
    }
    class Kad : Targy
    {
        public Kad(string nezd)
        {
            nezdUtan = nezd;
            targyNev = "kád";
            hasznalhatoParancsok = new string[1] { "nézd" };
        }
    }
    class Ajtó : Targy
    {
        public Ajtó(string nezd, string nyisd, string nyisdkulcs)
        {
            hasznalhatoParancsok = new string[3] { "nézd", "nyisd", "nyisd kulcs" };
            nezdUtan = nezd;
            nyisdUtan = nyisd;
            nyisdkulcsUtan = nyisdkulcs;
            targyNev = "ajtó";
        }
    }
    class Ablak : Targy
    {


        public Ablak(string nezd, string nyisd, string tord, string tordfeszvas)
        {
            hasznalhatoParancsok = new string[4] { "nézd", "nyisd", "törd", "törd feszítővas" };
            nezdUtan = nezd;
            nyisdUtan = nyisd;
            tordUtan = tord;
            tordfeszvasUtan = tordfeszvas;
            targyNev = "ablak";
        }
    }
    class Feszitovas : Targy
    {


        public Feszitovas(string nezd)
        {
            hasznalhatoParancsok = new string[3] { "nézd", "vedd fel", "tedd le" };
            nezdUtan = nezd;
            veddfelUtan = "Felvetted a feszítővasat";
            teddleUtan = "Letetted a feszítővasat";
            targyNev = "feszítővas";

        }
    }
    class Doboz : Targy
    {

        public Doboz(string nezd, string nyisd)
        {
            hasznalhatoParancsok = new string[4] { "nézd", "nyisd", "vedd fel", "tedd le" };
            nezdUtan = nezd;
            nyisdUtan = nyisd;
            veddfelUtan = "Felvetted a dobozt";
            teddleUtan = "Letetted a dobozt";
            targyNev = "doboz";
        }
    }
    class Kulcs : Targy
    {


        public Kulcs(string nezd)
        {
            hasznalhatoParancsok = new string[3] { "nézd", "vedd fel", "tedd le" };
            nezdUtan = nezd;
            veddfelUtan = "Felvetted a kulcsot";
            teddleUtan = "Letetted a kulcsot";
            targyNev = "kulcs";
        }
    }

    //
    //
    //

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

    //
    //
    //

    class Jatekos
    {
        const string fajl = @"../../mentes.sav";
        public List<Targy> Leltar = new List<Targy>(3);
        public Szoba Pozicio { get; set; }
        public bool szekrenyNyitva = false;
        public bool szekrenyHuzva = false;
        public bool dobozNyitva = false;
        public bool kulcsFelvéve = false;
        public bool ajtoNyitva = false;
        public bool ablakBetorve = false;
        public bool kadNezve = false;
        public bool nyert = false;
        List<bool> progress = new List<bool>();

        public Jatekos(Szoba poz)
        {
            Pozicio = poz;
            progress.Add(szekrenyNyitva);
            progress.Add(szekrenyHuzva);
            progress.Add(dobozNyitva);
            progress.Add(kulcsFelvéve);
            progress.Add(ajtoNyitva);
            progress.Add(kadNezve);
            progress.Add(ablakBetorve);
        }
        public void Betolt(Szoba nappaliSzoba, Szoba furdoSzoba)
        {
            if (File.ReadAllText(fajl).Split()[0] == "Nappali") { Pozicio = nappaliSzoba; }
            else if (File.ReadAllText(fajl).Split()[0] == "Fürdőszoba") { Pozicio = furdoSzoba; }
            int cnt = 0;
            foreach (var item in File.ReadAllLines(fajl).Skip(1))
            {
                progress[cnt] = Convert.ToBoolean(item);
            }

        }

        public void Mentes()
        {
            using (StreamWriter sw = new StreamWriter(fajl))
            {
                sw.WriteLine(Pozicio.szobaNeve);
                foreach (var item in progress)
                {
                    sw.WriteLine(item);
                }
                sw.Close();
            }

        }
    }
}
