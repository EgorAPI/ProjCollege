using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Launcher0._2.Classes
{
    public class DataBaseConnection
    {
        private const string connectionString = @"Server=31.31.196.27,2a00:f940:2:2:1:1:0:30;Database=u1621366_venera;User ID=u1621366_egor;Password=cD8tH5dW0b;charset= utf8";

        MySqlConnection connection = new MySqlConnection(connectionString);

        public MySqlConnection connOpen()
        {
            if (connection.State.ToString() == "Closed")
            {
                connection.Open();
            }
            return connection;
        }

        public void connClose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
