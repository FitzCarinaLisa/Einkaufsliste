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
    /// Interaktionslogik für Löschen.xaml
    /// </summary>
    public partial class Löschen : Window
    {
        int db_id = 0;
        public Löschen()
        {
            InitializeComponent();
            //
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());
            try
            {
                //Verbindung der DB
                conn.Open();

                //SQL Statement alles außer ID
                string sql = "SELECT Menge, Produkt, Bemerkung FROM einkaufen";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                //Listbox mit Inhalten aus der DB befüllen
                while (rdr.Read())
                {
                    bestehende_eintraege.Items.Add(rdr[0].ToString() + " " + rdr[1].ToString() + " " + rdr[2].ToString());
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //Datenbankverbindung schließen
            conn.Close();
        }

        private void auswaehlen(object sender, RoutedEventArgs e)
        {
            string gewaehlte_listbox = bestehende_eintraege.SelectedItem.ToString();
            string[] listbox_daten = gewaehlte_listbox.ToString().Split(" ");
            int menge = Convert.ToInt32(listbox_daten[0]);
            string produkt = listbox_daten[1];
            string bezeichnung = listbox_daten[2];

            //SQL Verbindung wird aufgebaut
            MySqlConnection conn = new MySqlConnection(Db_connection.DB_Connection());
            conn.Open();

            //Id bekommen damit ich weiß, was aus der Listbox eindeutig gewählt wurde
            string sql = "Select Id from einkaufen where Menge = " + Convert.ToInt32(listbox_daten[0]) + " AND Produkt LIKE '" + listbox_daten[1] + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            object result = cmd.ExecuteScalar();
            
            //db_id soll den Wert der aktuell aus der Listbox gewählen db haben
            if (result != null)
            {
                db_id = Convert.ToInt32(result);
            }

            conn.Close();
        }
        private void loeschen(object sender, RoutedEventArgs e)
        {
            //Datenbank Verbindung wird aufgebaut
            MySqlConnection conn1 = new MySqlConnection(Db_connection.DB_Connection());
            conn1.Open();

            //Es werden nur die Einträge gelöscht von der gewählten ID mit deren Daten
            string sql1 = "Delete from einkaufen where id=" + db_id + "";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn1);
            object result1 = cmd1.ExecuteScalar();

            //Datenbank Verbindung schließen
            conn1.Close();
            MessageBox.Show("Das Bearbeitete Produkt wurde erfolgreich gelöscht");
        }

        private void alles_loeschen(object sender, RoutedEventArgs e)
        {
            //Datenbankverbindung aufbauen
            MySqlConnection conn1 = new MySqlConnection(Db_connection.DB_Connection());
            conn1.Open();

            //Alle einträge aus der Db sollen gelöscht werden nur die Spalten namen bleiben vorhanden
            string sql1 = "Delete  from einkaufen ";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn1);
            object result1 = cmd1.ExecuteScalar();

            //Datenbank Verbindung Schließen
            conn1.Close();
            MessageBox.Show("Das Bearbeitete Produkt wurde erfolgreich gelöscht");
        }
    }
}
