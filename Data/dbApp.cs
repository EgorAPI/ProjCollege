using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Launcher0._2.Classes;
using Launcher0._2.Models;
using MySql.Data.MySqlClient;
using Launcher0._2.Data;

namespace Launcher0._2.Data
{
    public class dbApp
    {
        DataBaseConnection conn = null;

        //Получение 1-го App из БД по id
        public async Task<Apps> GetApp(int id)
        {
            conn = new DataBaseConnection();

            string query = $"SELECT Apps.ID, AppCategory_id,Description,Apps.Photo, Author_id,Apps.DateOfCreated,Path, NameApp FROM `Apps` where Apps.ID = {id}";

            Apps app = new Apps();

            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                using (MySqlDataReader reader = (MySqlDataReader)await comm.ExecuteReaderAsync())
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            app.ID = Convert.ToInt32(reader["ID"]);
                            app.NameApp = reader["NameApp"].ToString();
                            app.Description = reader["Description"].ToString();
                            app.Photo = reader["Photo"].ToString();
                            app.Path = reader["Path"].ToString();
                            app.DateOfCreated = (DateTime)reader["DateOfCreated"];
                            app.Author_id = Convert.ToInt32(reader["Author_id"]);
                            app.AppCategory_id = Convert.ToInt32(reader["AppCategory_id"]);
                        }
                    }
                conn.connClose();
                return app;
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //Получение всех Apps из БД
        public async Task<List<Apps>> GetApps()
        {
            conn = new DataBaseConnection();

            string query = $"select * from GetApps";

            List<Apps> apps;

            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                using (MySqlDataReader reader = (MySqlDataReader)await comm.ExecuteReaderAsync())
                    if (reader.HasRows)
                    {
                        apps = new List<Apps>();
                        while (await reader.ReadAsync())
                        {
                            Apps app = new Apps();
                            app.ID = Convert.ToInt32(reader["ID"]);
                            app.NameApp = reader["NameApp"].ToString();
                            app.Description = reader["Description"].ToString();
                            app.Photo = reader["Photo"].ToString();
                            app.Path = reader["Path"].ToString();
                            app.DateOfCreated = (DateTime)reader["DateOfCreated"];
                            app.Author_id = Convert.ToInt32(reader["Author_id"]);
                            app.AppCategory_id = Convert.ToInt32(reader["AppCategory_id"]);
                            app.AppCategory = reader["AppCategory"].ToString();
                            app.Author = reader["Author"].ToString();
                            apps.Add(app);
                        }

                        conn.connClose();
                        return apps;
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

        //Добавление Apps
        public async Task<int> InsertApp(Apps app)
        {
            conn = new DataBaseConnection();

            string query = "insert into Apps (NameApp, Description, Photo, AppCategory_id, Author_id) values (@nameapp, @description, @photo, @category, @author)";
            int res = 0;
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@nameapp", app.NameApp);
                    comm.Parameters.AddWithValue("@description", app.Description);
                    comm.Parameters.AddWithValue("@photo", app.Photo);
                    comm.Parameters.AddWithValue("@category", app.AppCategory_id);
                    comm.Parameters.AddWithValue("@author", app.Author_id);

                    res = await comm.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }

            conn.connClose();
            return res;
        }

        //Удаление App из БД по id
        public async Task<int> DeleteApp(int id)
        {
            conn = new DataBaseConnection();

            string query = $"delete from Apps where ID = {id}";
            int res = 0;
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                    res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
                if (res != 0)
                {
                    MessageBox.Show("Приложение удалено!");
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }

        //Изменение(обновление) App
        public async Task<int> UpdateApp(Apps app)
        {
            conn = new DataBaseConnection();

            string query = $"update Apps set NameApp = @nameapp, Description = @description, Photo = @photo, AppCategory_id = @category where ID = {app.ID}";
            int res = 0;
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@nameapp", app.NameApp);
                    comm.Parameters.AddWithValue("@description", app.Description);
                    comm.Parameters.AddWithValue("@photo", app.Photo);
                    comm.Parameters.AddWithValue("@category", app.AppCategory_id);

                    res = await comm.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.connClose();
            return res;
        }
    }
}
