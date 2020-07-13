using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klassen_Bibliothek
{
    public abstract class Person
    {
        public string _vorname;
        public string _nachname;

        public Person()
        {

        }

        public string Vorname { get => _vorname; set => _vorname = value; }
        public string Nachname { get => _nachname; set => _nachname = value; }

        public Person(string VName, string NName)
        {
            _vorname = NName;
            _nachname = VName;
        }
        public override string ToString()
        {
            string output = "";

            output += $"{Vorname} {Nachname}";

            return output;
        }
    }
}
   
