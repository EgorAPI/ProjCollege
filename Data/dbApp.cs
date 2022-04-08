using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Launcher0._2.Classes;
using Launcher0._2.Models;
using Launcher0._2.Data;

namespace Launcher0._2.Data
{
    public class dbApp
    {
        DataBase conn = new DataBase();

        //Получение 1-го приложения из БД по id
        public async Task<Apps> GetApp(int id)
        {
            string query = $"select * from Apps where ID = {id}";

            Apps app = new Apps();

            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                dbUser user = new dbUser();
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
                        app.Author_id = await user.GetUser(Convert.ToInt32(reader["Author_id"]));
                        app.AppCategory_id = Convert.ToInt32(reader["AppCategory_id"]);
                    }
                    conn.connClose();
                }
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
            conn.connClose();
            return app;
        }

        //Получение всех пользователей из БД
        public async Task<List<Apps>> GetApps()
        {
            string query = $"select * from Users";

            List<Apps> apps;

            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                dbUser user = new dbUser();
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
                        app.Author_id = await user.GetUser(Convert.ToInt32(reader["Author_id"]));
                        app.AppCategory_id = Convert.ToInt32(reader["AppCategory_id"]);
                        apps.Add(app);
                    }
                    conn.connClose();
                    return apps;
                }
                else
                {
                    conn.connClose();
                    return null;
                }
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //Добавление пользователя
        public async Task<int> InsertApp(Apps app)
        {
            string query = "insert into Apps (NameApp, Description, Photo) values (@nameapp, @description, @photo)";
            int res = 0;
            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                comm.Parameters.AddWithValue("@nameapp", app.NameApp);
                comm.Parameters.AddWithValue("@description", app.Description);
                comm.Parameters.AddWithValue("@photo", app.Photo);

                res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return res;
        }

        //Удаление пользователя(аккаунта) из БД по id
        public async Task<int> DeleteApp(int id)
        {
            string query = $"delete from Apps where ID = {id}";
            int res = 0;
            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
                if (res != 0)
                {
                    MessageBox.Show("Приложение удалено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }

        //Изменение(обновление) пользователя
        public async Task<int> UpdateApp(Apps app)
        {
            string query = $"update Apps set NameApp = @nameapp, Description = @description, Photo = @photo where ID = {app.ID}";
            int res = 0;
            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                comm.Parameters.AddWithValue("@nameapp", app.NameApp);
                comm.Parameters.AddWithValue("@description", app.Description);
                comm.Parameters.AddWithValue("@photo", app.Photo);

                res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return res;
        }
    }
}
