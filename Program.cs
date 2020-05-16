using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szabadulos_Jatek
{
    class Szoba {
        public string szobaNev;
        public string[] hasznalhatoParancsok = { "menj", "nézd" };
        public string szobaÉszak;
        public string szobaDél;
        public string szobaKelet;
        public string szobaNyugat;

    }
    class Targy
    {
        class Szekreny : Targy
        {
            public string[] hasznalhatoParancs = {"nyisd","húzd" };
            public string tartalma;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Script();
        }

        public static void Script()
        {
             string[] parancsok ={"menj","nézd","vedd fel","tedd le", "nyisd", "húzd","törd" };
             List<string> leltar = new List<string>(); 
        }
    }
}
