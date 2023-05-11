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
    /// Interaktionslogik für Bearbeiten.xaml
    /// </summary>
    public partial class Bearbeiten : Window
    {
        int db_id = 0;
        public Bearbeiten()
        {
            InitializeComponent();

            //Datenbankverbindung wird aufgebaut
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());
            try
            {
                conn.Open();

                //SQL Abfrage nach Menge, Produkt und Bemerkung
                string sql = "SELECT Menge, Produkt, Bemerkung FROM einkaufen";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    bestehende_eintraege.Items.Add(rdr[0].ToString() + " " + rdr[1].ToString()+" "+ rdr[2].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

        }

        private void bestehende_eintraege_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("clicked");
        }

        private void auswaehlen(object sender, RoutedEventArgs e)
        {

         string gewaehlte_listbox = bestehende_eintraege.SelectedItem.ToString();
         string[] listbox_daten = gewaehlte_listbox.ToString().Split(" ");
         Menge.Text = listbox_daten[0];
         Produkt.Text = listbox_daten[1];
         Bezeichnung.Text = listbox_daten[2];

            //Datenbankverbindung wird aufgebaut
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());
            conn.Open();

            //SQL Abfrage nach ID
            string sql = "Select Id from einkaufen where Menge = " + Convert.ToInt32(listbox_daten[0]) + " AND Produkt LIKE '" + listbox_daten[1] + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            object result = cmd.ExecuteScalar();

            if (result != null)
            {
                db_id = Convert.ToInt32(result);
            }
            conn.Close();
        }

        private void update(object sender, RoutedEventArgs e)
        {
            //Datenbankverbindung wird aufgebaut
            MySqlConnection conn1 = new MySqlConnection(Db_connection.DB_Connection());
            conn1.Open();

            //Datenbank eintrag der vorher bearbeitet wurde, wird in die DB gespeichert 
            string sql1 = "Update einkaufen set Menge=" + Convert.ToInt32(Menge.Text) + ", Produkt='" + Produkt.Text + "', Bemerkung='" + Bezeichnung.Text + "' where id="+ db_id + "";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn1);
            object result1 = cmd1.ExecuteScalar();

            //Datenbankverbindung schließen
            conn1.Close();
            MessageBox.Show("Das Bearbeitete Produkt wurde erfolgreich Bearbeitet");
        }
    }
}
