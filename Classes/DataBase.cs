using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Launcher0._2.Classes
{
    public class DataBase
    {
        private const string connectionString = "Server=EGORSHARP;Database=Venera;Trusted_Connection=True";

        SqlConnection connection = new SqlConnection(connectionString);

        public SqlConnection connOpen()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        public void connClose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
