using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klassen_Bibliothek
{
    public enum Grund
    {
        Marketing,
        Versamlung,
        Einkauf,
        Verkaufszahlen
    }
    public class Termin
    {
        int _terminID;
        int _kundenID;
        int _verantwortlicheMA;
        DateTime _start;
        DateTime _ende;
        Grund _termingrund;
        string _bemerkung;

        public Termin()
        {

        }

        public Termin(int terminID, int kundenID, int verantwortlicheMA, DateTime start, DateTime ende, Grund termingrund, string bemerkung)
        {
            _terminID = terminID;
            _kundenID = kundenID;
            _verantwortlicheMA = verantwortlicheMA;
            _start = start;
            _ende = ende;
            _termingrund = termingrund;
            _bemerkung = bemerkung;
        }

        public int TerminID { get => _terminID; set => _terminID = value; }
        public int KundenID { get => _kundenID; set => _kundenID = value; }
        public int VerantwortlicheMA { get => _verantwortlicheMA; set => _verantwortlicheMA = value; }
        public DateTime Start { get => _start; set => _start = value; }
        public DateTime Ende { get => _ende; set => _ende = value; }
        public Grund Termingrund { get => _termingrund; set => _termingrund = value; }
        public string Bemerkung { get => _bemerkung; set => _bemerkung = value; }

        public override string ToString()
        {
            return TerminID + KundenID + VerantwortlicheMA + _start.ToShortDateString() + ", " + _start.Hour + ":" + _start.Minute + " " + _termingrund + Bemerkung;
        }
    }

}
