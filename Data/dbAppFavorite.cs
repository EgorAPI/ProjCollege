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
    public class dbAppFavorite
    {
        DataBaseConnection conn = null;

        //Получить все избранные
        public List<AppFavorite> GetAppsFavorite()
        {
            conn = new DataBaseConnection();

            string query = $"select * from AppFavorite where User_id = {CurrentUser.user.ID}";

            List<AppFavorite> listapps;

            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                using (MySqlDataReader reader = comm.ExecuteReader())
                    if (reader.HasRows)
                    {
                        listapps = new List<AppFavorite>();
                        while (reader.Read())
                        {
                            AppFavorite favoriteapp = new AppFavorite();
                            favoriteapp.ID = Convert.ToInt32(reader["ID"]);
                            favoriteapp.App_id = Convert.ToInt32(reader["App_id"]);
                            favoriteapp.User_id = Convert.ToInt32(reader["User_id"]);
                            listapps.Add(favoriteapp);
                        }

                        conn.connClose();
                        return listapps;
                    }

                conn.connClose();
                return null;
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }

        }

        public async void AddFavorite(int app, int user)
        {
            conn = new DataBaseConnection();

            string query = "insert into AppFavorite (User_id, App_id) values (@User_id, @App_id)";
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@User_id", user);
                    comm.Parameters.AddWithValue("@App_id", app);

                    await comm.ExecuteNonQueryAsync();
                    conn.connClose();
                }
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
        }

        public async void RemoveFavorite(int app, int user)
        {
            conn = new DataBaseConnection();

            string query = "delete from AppFavorite where User_id = @User_id and App_id = @App_id";
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@User_id", user);
                    comm.Parameters.AddWithValue("@App_id", app);

                    await comm.ExecuteNonQueryAsync();
                    conn.connClose();
                }
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
        }

        public List<TopFavorite> GetTopFavorite()
        {
            conn = new DataBaseConnection();

            string query = $"select Apps.NameApp as AppName, Count(AppFavorite.App_id) as FavoriteCount from AppFavorite" +
                $" LEFT JOIN Apps on Apps.ID = AppFavorite.App_id GROUP by AppFavorite.App_id ORDER by AppFavorite.App_id";

            List<TopFavorite> listapps;

            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                using (MySqlDataReader reader = comm.ExecuteReader())
                    if (reader.HasRows)
                    {
                        listapps = new List<TopFavorite>();
                        while (reader.Read())
                        {
                            TopFavorite favoriteapp = new TopFavorite();
                            favoriteapp.AppName = reader["AppName"].ToString();
                            favoriteapp.FavoriteCount = Convert.ToInt32(reader["FavoriteCount"]);
                            listapps.Add(favoriteapp);
                        }

                        conn.connClose();
                        return listapps;
                    }

                conn.connClose();
                return null;
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
    public class TopFavorite 
    {
        public string AppName { get; set; }
        public int FavoriteCount { get; set; }
    }

}
