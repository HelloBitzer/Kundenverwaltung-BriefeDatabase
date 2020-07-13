using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using klassen_Bibliothek;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Diagnostics;
using Image = iText.Layout.Element.Image;
using iText.IO.Image;
using System.IO;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjektNeu
{
    /// <summary>
    /// Interaktionslogik für PDF_Briefe.xaml
    /// </summary>
    public partial class PDF_Briefe : Window
    {
        //Kundenliste 

        //List<Kunde> Kundenliste;
        //int index;
        public bool SQLuse;
       public  Kunde kundendaten = new Kunde();

        public PDF_Briefe(object kunde,  bool useSQL)
        {
            InitializeComponent();

            if (useSQL == false)// in liste
            {
              
                kundendaten=(Kunde) kunde;
                
                TB_vname.Text = kundendaten.Vorname;
                TB_nname.Text = kundendaten.Nachname;

                Adresse Adress = new Adresse();
                TB_strasse.Text = kundendaten.kontaktdaten.Adress.Strasse;
                TB_hsnr.Text = kundendaten.kontaktdaten.Adress.Hausnummer;
                TB_plz.Text = kundendaten.kontaktdaten.Adress.PLZ;
                TB_ort.Text = kundendaten.kontaktdaten.Adress.Ort;

            }
            else
            {

            //Daten aus Database 

            DataRowView data = (DataRowView)kunde;
            kundendaten.Vorname = (string)data.Row.ItemArray[1];
            kundendaten.Nachname = (string)data.Row.ItemArray[2];
            kundendaten.geburtsdatum = (DateTime)data.Row.ItemArray[3];

            kundendaten.kontaktdaten = new KontaktDaten();
            kundendaten.kontaktdaten.Email = (string)data.Row.ItemArray[4];
            kundendaten.kontaktdaten.Telefon = (string)data.Row.ItemArray[5];

            kundendaten.Kontaktdaten.Adress= new Adresse();
            kundendaten.kontaktdaten.Adress.Strasse = (string)data.Row.ItemArray[6];
            kundendaten.kontaktdaten.Adress.Hausnummer = (string)data.Row.ItemArray[7];
            kundendaten.kontaktdaten.Adress.PLZ = (string)data.Row.ItemArray[8];
            kundendaten.kontaktdaten.Adress.Ort = (string)data.Row.ItemArray[9];

            //Kundendaten einfügen
            TB_vname.Text = kundendaten.Vorname;
            TB_nname.Text = kundendaten.Nachname;
            TB_strasse.Text = kundendaten.kontaktdaten.Adress.Strasse;
            TB_hsnr.Text = kundendaten.kontaktdaten.Adress.Hausnummer;
            TB_plz.Text = kundendaten.kontaktdaten.Adress.PLZ;
            TB_ort.Text = kundendaten.kontaktdaten.Adress.Ort;


            }
        }

        //PDF Brief drucken Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Abfrage ob man die PDF speichern möchte
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Files(*.pdf)|*.pdf";
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".pdf";

            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;


                //writer instanzieren
                PdfWriter writer = new PdfWriter(path);

                //pdfdocument erzeugen
                PdfDocument pdf = new PdfDocument(writer);


                //Document erzeugen
                using (Document document = new Document(pdf, PageSize.A4))
                {
                    //Einfügen in das document

                    Image logo = new Image(ImageDataFactory.CreateJpeg(new Uri($"{Directory.GetCurrentDirectory()}\\logo.jpg", UriKind.Absolute)));
                    logo.ScaleAbsolute(180f, 50f);
                    logo.SetFixedPosition(400f, 750f);
                    document.Add(logo);

                    document.Add(new Paragraph(TB_vname.Text + " " + TB_nname.Text).SetFixedPosition(45f, 780f, 800f));
                    document.Add(new Paragraph(TB_strasse.Text + " " + TB_hsnr.Text).SetFixedPosition(45f, 760f, 800f));
                    document.Add(new Paragraph(TB_plz.Text + " " + TB_ort.Text).SetFixedPosition(45f, 740f, 800));


                    document.Add(new Paragraph(Betreff.Text).SetFixedPosition(45f, 600f, 800f));

                    document.Add(new Paragraph(Anrede.Text).SetFixedPosition(90f, 500f, 800f));

                    Image inhalt = new Image(ImageDataFactory.CreateJpeg(new Uri($"{Directory.GetCurrentDirectory()}\\inhalt.jpg", UriKind.Absolute)));
                    inhalt.ScaleAbsolute(400f, 400f);
                    inhalt.SetFixedPosition(70f, 100f);
                    document.Add(inhalt);

                }

                pdf.Close();

                //writer schliesen
                writer.Close();

                //öffnen des documentes
                Process.Start(path);

            }
            else
            {
                
            }

        }
        
    }
}






