using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;

namespace Einkaufsliste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Bearbeiten(object sender, RoutedEventArgs e)
        {
            //Neues Fenster für Bearbeiten Modus
            Window bearbeiten = new Bearbeiten();
            bearbeiten.Owner = this;
            bearbeiten.ShowDialog();
        }

        private void Hinzufügen(object sender, RoutedEventArgs e)
        {
            //es wird ein neues WPF Fenster erzeugt 
            Window hinzufügen = new Hinzufügen();
            hinzufügen.Owner = this;
            hinzufügen.ShowDialog();
        }

        private void Senden(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());
            try
            {
                conn.Open();

                //SQL Statement von allem außer Id
                string sql = "SELECT Menge, Produkt, Bemerkung FROM einkaufen";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                //Der Pfad zu der Text Datei
                StreamWriter sw = new StreamWriter("D:\\test.txt");

                //1. Zeile Text
                sw.WriteLine("Das ist meine Einkaufsliste :)");
                while (rdr.Read())
                {
                    //Text Datei befüllen
                    sw.WriteLine("Menge: " + rdr[0].ToString()+ " Produkt: "+ rdr[1].ToString()+" Bemerkung: "+ rdr[2].ToString());
                      
                }

                sw.WriteLine("Ende :)");
                sw.Close();
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            //mit Gmail keine Funktionalität => zu hohe Sicherheitsfeatures man kann die funktion nicht mehr deaktivieren
            SmtpClient email = new SmtpClient();
            email.Host = "smtp.gmail.com";
            email.Credentials = new NetworkCredential("von@test.com", "Passwort");

            MailMessage nachricht = new MailMessage("von@test.com", "nach@test.com");
            nachricht.Subject = "Einkaufsliste";
            nachricht.Body = "Das ist meine Einkaufsliste";
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment("D:\\test.txt");
            nachricht.Attachments.Add(attachment);
            email.Send(nachricht);
            MessageBox.Show("Die Liste wurde erfolgreich versendet");
        }

        private void Löschen(object sender, RoutedEventArgs e)
        {
            //es wird ein neues WPF Fenster erzeugt 
            Window löschen = new Löschen();
            löschen.Owner = this;
            löschen.ShowDialog();
        }
    }
}
