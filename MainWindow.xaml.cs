using klassen_Bibliothek;
using MySql.Data.MySqlClient;
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

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace ProjektNeu
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
 
    public partial class MainWindow : Window
    {
        List<Kunde> Kundenliste = new List<Kunde>();
        int index;
        Kunde k1 = new Kunde();
        string connstring = "Server=localhost;Database=team1;Uid=root;Convert Zero Datetime=True;";
        public bool useSQL = true;
        public MainWindow()
        {
            InitializeComponent();

            DG_Update();
            #region später Arbeit
            //if(cb_sqlusing.IsChecked == true)
            //{
            //    useSQL = true;
            //}
            //else
            //{
            //    useSQL= false;
            //}
            #endregion
        }
        //DG_Allekunden aktualisieren
        public void DG_Update()
        {
            
            try
            {
 // SQL queries benutzen für Data Grid zu aktualisieren
                string query;
                MySqlConnection con = new MySqlConnection(connstring);
                con.Open();
                DataTable dt = new DataTable();

                query = ("select t_kunde.KundenID,t_kunde.Vorname,t_kunde.Nachname , " +
                         " t_kunde.Geburtsdatum ,t_kontaktdaten.Email,t_kontaktdaten.Telefon ," +
                         " t_adresse.Strasse,t_adresse.Hausnummer,t_adresse.PLZ,t_adresse.Ort" +
                         " from t_kunde " +
                          " INNER JOIN t_kontaktdaten on t_kunde.KontaktdatenID = t_kontaktdaten.KontaktdatenID " +
                          " INNER JOIN t_adresse on t_kunde.AdressenID = t_adresse.AdressenID");

                MySqlCommand com = new MySqlCommand(query, con);
                MySqlDataAdapter da = new MySqlDataAdapter(com);


                da.Fill(dt);


                DG_Allekunden.ItemsSource = dt.DefaultView;
                //fenster_k_bearbeiten fb = new fenster_k_bearbeiten();
                //fb.DG_kunde.ItemsSource = DG_Allekunden.ItemsSource;

                con.Close();
            }
            catch (Exception)
            {

                //MessageBox.Show("DEBUG: in MAIN DG_Update: no SQL ");
                useSQL = false;            //Parameter prüft connection und Data Arbeit bei Database oder List
            }

            if (useSQL == false)          //Kein SQL connection, arbeit in Liste
            {
                laden_speichern ls = new laden_speichern();
                Kundenliste = ls.KundenLaden();
                DG_Allekunden.ItemsSource = Kundenliste;
            }
        }
        // Exit the program
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Button prüft ob die Connection ist mit SQL oder ohne (in Listen arbeiten)
        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(connstring);

            try
            {
                con.Open();
            }
            catch (Exception)
            {
                useSQL = false;
            }


            if (con.State == ConnectionState.Open)
            {
                MessageBox.Show("SQL Data Base connected !!");
                useSQL = true;
            }
            else
            {

                MessageBox.Show("XML List connected !!");
                useSQL = false;
            }
            con.Close();
            DG_Update();
        }



        // Button Alle Kunden ; zeigt alle Kunden und aktualisiert dér Data Grid
        private void btn_insert_Click(object sender, RoutedEventArgs e)
        {

            if (useSQL == true)
            {

                DG_Update();
            }
            else
            {
                DG_Allekunden.ItemsSource = null;

                DG_Allekunden.ItemsSource = Kundenliste;
            }

        }



        // Button für Kunden in SQL oder Listen anlegen
        private void Bt_kd_anlegen_Click(object sender, RoutedEventArgs e)
        {
                                                                                                       //fenster erstellen
            fenster_k_anlegen kf = new fenster_k_anlegen(kunde: k1, useSQL: useSQL);
                                                                                                      //fenster öffnen
            kf.ShowDialog();
                                                                                                     //data grid aktulallisieren

                DG_Update();
            
        }


        // Delete from SQL Database oder Liste
        private void delete_Click(object sender, RoutedEventArgs e)
        {

            if (useSQL == false)
            {
                index = DG_Allekunden.SelectedIndex;

                if (DG_Allekunden.SelectedIndex >= 0 && Kundenliste != null)
                {
                    Kundenliste.RemoveAt(DG_Allekunden.SelectedIndex);
                    laden_speichern ls = new laden_speichern();
                    ls.KundenSpeichern(Kundenliste);
                }
                else
                {
                    MessageBox.Show("Bitte zuerst Kunde Auswählen");
                }
                //Datengrid Aktualisieren

                DG_Allekunden.ItemsSource = null;
                DG_Allekunden.ItemsSource = Kundenliste;

            }
            else // SQL verbindet
            { 
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=Team1;Uid=root;");    //connection bauen
            con.Open();                                                                               //connection öffnen
            string query = $"select * from t_kunde where KundenID={((DataRowView)DG_Allekunden.SelectedItem).Row.ItemArray[0]}";     //gibt alle Daten die in t_kunde sind und vergleicht der Primary key mit den selected rows erste stelle wo normalerweise kommt der primary key 
            MySqlCommand com = new MySqlCommand(query);                                               //select query in command
            com.Connection = con;                                                                     //command ist connected mit unsere connected database
            MySqlDataReader reader = com.ExecuteReader();                                             // lesen alle daten von t_kunde

                //leere strings ; die reservieren platz für :
            string kid = "";                              //KundenId
            string konid = "";                            //KontaktdatenID
            string adid = "";                             //AdressID



            if (reader.Read())
            {
                kid = reader[0].ToString();              // er holt nur stelle 0 von table t_kunde , stelle 0 ist KundenID
                konid = reader[2].ToString();           //er holt nur stelle 2  von table t_kunde, stelle 2 isr kontaktdatenID
                    adid = reader[5].ToString();        // er holt nur stelle 5 von table t_kunde , stelle 0 ist AdressId

                }
                reader.Close();                        //close executreader


                // Bei Delete process muss der delete von mehr als eine table passiert; weil t_kunde hat 2 foreign key(kontaktdatenid,adressid)
                //alle daten die verbinden mit t_kunde muss auch löschen
                //1. löschen alle Kunden Termin
                //2.löschen t_kunde table 
                //3.löschen alle kunden Kontaktdaten von t_kontaktdaten
                //4.und dann löschen kunden Adress daten von t_Adress

            com.CommandText = $"delete from t_termin where KundenID ={kid}";    //delete alle Kunden Termine before t_kunde table zu löschen



            if (com.ExecuteNonQuery() > 0   //
)
            {

                MessageBox.Show(" Successfuly Termine deleted");
            }


            com.CommandText = $"delete from t_kunde where KundenID ={kid}";    


            if (com.ExecuteNonQuery() > 0
)
            {

                MessageBox.Show(" Successfuly!! Kunde is deleted");
            }
            try
            {

                com.CommandText = $"delete from t_kontaktdaten where KontaktdatenID ={konid}";                //alle kunden kontaktdaten löschen
                if (com.ExecuteNonQuery() > 0
    )
                {

                    MessageBox.Show(" Successfuly Kunde Kontakdaten deleted");                        
                }


            }
            catch (Exception)
            {


            }



            com.CommandText = $"delete from  t_adresse where AdressenID={adid}";                   //kunde adress löschen
            com.ExecuteNonQuery();



            if (com.ExecuteNonQuery() > 0
)
            {

                MessageBox.Show(" Successfuly Kunde Adress deleted");
            }


            DG_Update();
            con.Close();


            }
        }


        // Brief erstellen mit Data Base connection
        private void BT_brief_erst_Click(object sender, RoutedEventArgs e)
        {
            index = DG_Allekunden.SelectedIndex;

            if (index < 0)
            {
                MessageBox.Show("Kein Kunde gewählt. Bitte wählen Sie einen Kunden aus. ");
            }


            else
            {
                if (useSQL == false)
                {

                // Fenster erstellen
                PDF_Briefe pdf = new PDF_Briefe(Kundenliste[index], useSQL);
                //Fenster öffnen
                pdf.ShowDialog();


                }
                else
                {
                    // Fenster erstellen
                    PDF_Briefe pdf = new PDF_Briefe(DG_Allekunden.SelectedItem, useSQL);
                    //Fenster öffnen
                    pdf.ShowDialog();

                }
            }
        }


        //Änderung die Data in Data Base
        private void bt_ändern_Click(object sender, RoutedEventArgs e)
        {
            if (DG_Allekunden.SelectedItem == null)
            {
                MessageBox.Show("Bitte, wählen Sie einen Eintrag aus!");

            }
            else
            {

                // Alle selected Data in Anlegen Fenster legen
                if (useSQL == true)
                {



                    DataRowView data = ((DataRowView)DG_Allekunden.SelectedItem);
                    index = -1;

                    fenster_k_aenderen kf = new fenster_k_aenderen(kunde: k1, data: data, index: index, useSQL: useSQL);
                    kf.ShowDialog();
                    //MessageBox.Show("DEBUG:in MAIN: after kf show ");
                    //MessageBox.Show("DEBUG:in MAIN: before DG_Update ");

                    //MessageBox.Show("DEBUG:in MAIN: after DG_Update ");
                }

                else
                {

                    index = DG_Allekunden.SelectedIndex;
                    fenster_k_aenderen kf = new fenster_k_aenderen(kunde: k1, data: null, index: index, useSQL: useSQL);
                    kf.ShowDialog();


                }

                DG_Update();

            }
        }
        private void DG_Allekunden_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {

        }
    }
}

