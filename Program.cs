using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Szabadulos_Jatek
{
    /// <summary>
    /// Sajnos a githubbbal kevés tapasztalatom van ezért nem tudom, hogy most mindig uj branchet kellett volna használnom vagy elég ha mindig
    /// master branchre mentek, így most nem vagyok benne biztos,hogy nyomonkövethető-e a haladásom, de valahogy úgy nézett ki hogy:
    /// péntek: kitaláltam és lekodoltam, hogy minden szoba és tárgy egy class legyen(targy.cs)
    /// szombat-szerda: nekiálltam az utasitas különböztetőnek és tökéletisítettem azt(itt 2 napot nem dologoztam külsös okok miatt)
    /// csütörtök: mentés - betöltéssel bajlódtam péntekig
    /// </summary>
    class Program
    {
        public static Szoba NappaliSzoba = NappaliGen();
        public static Szoba FurdoSzoba = FurdoGen();
        public static Jatekos jatekos = new Jatekos(NappaliSzoba);

        static void Main(string[] args)
        {
            Jatek();
            Console.ReadKey();
        }



        public static void Jatek()
        {

            string[] altalanosParancsok = { "menj", "nézd", "vedd", "tedd", "nyisd", "húzd", "törd" };
            string[] speckoParancsok = { "leltár", "mentés", "betöltés" };
            string input;

            do
            {
                input = Console.ReadLine();
                if (altalanosParancsok.Contains(input.Split()[0]) || speckoParancsok.Contains(input.Split()[0]))
                {
                    UtasitasKulonbozteto(input);
                }
                else
                {
                    Console.WriteLine("Ez a parancs nem létezik");
                }
            } while (!jatekos.nyert);

        }

        public static void UtasitasKulonbozteto(string input)
        {
            bool nincsSzobaban = true;
            input.ToLower();
            string[] targynelkuliparancsok = { "leltár", "menj észak", "menj dél", "menj kelet", "menj nyugat", "mentés mentes.sav", "betöltés mentes.sav" };
            if (targynelkuliparancsok.Contains(input))
            {
                switch (input)
                {
                    case "leltár":
                        if (jatekos.Leltar.Count == 0) Console.WriteLine("A leltárad üres");
                        else
                        {
                            Console.Write("Nálad van: ");
                            foreach (var item in jatekos.Leltar)
                            {
                                Console.Write(item.targyNev + " ");
                            }
                            Console.WriteLine();
                        }
                        break;

                    case "menj észak":
                        if (jatekos.ablakBetorve)
                        {
                            jatekos.nyert = true;
                            Console.WriteLine(jatekos.Pozicio.szobaEszak);
                        }
                        else Console.WriteLine(jatekos.Pozicio.szobaEszak);
                        break;
                    case "menj dél":
                        Console.WriteLine(jatekos.Pozicio.szobaDel);
                        break;
                    case "menj kelet":
                        if (jatekos.Pozicio == FurdoSzoba)
                        {
                            Console.WriteLine(jatekos.Pozicio.szobaKelet);
                            jatekos.Pozicio = NappaliSzoba;
                        }
                        else Console.WriteLine(jatekos.Pozicio.szobaKelet);
                        break;
                    case "menj nyugat":
                        if (jatekos.ajtoNyitva && jatekos.Pozicio == NappaliSzoba)
                        {
                            Console.WriteLine(jatekos.Pozicio.szobaNyugat);
                            jatekos.Pozicio = FurdoSzoba;
                        }
                        else if (!jatekos.ajtoNyitva && jatekos.Pozicio == NappaliSzoba) Console.WriteLine("Amíg nem nyitod ki azt az ajtót nem tudsz arra menni");
                        else Console.WriteLine(jatekos.Pozicio.szobaNyugat);
                        break;
                    case "mentés mentes.sav":
                        jatekos.Mentes();
                        Console.WriteLine("Játék elmentve");
                        break;
                    case "betöltés mentes.sav":
                        jatekos.Betolt(NappaliSzoba, FurdoSzoba);
                        Console.WriteLine("Játék betöltve");
                        break;
                        
                }
            }
            else
            {

                if (input == "nézd") { Console.WriteLine(jatekos.Pozicio.nezdSzoba); nincsSzobaban = false; }
                else
                {
                    foreach (var item in jatekos.Pozicio.szobaTargyai.Concat(jatekos.Leltar))
                    {
                        if (!input.Contains(item.targyNev))
                        {
                            nincsSzobaban = true;
                            continue;
                        }
                        else
                        {
                            nincsSzobaban = false;
                            if (input == $"nézd {item.targyNev}")
                            {
                                if (input == "nézd kád") jatekos.kadNezve = true;
                                Console.WriteLine(item.nezdUtan);
                                break;
                            }
                            else if (input == $"nyisd {item.targyNev}")
                            {
                                if (item.hasznalhatoParancsok.Contains("nyisd")) { Console.WriteLine(item.nyisdUtan); }
                                else { Console.WriteLine($"A {item.targyNev} nem kinyitható"); }
                                if (input == "nyisd szekrény") { jatekos.szekrenyNyitva = true; }
                                if (input == "nyisd doboz") { jatekos.dobozNyitva = true; }
                                break;
                            }
                            else if (input == $"nyisd {item.targyNev} kulcs")
                            {

                                if (input == "nyisd ajtó kulcs" && jatekos.kulcsFelvéve) jatekos.ajtoNyitva = true;
                                if (item.hasznalhatoParancsok.Contains("nyisd kulcs")) Console.WriteLine(item.nyisdkulcsUtan);
                                else Console.WriteLine($"A {item.targyNev} nem nyitható kulccsal");
                                break;
                            }
                            else if (input == $"húzd {item.targyNev}")
                            {
                                if (input == "húzd szekrény")
                                {
                                    jatekos.szekrenyHuzva = true;
                                    NappaliSzoba.szobaEszak = "Északra az ablak van, be kéne törni";
                                }
                                if (item.hasznalhatoParancsok.Contains("húzd")) Console.WriteLine(item.huzdUtan);
                                else Console.WriteLine($"A {item.targyNev} nem elhúzható");
                                break;
                            }
                            else if (input == $"törd {item.targyNev}")
                            {

                                if (item.hasznalhatoParancsok.Contains("törd")) Console.WriteLine(item.tordUtan);
                                else Console.WriteLine($"A {item.targyNev} nem törhető");
                                break;
                            }
                            else if (input == $"törd {item.targyNev} feszítővas")
                            {
                                if (input == "törd ablak feszítővas")
                                {
                                    jatekos.ablakBetorve = true;
                                    NappaliSzoba.szobaEszak = "Gratulálunk sikerült megszöknöd";
                                    NappaliSzoba.nezdSzoba = "Nyugatra ajtó, Északra betörten tátongó ablak néz a külvilágra";
                                }
                                if (item.hasznalhatoParancsok.Contains("törd feszítővas")) Console.WriteLine(item.tordfeszvasUtan);
                                else Console.WriteLine($"A {item.targyNev} nem törhető feszítővassal sem");
                                break;
                            }
                            else if (input == $"vedd fel {item.targyNev}")
                            {
                                if (item.hasznalhatoParancsok.Contains("vedd fel"))
                                {
                                    if (jatekos.kadNezve && input == "vedd fel feszítővas")
                                    {
                                        Console.WriteLine(item.veddfelUtan);
                                        jatekos.Pozicio.szobaTargyai.Remove(item);
                                        jatekos.Leltar.Add(item);
                                    }
                                    else if (jatekos.szekrenyNyitva && input == "vedd fel doboz")
                                    {
                                        Console.WriteLine(item.veddfelUtan);
                                        jatekos.Pozicio.szobaTargyai.Remove(item);
                                        jatekos.Leltar.Add(item);
                                    }
                                    else if (jatekos.dobozNyitva && input == "vedd fel kulcs")
                                    {
                                        jatekos.kulcsFelvéve = true;
                                        Console.WriteLine(item.veddfelUtan);
                                        jatekos.Pozicio.szobaTargyai.Remove(item);
                                        jatekos.Leltar.Add(item);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"A {item.targyNev} nem található még a szobában");
                                    }

                                }
                                else Console.WriteLine($"A {item.targyNev} nem vehető fel");

                                break;
                            }
                            else if (input == $"tedd le {item.targyNev}")
                            {
                                if (item.hasznalhatoParancsok.Contains("tedd le"))
                                {
                                    Console.WriteLine(item.teddleUtan);
                                    jatekos.Leltar.Remove(item);
                                    jatekos.Pozicio.szobaTargyai.Add(item);

                                }
                                else Console.WriteLine($"A {item.targyNev} nem tehető le");
                                break;
                            }
                        }
                    }
                }
                if (nincsSzobaban) Console.WriteLine("Ez a tárgy nincs ebben szobában");
            }
        }
        public static Szoba NappaliGen()
        {
            Szekreny szekreny = new Szekreny("Ez egy egyszeru szekrény, nem kell hozzá kulcs", "Kinyitottad a szekrényt. Egy dobozt látsz.", "Elhúztad a szekrényt. Mögötte egy ablakot találsz.");
            Ablak ablak = new Ablak("Ha ezen átjutsz kiszabadulsz", "Az ablak zárva van.", "A kezeddel nem tudod összetörni, mert megvágnád magadat.", "A feszítővassal betöröd az üveget.");
            Doboz doboz = new Doboz("Könnyen kinyithatod és felveheted", "Kinyitottad a dobozt. Egy kulcsot találsz benne.");
            Kulcs ajtoKulcs = new Kulcs("Ez a kulcs nyithat akár ajtót, ládát, ablakot is.");
            Ajtó furdoAjto = new Ajtó("Az ajtó be van zárva és széttörni se lehet", "Az ajtó kulcsra van zárva", "Kinyitottad az ajtót.(nyugat)");
            List<Targy> nappaliTargyak = new List<Targy>() { ablak, szekreny, doboz, ajtoKulcs, furdoAjto };
            Szoba nappaliSzoba = new Szoba("Nappali", "A nappaliban vagy.Itt található egy szekrény.Nyugatra látsz egy ajtót.", "Északra a szekrény van", "", "", "A fürdőszobában vagy. Itt található egy kád.", nappaliTargyak);
            return nappaliSzoba;
        }
        public static Szoba FurdoGen()
        {
            Ajtó nappaliAjto = new Ajtó("Nappaliba(kelet) nyíló nyitott ajtó", "Már nyitva van", "Már nyitva van");
            Kad furdoKád = new Kad("A kádban egy feszítővasat látsz.");
            Feszitovas feszitovas = new Feszitovas("A fesztítővas alkalmas dolgok széttörésére");
            List<Targy> furdoTargyak = new List<Targy>() { nappaliAjto, furdoKád, feszitovas };
            Szoba furdoSzoba = new Szoba("Fürdőszoba", "A fürdőszobában vagy. Itt található egy kád. ", "", "", "A nappaliban vagy. Itt található egy szekrény. Nyugatra látsz egy ajtót.", "", furdoTargyak);
            return furdoSzoba;
        }
    }
}
