using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

namespace Einkaufsliste
{
    /// <summary>
    /// Interaktionslogik für Hinzufügen.xaml
    /// </summary>
    public partial class Hinzufügen : Window
    {
        public Hinzufügen()
        {
            InitializeComponent();
            //Datenbank Verbindung aufbauen
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());
            try
            {
                conn.Open();

                //SQL Abfrage nach Menge und Produkt
                string sql = "SELECT Menge, Produkt FROM einkaufen";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                //Listbox befüllen mit einträgen aus DB
                while (rdr.Read())
                {
                    bestehende_eintraege.Items.Add(rdr[0].ToString()+ " "+rdr[1].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
             
            conn.Close();
        }

        private void Speichern(object sender, RoutedEventArgs e)
        {
            //Datenbank Verbindung wird aufgebaut
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());

            try
            {
                conn.Open();

                //Einfügen von Datenbankeinträgen 
                string sql = "Insert into einkaufen (Produkt, Menge, Bemerkung) values ('"+Produkt.Text+"',"+ Convert.ToInt32(Menge.Text) + ",'"+ Bezeichnung.Text + "');";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
              test.Content="Das Produkt konnte leider nicht zur Einkaufsliste Hinzugefügt werden.";
            }
            MessageBox.Show("Das Produkt wurde zur Einkaufsliste Hinzugefügt");
            conn.Close();
        }
    }
}
