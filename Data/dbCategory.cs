using Launcher0._2.Classes;
using Launcher0._2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher0._2.Data
{
    public class dbCategory
    {
        DataBaseConnection connection = null;
        public async Task<List<AppCategory>> GetCategories()
        {
            try
            {
                connection = new DataBaseConnection();
                using (MySqlCommand command = new MySqlCommand("select * from AppCategory", connection.connOpen()))
                using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    List<AppCategory> categories = new List<AppCategory>();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            AppCategory category = new AppCategory()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                NameCategory = reader["NameCategory"].ToString(),
                            };

                            categories.Add(category);
                        }

                        connection.connClose();
                        return categories;
                    }
                }
            }
            catch (Exception)
            {
            }
            connection.connClose();
            return null;
        }

        public async Task<int> InsertCategory(string name)
        {
            try
            {
                connection = new DataBaseConnection();
                using (MySqlCommand command = new MySqlCommand("insert into AppCategory (CategoryName) values (@name)", connection.connOpen()))
                {
                    command.Parameters.AddWithValue("@name", name);
                    int res = await command.ExecuteNonQueryAsync();

                    connection.connClose();
                    return res;
                }
            }
            catch (Exception)
            {
            }
            connection.connClose();
            return 0;

        }
    }
}
