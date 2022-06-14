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
using MySql.Data.MySqlClient;

namespace Launcher0._2.Data
{
    public class dbUser
    {
        private DataBaseConnection conn = new DataBaseConnection();

        //Получение 1-го пользователя по id
        public async Task<User> GetUser(int id)
        {
            string query = $"SELECT Users.ID, UserName, Password, Email, DateOfCreated, UserStatus.NameStatus as UserStatus_id from Users LEFT JOIN UserStatus on UserStatus.ID = Users.UserStatus_id where Users.ID = {id}";

            try
            {
                User user = new User();
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                using (MySqlDataReader reader = (MySqlDataReader)await comm.ExecuteReaderAsync())
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            user.ID = Convert.ToInt32(reader["ID"]);
                            user.UserName = reader["UserName"].ToString();
                            user.Password = reader["Password"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                            user.UserStatus_id = reader["UserStatus_id"].ToString();
                        }
                    }
                conn.connClose();
                return user;
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //Получение 1-го пользователя по Email and Password
        public async Task<User> GetUser(string email, string password)
        {
            string query = $"SELECT Users.ID, UserName, Password, Email, DateOfCreated, UserStatus.NameStatus as UserStatus_id from Users LEFT JOIN UserStatus on UserStatus.ID = Users.UserStatus_id where Email = @email and Password = @password";

            try
            {
                User user = new User();
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@email", email);
                    comm.Parameters.AddWithValue("@password", password);
                    using (MySqlDataReader reader = (MySqlDataReader)await comm.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                user.ID = Convert.ToInt32(reader["ID"]);
                                user.UserName = reader["UserName"].ToString();
                                user.Password = reader["Password"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                                user.UserStatus_id = reader["UserStatus_id"].ToString();
                            }
                        }

                        conn.connClose();
                        return user;
                    }
                }

            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<User> GetUser(string email)
        {
            string query = $"SELECT Users.ID, UserName, Password, Email, DateOfCreated, UserStatus.NameStatus as UserStatus_id from Users LEFT JOIN UserStatus on UserStatus.ID = Users.UserStatus_id where Email = @email";

            try
            {
                User user = new User();
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@email", email);
                    using (MySqlDataReader reader = (MySqlDataReader)await comm.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                user.ID = Convert.ToInt32(reader["ID"]);
                                user.UserName = reader["UserName"].ToString();
                                user.Password = reader["Password"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                                user.UserStatus_id = reader["UserStatus_id"].ToString();
                            }
                        }

                        conn.connClose();
                        return user;
                    }
                }

            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //Получение всех пользователей
        public async Task<List<User>> GetUsers()
        {
            string query = $"select * from GetUsers";

            List<User> users;

            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                using (MySqlDataReader reader = (MySqlDataReader)await comm.ExecuteReaderAsync())
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
                            user.DateOfCreated = (DateTime)reader["DateOfCreated"];
                            user.UserStatus_id = reader["UserStatus_id"].ToString();
                            users.Add(user);
                        }

                        conn.connClose();
                        return users;
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

        //Добавление пользователя
        public async Task<int> InsertUser(User user)
        {
            string query = "insert into Users (UserName, Password, Email) values (@username, @password, @email)";
            int res = 0;
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@username", user.UserName);
                    comm.Parameters.AddWithValue("@password", user.Password);
                    comm.Parameters.AddWithValue("@email", user.Email);
                    res = await comm.ExecuteNonQueryAsync();

                    conn.connClose();
                    return res;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.connClose();
            return res;
        }

        //Удаление пользователя по id
        public async Task<int> DeleteUser(int id)
        {
            string query = $"delete from Users where ID = {id}";
            int res = 3;
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                    res = await comm.ExecuteNonQueryAsync();
                conn.connClose();
                if (res !=0)
                {
                    MessageBox.Show("Аккаунт был удален!");
                }
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
            return res;
        }

        //Изменение пользователя
        public async Task<int> UpdateUser(User user)
        {
            string query = $@"update Users set UserName = @username, Password = @password, Email = @email where ID = {user.ID}";
            int res = 0;
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                {
                    comm.Parameters.AddWithValue("@username", user.UserName);
                    comm.Parameters.AddWithValue("@password", user.Password);
                    comm.Parameters.AddWithValue("@email", user.Email);

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

        //Проверка наличия пользователя в бд по Email
        public bool CheckUserEmail_in_DB(string email)
        {
            string comm1 = "select * from Users where Email = @email";

            using (MySqlCommand command = new MySqlCommand(comm1, conn.connOpen()))
            {
                command.Parameters.AddWithValue("email", email);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
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

        //Проверка наличия пользователя в бд по Email и Паролю
        public bool CheckUserEmailandPassword_in_DB(string email, string password)
        {
            string comm1 = "select * from Users where Email = @email and Password = @password";

            using (MySqlCommand command = new MySqlCommand(comm1, conn.connOpen()))
            {
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
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

        //Обновление активности
        public async Task UpdateActivity(int id)
        {
            string query = $"update Users set Activity = '{DateTime.Now}'";
            try
            {
                using (MySqlCommand comm = new MySqlCommand(query, conn.connOpen()))
                await comm.ExecuteNonQueryAsync();
                conn.connClose();
            }
            catch (Exception ex)
            {
                conn.connClose();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
