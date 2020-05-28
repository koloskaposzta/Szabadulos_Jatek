using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace Szabadulos_Jatek
{

    class Jatekos
    {
        const string fajl = @"../../mentes.sav";
        public List<Targy> Leltar = new List<Targy>(3);
        public Szoba Pozicio { get; set; }
        public bool szekrenyNyitva { get; set; }
        public bool szekrenyHuzva { get; set; }
        public bool dobozNyitva { get; set; }
        public bool kulcsFelvéve { get; set; }
        public bool ajtoNyitva { get; set; }
        public bool ablakBetorve { get; set; }
        public bool kadNezve { get; set; }
        public bool nyert { get; set; }
        List<bool> progress = new List<bool>();

        public Jatekos(Szoba poz)
        {
            szekrenyNyitva = false;
            szekrenyHuzva = false;
            dobozNyitva = false;
            kulcsFelvéve = false;
            ajtoNyitva = false;
            ablakBetorve = false;
            kadNezve = false;
            nyert = false;

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

            try
            {
                string[] s = File.ReadAllText(fajl).Split();
            if (s[0] == "N") Pozicio = nappaliSzoba;
            else if (s[0] == "F") Pozicio = furdoSzoba;
            string progressString = s[1];
            int cnt = 0;
            foreach (var item in progressString)
            {
                if (item == '1')
                {
                    progress[cnt] = true;
                }
                else progress[cnt] = false;
                cnt++;
            }
            string leltarString = s[2];
            foreach (var item in leltarString)
            {
                if (item == 'd') ((Doboz)nappaliSzoba.szobaTargyai[2]).TargyFelVeves(this, nappaliSzoba);
                else if (item == 'k') nappaliSzoba.szobaTargyai[3].TargyFelVeves(this, nappaliSzoba);
                else if (item == 'f') furdoSzoba.szobaTargyai[2].TargyFelVeves(this, furdoSzoba);

            }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sikertelen betöltés, a mentes.sav-ot helytelenül módosították.");
                Console.WriteLine(ex);
                throw;
            }


        }

        public void Mentes()
        {
            progress.Clear();
            progress.Add(szekrenyNyitva);
            progress.Add(szekrenyHuzva);
            progress.Add(dobozNyitva);
            progress.Add(kulcsFelvéve);
            progress.Add(ajtoNyitva);
            progress.Add(kadNezve);
            progress.Add(ablakBetorve);

            using (StreamWriter sw = new StreamWriter(fajl))
            {
                if (Pozicio.szobaNeve == "Nappali") sw.Write("N ");
                else if (Pozicio.szobaNeve == "Fürdőszoba") sw.Write("F ");
                foreach (var item in progress)
                {
                    if (item.Equals(true)) sw.Write("1");
                    else sw.Write("0");
                }
                sw.Write(" ");
                foreach (var item in Leltar)
                {
                    if (item.targyNev == "doboz") sw.Write("d");
                    else if (item.targyNev == "kulcs") sw.Write("k");
                    else if (item.targyNev == "feszítővas") sw.Write("f");
                }
                sw.Close();
            }

        }
    }
}
