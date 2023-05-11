using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einkaufsliste
{
    internal class Db_connection
    {
        //Datenbank Verbindungsdaten
        public static string DB_Connection()
        {
            string connStr = "server=localhost;user=root;database=einkaufsliste;port=3306;password=test";
            return connStr;
        }

    }
}
