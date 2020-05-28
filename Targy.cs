using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void TargyNezes(Jatekos jatekos)
        {
            if (targyNev == "kád") jatekos.kadNezve = true;
            Console.WriteLine(nezdUtan);

        }
        public void TargyNyitas(Jatekos jatekos)
        {
            if (hasznalhatoParancsok.Contains("nyisd"))
            {
                Console.WriteLine(nyisdUtan);
            }
            else Console.WriteLine($"A {targyNev} nem kinyitható");
            if (targyNev == "szekrény") jatekos.szekrenyNyitva = true;
            if (targyNev == "doboz") jatekos.dobozNyitva = true;
        }
        public void TargyNyitasKulccsal(Jatekos jatekos, Targy kulcs)
        {
            if (jatekos.Leltar.Contains(kulcs))
            {
                if (hasznalhatoParancsok.Contains("nyisd kulcs"))
                {
                    Console.WriteLine(nyisdkulcsUtan);
                    jatekos.ajtoNyitva = true;
                }
                else if (hasznalhatoParancsok.Contains("nyisd"))
                {
                    Console.WriteLine($"A {targyNev} nyitásához nem kell kulcs");
                    TargyNyitas(jatekos);
                }
                else Console.WriteLine($"A {targyNev} nem nyitható kulccsal sem");
            }
            else Console.WriteLine("Nincs is kulcs a leltárodban!");

        }
        public void TargyHuzas(Jatekos jatekos, Szoba aktualisSzoba)
        {
            if (hasznalhatoParancsok.Contains("húzd"))
            {
                Console.WriteLine(huzdUtan);
                if (targyNev == "szekrény") jatekos.szekrenyHuzva = true;
                aktualisSzoba.nezdSzoba = "Nappaliban vagy, északra egy ablak, nyugatra ajtó";
                aktualisSzoba.szobaEszak = "Ahhoz hogy északra tudj menni, be kell törni azt az ablakot.";

            }
            else Console.WriteLine($"A {targyNev} nem elhúható");
        }
        public void TargyTores(Jatekos jatekos)
        {
            if (hasznalhatoParancsok.Contains("törd"))
            {
                Console.WriteLine(tordUtan);
            }
            else Console.WriteLine($"A {targyNev} nem törhető");
        }
        public void TargyToresFeszitovassal(Jatekos jatekos, Szoba aktualisSzoba)
        {
            if (hasznalhatoParancsok.Contains("törd feszítővas"))
            {
                Console.WriteLine(tordfeszvasUtan);
                if (targyNev == "ablak")
                {
                    jatekos.ablakBetorve = true;
                    aktualisSzoba.nezdSzoba = "Nappaliban vagy. Északra tántongó lyuk, nyugatra ajtó";
                    aktualisSzoba.szobaEszak = "Gratulálunk sikerült megszöknöd";
                    aktualisSzoba.szobaTargyai[0].nezdUtan = "Be van törve az ablak, ha kimész rajta kiszabadulsz.";
                }

            }
            else if (hasznalhatoParancsok.Contains("törd"))
            {
                Console.WriteLine($"A {targyNev} eltöréséhez nem kell feszítővas");
                TargyTores(jatekos);
            }
            else Console.WriteLine($"A {targyNev} nem törhető feszítővassal sem");

        }
        public void TargyFelVeves(Jatekos jatekos, Szoba aktualisSzoba)

        {
            if (hasznalhatoParancsok.Contains("vedd fel"))
            {
                if (aktualisSzoba.szobaTargyai.Contains(this))
                {
                    jatekos.Leltar.Add(this);
                    aktualisSzoba.szobaTargyai.Remove(this);
                    Console.WriteLine(veddfelUtan);
                    if (targyNev == "doboz" && jatekos.szekrenyNyitva)
                    {
                        ((Szekreny)aktualisSzoba.szobaTargyai[1]).ElvittekaDobozt();
                    }
                    else if (targyNev == "kulcs" && jatekos.dobozNyitva)
                    {
                        ((Doboz)jatekos.Leltar[0]).ElvittekaKulcsot();
                        jatekos.kulcsFelvéve = true;
                    }
                    else if (targyNev == "feszítővas" && jatekos.kadNezve)
                    {
                        ((Kad)aktualisSzoba.szobaTargyai[1]).ElvittekaFeszvast();

                    }
                }
                else Console.WriteLine($"A {targyNev} nincs a szobában");
            }
            else Console.WriteLine($"A {targyNev} nem felvehető");
        }
        public void TargyLeTeves(Jatekos jatekos, Szoba aktualisSzoba)
        {
            if (hasznalhatoParancsok.Contains("tedd le"))
            {
                Console.WriteLine(teddleUtan);
                aktualisSzoba.szobaTargyai.Add(this);
                jatekos.Leltar.Remove(this);
                aktualisSzoba.nezdSzoba += $" A földön egy {targyNev} hever";
            }
            else Console.WriteLine($"A {targyNev} nem tehető le");
        }
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
        public void ElvittekaDobozt()
        {
            nyisdUtan = "Üres a szekrény";
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
        public void ElvittekaFeszvast()
        {
            nezdUtan = "Ez egy üres kád, tökéletesen beleillik egy feszítővas";
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
        public void ElvittekaKulcsot()
        {
            nyisdUtan = "Üres a doboz, már le is teheted";
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
}
