using Launcher0._2.Classes;
using Launcher0._2.Data;
using Launcher0._2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

namespace Launcher0._2.AuthPages
{
    /// <summary>
    /// Логика взаимодействия для ForgotPassPage.xaml
    /// </summary>
    public partial class ForgotPassPage : Page
    {
        public ForgotPassPage()
        {
            InitializeComponent();
        }

        int code = 0;
        byte state = 0;

        private async void btnForgot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbUser dbuser = new dbUser();

                if (state == 2)
                {
                    User user = await dbuser.GetUser(TBEmail.Text);
                    user.Password = new HashBuilder().GetHash(TBPas.Text);

                   await dbuser.UpdateUser(user);

                    AuthWindow.authWin.AuthFrame.GoBack();
                }

                if (state == 1)
                {
                    if (CodeTB.Text == code.ToString())
                    {
                        code = 0;

                        state = 2;

                        CodeGrid.Visibility = Visibility.Hidden;
                        PassChangeGrid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbError.Text = "Неверный код!";
                    }
                }

                if (state == 0)
                {
                    if (dbuser.CheckUserEmail_in_DB(TBEmail.Text) != true)
                    {
                        MessageBox.Show("Почта не найдена!");
                    }
                    else
                    {
                        code = new Random().Next(1000, 9000);

                        EmailSender mailsender = new EmailSender();
                        await mailsender.SendMailAsync(TBEmail.Text, code);

                        state = 1;

                        TBEmail.Visibility = Visibility.Hidden;
                        CodeGrid.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
