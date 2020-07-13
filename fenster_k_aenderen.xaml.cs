using klassen_Bibliothek;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ProjektNeu
{
    /// <summary>
    /// Interaktionslogik für fenster_k_aenderen.xaml
    /// </summary>
    public partial class fenster_k_aenderen : Window
    {
        
        public int idx;
        string connstring = "Server=localhost;Database=team1;Uid=root;";

        public Kunde k1 = new Kunde();
        public List<Kunde> Kundenliste ;
        public bool SQLuse;

        //public object DG_Allekunden { get; private set; }


            
        //public fenster_k_aenderen(List<Kunde> Kundenliste, int index)
        public fenster_k_aenderen(Kunde kunde, DataRowView data, int index, bool useSQL)

        {
            InitializeComponent();
            k1 = kunde;
            SQLuse = useSQL;
            idx = index;

            if (SQLuse==false)    //use sql oder liste 
            {
                laden_speichern ls = new laden_speichern();
                Kundenliste = ls.KundenLaden();

                this.Kundenliste = Kundenliste;
               


                //MessageBox.Show("DEBUG:: open window aenderen");

                TB_Nname.Text = Kundenliste[index].Nachname; 
                TB_Vname.Text = Kundenliste[index].Vorname;
                TB_Geburtsdatum.Text = Kundenliste[index].geburtsdatum.ToString();
                TB_mail.Text = Kundenliste[index].kontaktdaten.Email;
                TB_Telefon.Text = Kundenliste[index].kontaktdaten.Telefon;
                TB_Strasse.Text = Kundenliste[index].kontaktdaten.Adress.Strasse;
                TB_Hausnummer.Text = Kundenliste[index].kontaktdaten.Adress.Hausnummer;
                TB_PLZ.Text = Kundenliste[index].kontaktdaten.Adress.PLZ;
                TB_Ort.Text = Kundenliste[index].kontaktdaten.Adress.Ort;

            }
            else
            {
            
            k1.KundeID = (int)data.Row.ItemArray[0];
            k1.Vorname = (string)data.Row.ItemArray[1];

            k1.Nachname = (string)data.Row.ItemArray[2];
            k1.geburtsdatum = (DateTime)data.Row.ItemArray[3];

            k1.kontaktdaten = new KontaktDaten();
            k1.kontaktdaten.Email = (string)data.Row.ItemArray[4];
            k1.kontaktdaten.Telefon = (string)data.Row.ItemArray[5];

            k1.Kontaktdaten.Adress = new Adresse();
            k1.kontaktdaten.Adress.Strasse = (string)data.Row.ItemArray[6];
            k1.kontaktdaten.Adress.Hausnummer = (string)data.Row.ItemArray[7];
            k1.kontaktdaten.Adress.PLZ = (string)data.Row.ItemArray[8];
            k1.kontaktdaten.Adress.Ort = (string)data.Row.ItemArray[9];

            ////Kundendaten transfer from DG to Anlege Fenster

            TB_Nname.Text = k1.Nachname; ;
            TB_Vname.Text = k1.Vorname;
            TB_Geburtsdatum.Text = k1.geburtsdatum.ToString();
            TB_mail.Text = k1.kontaktdaten.Email;
            TB_Telefon.Text = k1.kontaktdaten.Telefon;
            TB_Strasse.Text = k1.kontaktdaten.Adress.Strasse;
            TB_Hausnummer.Text = k1.kontaktdaten.Adress.Hausnummer;
            TB_PLZ.Text = k1.kontaktdaten.Adress.PLZ;
            TB_Ort.Text = k1.kontaktdaten.Adress.Ort;
            }

        }

        public void BT_aendern_Click(object sender, RoutedEventArgs e)
        {


            if (SQLuse == false)
            {
                
                //Kundendaten ändern
                MessageBox.Show($"{idx}");                                     // Index controller
                Kundenliste[idx].Nachname = TB_Nname.Text;
                Kundenliste[idx]._vorname = TB_Vname.Text;
                Kundenliste[idx].Kontaktdaten.Email = TB_mail.Text;
                Kundenliste[idx].Kontaktdaten.Telefon = TB_Telefon.Text;
                DateTime tempDat = Kundenliste[idx].geburtsdatum;
                if (DateTime.TryParse(TB_Geburtsdatum.Text, out Kundenliste[idx].geburtsdatum))
                {
                }
                else
                {
                    MessageBox.Show("Falsches Datumsformat.");
                    Kundenliste[idx].geburtsdatum = tempDat;
                    //close = false;
                }

                Kundenliste[idx].Kontaktdaten.Adress.Umzug(TB_Strasse.Text, TB_Hausnummer.Text, TB_PLZ.Text, TB_Ort.Text);

                if (CB_Lieferanschrift.IsChecked == true)
                {
                    //falls abweichende lieferanschrift 
                    Kundenliste[idx].lieferanschrift.Umzug(TB_Strasse_la.Text, TB_Hausnummer_la.Text, TB_PLZ_la.Text, TB_Ort_la.Text);

                }
                laden_speichern ls = new laden_speichern();
                ls.KundenSpeichern(Kundenliste) ;
            }
            //if (close)
            //{

            //    this.Close();
            //}
        


            else
            {

            //Kommunikation mit Datenbank
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=Team1;Uid=root;");

            con.Open();



            //Speicher 
            k1.Vorname = TB_Vname.Text;
            k1.Nachname = TB_Nname.Text;
            k1.geburtsdatum = (DateTime.Parse(TB_Geburtsdatum.Text));

            k1.kontaktdaten = new KontaktDaten();
            k1.kontaktdaten.Email = TB_mail.Text;
            k1.kontaktdaten.Telefon = TB_Telefon.Text;

            k1.kontaktdaten.Adress = new Adresse();
            k1.Kontaktdaten.Adress.Strasse = TB_Strasse.Text;
            k1.kontaktdaten.Adress.Hausnummer = TB_Hausnummer.Text;
            k1.kontaktdaten.Adress.PLZ = TB_PLZ.Text;
            k1.kontaktdaten.Adress.Ort = TB_Ort.Text;

               
                string kid = "";
                string konid = "";
                string adid = "";


            string query = $"select * from t_kunde where KundenID= {k1.KundeID }";
               
            MySqlCommand com = new MySqlCommand(query);
                com.Connection = con;
                MySqlDataReader reader = com.ExecuteReader(); // lesen alle daten von t_kunde
                if (reader.Read())
                {
                    kid = reader[0].ToString();           // er holt nur stelle 0 , stelle 0 isr KundenID
                    konid = reader[2].ToString();        //er holt nur stelle 2 , stelle 0 isr kontaktID
                    adid = reader[5].ToString();         //AdressID
                }
                reader.Close();
                
                
                MessageBox.Show($"DEBUG:in AENDEREN: KundenID ={ kid} ");

                //MessageBox.Show("DEBUG:in AENDEREN: Daten aus Formular gelesen ");
                string updatequery = $"update t_kunde set Nachname='{k1.Nachname}',Vorname='{k1.Vorname}'  where KundenID = {kid}";

            com.CommandText = updatequery;
            com.ExecuteNonQuery();
               
            //MessageBox.Show("DEBUG:in AENDEREN: in SQL geschrieben ");
            MessageBox.Show($"DEBUG:in AENDEREN: kontaktID = {konid} ");


                string konupdate = $"update t_kontaktdaten set Email='{k1.kontaktdaten.Email}',Telefon='{k1.kontaktdaten.Telefon}'   where KontaktdatenID = {konid} ";
                com.CommandText = konupdate;
                com.ExecuteNonQuery();

                string adressupdate = $"update t_adresse set Strasse ='{k1.kontaktdaten.Adress.Strasse}',Hausnummer ='{k1.kontaktdaten.Adress.Hausnummer}' ,PLZ= '{k1.kontaktdaten.Adress.PLZ}',Ort= '{k1.kontaktdaten.Adress.Ort}' where AdressenID={adid} ";
                com.CommandText = adressupdate;
                com.ExecuteNonQuery();

               

            con.Close();
            }
            Close();
           
           
        }








        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //laden_speichern ls = new laden_speichern();
            //ls.KundenSpeichern(Kundenliste);


        }

    }
}

