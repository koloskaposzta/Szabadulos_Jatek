using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Szabadulos_Jatek
{
    /// <summary>
    /// 
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
            bool Voltmar = false;
            bool nincsSzobaban = true;
            input.ToLower();
            string[] targynelkuliparancsok = { "leltár", "menj", "mentés", "betöltés" };
            foreach (var i in targynelkuliparancsok)
            {
                if (input.Contains(i))
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
                    if (input.Contains("menj") && (!input.Contains("észak") || !input.Contains("dél") || !input.Contains("nyugat") || !input.Contains("kelet")))
                    {
                        Console.WriteLine("A menj parancs után, égtájat kell megadni.(észak, dél, nyugat, kelet)");
                    }
                    else if ((input.Contains("betöltés") || input.Contains("mentés")) && !input.Contains("mentes.sav"))
                    {
                        Console.WriteLine("Csak a mentes.sav létezik mint mentés file");
                    }
                    Voltmar = true;
                    break;
                }
            }

            if (!targynelkuliparancsok.Contains(input) && !Voltmar)
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
                                item.TargyNezes(jatekos);
                                break;
                            }
                            else if (input == $"nyisd {item.targyNev}")
                            {
                                item.TargyNyitas(jatekos);
                                break;
                            }
                            else if (input == $"nyisd {item.targyNev} kulcs")
                            {
                                item.TargyNyitasKulccsal(jatekos, NappaliSzoba.szobaTargyai[3]);
                                break;
                            }
                            else if (input == $"húzd {item.targyNev}")
                            {
                                item.TargyHuzas(jatekos, jatekos.Pozicio);
                                break;
                            }
                            else if (input == $"törd {item.targyNev}")
                            {
                                item.TargyTores(jatekos);
                                break;
                            }
                            else if (input == $"törd {item.targyNev} feszítővas")
                            {
                                item.TargyToresFeszitovassal(jatekos, jatekos.Pozicio);
                                break;
                            }
                            else if (input == $"vedd fel {item.targyNev}")
                            {
                                item.TargyFelVeves(jatekos, jatekos.Pozicio);
                                break;
                            }
                            else if (input == $"tedd le {item.targyNev}")
                            {
                                item.TargyLeTeves(jatekos, jatekos.Pozicio);
                                break;
                            }
                        }
                    }
                    if (nincsSzobaban) Console.WriteLine("Ez a tárgy nincs ebben szobában");
                }

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
