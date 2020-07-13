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
    /// Interaktionslogik für fenster_k_anlegen.xaml
    /// </summary>
    public partial class fenster_k_anlegen : Window
    {
        
     
        string connstring = "Server=localhost;Database=team1;Uid=root;";

        public Kunde k1 = new Kunde();
        public bool SQLuse;
        public List<Kunde> Kundenliste = new List<Kunde>();

        //public fenster_k_anlegen(List<Kunde> Kundenliste, int index)
        public fenster_k_anlegen(Kunde kunde, bool useSQL)

        {
            InitializeComponent();
            SQLuse = useSQL;
            if (SQLuse == false)
            {

            laden_speichern ls = new laden_speichern();
            Kundenliste = ls.KundenLaden();
            //this.Kundenliste = Kundenliste;
           
            }



        }

        public void BT_speichern_Click(object sender, RoutedEventArgs e)
        {


            if (SQLuse == false)

            {
                //MessageBox.Show("DEBUG:in MAIN: do not use SQL DB ");
                //List<Kunde> Kunde = new List<Kunde>();
                //bool close = true;

                kundeAnlegenInListe();

            }
            else
            {
                try
                {

                    //Kommunikation mit Datenbank
                    #region sql query anlegen
                    MySqlConnection con = new MySqlConnection("Server=localhost;Database=Team1;Uid=root;");

            con.Open();

            
            MySqlCommand com = new MySqlCommand();

            com.Connection = con;
            

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

            #region data holen
            // wie kann mann alle Daten von 3 Tabellen hollen ; und danach auf die Fremdkeys suchen
            //string insertadresse = $"insert into t_adresse values(null,'{k1.kontaktdaten.Adress.Strasse}','{k1.kontaktdaten.Adress.Hausnummer}','{k1.kontaktdaten.Adress.PLZ}','{k1.kontaktdaten.Adress.Ort}')";
            //string insertkontaktdaten = $"insert into t_kontaktdaten values(null,{adresid},'{k1.kontaktdaten.Email}','{k1.kontaktdaten.Telefon}')";
            //string insertkunde = $"insert into t_kunde values(null,'{date}',{konid},'{k1.Vorname}','{k1.Nachname}',{adresid})";
            //com.CommandText = insertadresse;//insert 
            //com.ExecuteNonQuery();//insertadress
            //com.CommandText = insertkontaktdaten;
            //com.ExecuteNonQuery();//für kontaktdaten
            //com.CommandText = insertkunde;
            //com.ExecuteNonQuery();für kunde


            #endregion

            string insertadresse = $"insert into t_adresse values(null,'{k1.kontaktdaten.Adress.Strasse}','{k1.kontaktdaten.Adress.Hausnummer}','{k1.kontaktdaten.Adress.PLZ}','{k1.kontaktdaten.Adress.Ort}')";
            //select für forign key

            string adressquery = $"select * from t_adresse order by AdressenId desc";


            //1
            com.CommandText = insertadresse;            //insert 
            com.ExecuteNonQuery();                     //insertadress

            // adress id nehmen und benutzen
            com.CommandText = adressquery;

            MySqlDataReader reader = com.ExecuteReader();//lesen adressen table +sorteiern desc

            string adresid = "";       //adressid was wir abgeholen
            if (reader.Read())        // hol der adress Id ab
            {
                adresid = reader[0].ToString();
            }
            reader.Close();   //close executreader
            //2
            string insertkontaktdaten = $"insert into t_kontaktdaten values(null,{adresid},'{k1.kontaktdaten.Email}','{k1.kontaktdaten.Telefon}')";
            string kontaktquery = $"select * from t_kontaktdaten order by AdressenId desc";//ich brauche die für kunde table als forignkey

            com.CommandText = insertkontaktdaten;
            com.ExecuteNonQuery();//für kontaktdaten
            com.CommandText = kontaktquery;

            MySqlDataReader konreader = com.ExecuteReader();//lesen adressen table +sorteiern desc

            string konid = "";       //adressid was wir abgeholen
            if (konreader.Read())        // hol der adress Id ab
            {
                konid = konreader[0].ToString();
            }
            konreader.Close();
            //3

            DateTime dateTimeVariable = DateTime.Now;
            string date = dateTimeVariable.ToString("yyyy-MM-dd H:mm:ss");


            string insertkunde = $"insert into t_kunde values(null,'{date}',{konid},'{k1.Vorname}','{k1.Nachname}',{adresid})";

            com.CommandText = insertkunde;
            com.ExecuteNonQuery();

            
            MessageBox.Show("User succesfully registered");

            con.Close();

                    #endregion
                    Close();
                }
                catch (Exception)
                {

                    MessageBox.Show("***********");
                }
            }
            
        }

        // Neue Kunde in Liste anlegen
        
        private void kundeAnlegenInListe()
        {
            bool close = true;

            try
            {
               
                    //Kunde Neu Anlegen

                    //Prüfung Lieferanschrift = Anschrift
                    if (CB_Lieferanschrift.IsChecked == true)
                    {
                        //wenn abfrage true dann mit Lieferanschrift
                        Adresse Lieferanschrift = new Adresse(TB_Strasse_la.Text, TB_Hausnummer_la.Text, TB_PLZ_la.Text, TB_Ort_la.Text);
                        Kundenliste.Add(new Kunde(TB_Vname.Text, TB_Nname.Text, TB_Geburtsdatum.Text, Lieferanschrift, TB_Strasse.Text, TB_PLZ.Text, TB_Ort.Text, TB_Hausnummer.Text, TB_mail.Text, TB_Telefon.Text));
                    }

                    else
                    {
                        //Wenn ohne Lieferanschrift
                        Kundenliste.Add(new Kunde(TB_Vname.Text, TB_Nname.Text, TB_Geburtsdatum.Text, TB_Strasse.Text, TB_PLZ.Text, TB_Ort.Text, TB_Hausnummer.Text, TB_mail.Text, TB_Telefon.Text));
                    MessageBox.Show("user added");
                    
               

                   
                if (close)
                {

                    this.Close();
                }
                    }


            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Alle felder müssen ausgefüllt werden!");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Bitte Grund auswählen!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Falsches Datumsformat.");
            }
            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show("Falsches Datumsformat.");
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            laden_speichern ls = new laden_speichern();
            ls.KundenSpeichern(Kundenliste);


        }

    }
}

