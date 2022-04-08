using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher0._2.Classes;
using Launcher0._2.Models;
using System.Windows;

namespace Launcher0._2.Data
{
    public class dbUser
    {
        private DataBase conn = new DataBase();

        //Получение 1-го пользователя из БД по id
        public async Task<User> GetUser(int id)
        {
            string query = $"select * from Users where ID = {id}";

            User user = new User();

            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        user.ID = Convert.ToInt32(reader["ID"]);
                        user.UserName = reader["UserName"].ToString();
                        user.Password = reader["Password"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.Phone = reader["Phone"].ToString();
                        user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                        user.DateOfBirth = (DateTime)reader["DateOfBirth"];
                        user.UserStatus_id =Convert.ToInt32(reader["UserStatus_id"]);
                    }
                }
                conn.connClose();
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
            conn.connClose();
            return user;
        }

        //Получение 1-го пользователя из БД по Email and Password
        public async Task<User> GetUser(string email, string password)
        {
            string query = $"select * from Users where Email = @email and Password = @password";

            User user = new User();

            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                comm.Parameters.AddWithValue("@email", email);
                comm.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        user.ID = Convert.ToInt32(reader["ID"]);
                        user.UserName = reader["UserName"].ToString();
                        user.Password = reader["Password"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.Phone = reader["Phone"].ToString();
                        user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                        user.DateOfBirth = (DateTime)reader["DateOfBirth"];
                        user.UserStatus_id = Convert.ToInt32(reader["UserStatus_id"]);
                    }
                }
                conn.connClose();
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
            conn.connClose();
            return user;
        }

        //Получение всех пользователей из БД
        public async Task<List<User>> GetUsers()
        {
            string query = $"select * from Users";

            List<User> users;

            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    users = new List<User>();
                    while (await reader.ReadAsync())
                    {
                        User user = new User();
                        user.ID = Convert.ToInt32(reader["ID"]);
                        user.UserName = reader["UserName"].ToString();
                        user.Password = reader["Password"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.Phone = reader["Phone"].ToString();
                        user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                        user.DateOfBirth = (DateTime)reader["DateOfBirth"];
                        user.UserStatus_id = Convert.ToInt32(reader["UserStatus_id"]);
                        users.Add(user);
                    }
                    conn.connClose();
                    return users;
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
        public async Task<int> InsertUser(User user)
        {
            string query = "insert into Users (UserName, Password, Email) values (@username, @password, @email)";
            int res = 0;
            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                comm.Parameters.AddWithValue("@username", user.UserName);
                comm.Parameters.AddWithValue("@password", user.Password);
                comm.Parameters.AddWithValue("@email", user.Email);

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
        public async Task<int> DeleteUser(int id)
        {
            string query = $"delete from Users where ID = {id}";
            int res = 3;
            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
                if (res !=0)
                {
                    MessageBox.Show("Аккаунт был удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }

        //Изменение(обновление) пользователя
        public async Task<int> UpdateUser(User user)
        {
            string query = $"update Users set UserName = @username, Password = @password, Email = @email, DateOfBirth = '{user.DateOfBirth}' where ID = {user.ID}";
            int res = 0;
            try
            {
                SqlCommand comm = new SqlCommand(query, conn.connOpen());
                comm.Parameters.AddWithValue("@username", user.UserName);
                comm.Parameters.AddWithValue("@password", user.Password);
                comm.Parameters.AddWithValue("@email", user.Email);

                res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return res;
        }

        //Проверка наличия пользователя в бд по Email
        public async Task<bool> CheckUserEmail_in_DB(string email)
        {
            string comm1 = "select * from Users where Email = @email";

            SqlCommand command = new SqlCommand(comm1, conn.connOpen());
            command.Parameters.AddWithValue("email", email);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            conn.connClose();

            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Проверка наличия пользователя в бд по Email и Паролю
        public async Task<bool> CheckUserEmailandPassword_in_DB(string email, string password)
        {
            string comm1 = "select * from Users where Email = @email and Password = @password";

            SqlCommand command = new SqlCommand(comm1, conn.connOpen());
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                conn.connClose();
                return true;
            }
            else
            {
                conn.connClose();
                return false;
            }
        }
    }
}
