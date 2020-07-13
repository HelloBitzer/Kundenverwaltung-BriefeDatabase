using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klassen_Bibliothek
{
    public class Bankverbindung
    {
        int _kontonummer;
        int _blz;
        string _bankname;

        public Bankverbindung()
        {

        }

        public Bankverbindung(int kontonummer, int blz, string bankname)
        {
            _kontonummer = kontonummer;
            _blz = blz;
            _bankname = bankname;
        }

        public int Kontonummer { get => _kontonummer; set => _kontonummer = value; }
        public int BLZ { get => _blz; set => _blz = value; }
        public string Bankname { get => _bankname; set => _bankname = value; }

        public override string ToString()
        {
            return $"{Kontonummer} , {BLZ} , {Bankname} ";
        }

    }
}
