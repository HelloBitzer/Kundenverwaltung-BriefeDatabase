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
using System.Windows.Shapes;

namespace ProjektNeu
{
    /// <summary>
    /// Interaktionslogik für fenster_k_bearbeiten.xaml
    /// </summary>
    public partial class fenster_k_bearbeiten : Window
    {
        List<Kunde> Kundenliste;
        int index;
       
        string connstring = "Server=localhost;Database=team1;Uid=root;Convert Zero Datetime=True;";

        public Kunde k1 ;



        public fenster_k_bearbeiten()
        {
            InitializeComponent();
           

            laden_speichern ls = new laden_speichern();
            Kundenliste = ls.KundenLaden();
           this.Kundenliste = Kundenliste;
          
            Kunde kunde;
            DG_kunde.ItemsSource = null;
            if (Kundenliste.Count < 1)
            {
                MessageBox.Show("kein kunden in data grid");
            }
            else
            {

                DG_kunde.ItemsSource = Kundenliste;
            }
        }



        // anlegem in bearbeiten fenster

        private void BT_aendern_Click(object sender, RoutedEventArgs e)
        {

            if (DG_kunde.SelectedIndex < 0)
            {
                MessageBox.Show("Kein Kunde gewählt, Maske zur neuanlage wird geöffnet.");
               
            }

            //Fenster erstellen
           fenster_k_aenderen fa = new fenster_k_aenderen (kunde : k1, data:null, index:-1 , useSQL:false) ;
            //Fenster öffnen
            fa.ShowDialog();

            
            DG_kunde.ItemsSource = null;
            DG_kunde.ItemsSource = Kundenliste;


        }




        private void BT_löschen_Click(object sender, RoutedEventArgs e)
        {

            if (DG_kunde.SelectedIndex >= 0 && Kundenliste != null)
            {
                Kundenliste.RemoveAt(DG_kunde.SelectedIndex);
                laden_speichern ls = new laden_speichern();
                ls.KundenSpeichern(Kundenliste);
            }
            else
            {
                MessageBox.Show("Bitte zuerst Kunde Auswählen");
            }
            //Datengrid Aktualisieren

            DG_kunde.ItemsSource = null;
            DG_kunde.ItemsSource = Kundenliste;




        }



        public void BT_brief_erst_Click(object sender, RoutedEventArgs e)
        {
            //if (DG_kunde.SelectedIndex < 0)
            //{
            //    MessageBox.Show("Kein Kunde gewählt. Bitte wählen Sie einen Kunden aus. ");
            //}


            //else
            //{
            //    // Fenster erstellen
            //    PDF_Briefe pdf = new PDF_Briefe(Kundenliste, DG_kunde.SelectedIndex);
            //    //Fenster öffnen
            //    pdf.ShowDialog();
            //}
        }

        

       
    }
}
