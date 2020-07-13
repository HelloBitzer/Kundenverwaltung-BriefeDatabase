using klassen_Bibliothek;
using System;
using System.Collections.Generic;

namespace ProjektNeu
{
    public class Kunde : Person
    {
        public int kundeID;
        public DateTime geburtsdatum;
        public KontaktDaten kontaktdaten;
        public Adresse lieferanschrift;
        public List<Termin> termine = new List<Termin>();



        public int KundeID { get => kundeID;  set => kundeID = value; }
        public DateTime Geburtsdatum { get => geburtsdatum; set => geburtsdatum = value; }
        public KontaktDaten Kontaktdaten { get => kontaktdaten;  set => kontaktdaten = value; }
        public Adresse Lieferanschrift { get => lieferanschrift;  set => lieferanschrift = value; }
        public List<Termin> Termine { get => termine; set => termine = value; }


        public Kunde()
        {

        }



        public Kunde(string VName, string NName, string GeburtsDatum, KontaktDaten kundenkontaktdaten) : base(VName, NName)
        {
            geburtsdatum = Convert.ToDateTime( GeburtsDatum);
            kontaktdaten = kundenkontaktdaten;
           
        }


        public Kunde(string VName, string NName, string Geburtsdatum,Adresse Lieferanschrift,string strasse, string plz,string ort, string hnr,string email,string tel) : base(VName,NName)
        {
           
            lieferanschrift = Lieferanschrift;
            geburtsdatum = Convert.ToDateTime(Geburtsdatum);
            Kontaktdaten = new KontaktDaten(new Adresse(strasse,hnr,plz,ort),email,tel);

        }

        public Kunde(string VName, string NName,string Geburtsdatum, string strasse, string plz, string ort, string hnr, string email, string tel) : base(VName, NName)
        {
            Kontaktdaten = new KontaktDaten(new Adresse(strasse, hnr, plz, ort), email, tel);
            
           lieferanschrift = kontaktdaten.Adress;
            
            geburtsdatum = Convert.ToDateTime(Geburtsdatum);

        }
        
    }
        }
    


