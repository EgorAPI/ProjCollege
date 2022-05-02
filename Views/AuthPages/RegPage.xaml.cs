using Launcher0._2.Classes;
using Launcher0._2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher0._2.Data;

namespace Launcher0._2
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        int code = 0;
        byte state = 0;
        bool validPas = false;
        bool validEmail = false;
        private async void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validPas == true && validEmail == true)
                {


                    dbUser dbuser = new dbUser();

                    if (state == 1)
                    {
                        if (CodeTB.Text == code.ToString())
                        {
                            code = 0;
                            User user = new User()
                            {
                                Email = TBEmail.Text,
                                UserName = TBName.Text,
                                Password = new HashBuilder().GetHash(PBPass.Password.ToString())
                            };

                            int res = await dbuser.InsertUser(user);

                            if (res == 1) { MessageBox.Show("Успешно!"); AuthWindow.authWin.AuthFrame.GoBack(); }
                            else { MessageBox.Show("Что-то пошло не так:("); }
                        }
                        else
                        {
                            tbError.Text = "Неверный код!";
                        }
                    }

                    if (state == 0)
                    {

                        if (dbuser.CheckUserEmail_in_DB(TBEmail.Text) == true || PBPass.Password == "" || TBName.Text == "")
                        {
                            MessageBox.Show("Пользователь с таким Email уже существует!\nили данные не корректны");
                        }
                        else
                        {
                            code = new Random().Next(1000, 9000);

                            EmailSender mailsender = new EmailSender();
                            await mailsender.SendMailAsync(TBEmail.Text, code);

                            state = 1;

                            RegGrid.Visibility = Visibility.Hidden;
                            CodeGrid.Visibility = Visibility.Visible;
                            RegBtn.Content = "Отправить";
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TBEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var valid = new ValidDataCheck();

            if (valid.EmailCheck(TBEmail.Text) == true)
            {
                rectIndicator.Fill = Brushes.Green;
                validEmail = true;
            }
            else { rectIndicator.Fill = Brushes.Red; validEmail = false; }
                
        }

        private void PBPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var valid = new ValidDataCheck();

            if (valid.PassCheck(PBPass.Password) == true)
            {
                rectIndicator1.Fill = Brushes.Green;
                validPas = true;

            }
            else { rectIndicator1.Fill = Brushes.Red; validPas = false; }
        }
    }
}
