using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace ProjektNeu
{
    class laden_speichern
    {
        public void KundenSpeichern(List<Kunde> Kundenliste)
        {
            //Serializer und Filestream erstellen
            XmlSerializer xmlser = new XmlSerializer(typeof(List<Kunde>));
            FileStream fs = new FileStream("Kundendaten.xml", FileMode.Create);

            xmlser.Serialize(fs, Kundenliste);
            fs.Close();
        }

        public List<Kunde> KundenLaden()
        {

            if (File.Exists("Kundendaten.xml"))
            {

                XmlSerializer xmlser = new XmlSerializer(typeof(List<Kunde>));
                FileStream fs = new FileStream("Kundendaten.xml", FileMode.Open);

                List<Kunde> geladeneListe = new List<Kunde>();
                  geladeneListe=  (List<Kunde>)xmlser.Deserialize(fs);
                fs.Close();

                return geladeneListe;
            }


            return new List<Kunde>();
        }
    }
}                             


    
