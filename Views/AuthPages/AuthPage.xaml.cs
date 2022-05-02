using Launcher0._2.AuthPages;
using Launcher0._2.Classes;
using Launcher0._2.Data;
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

namespace Launcher0._2
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

                PassTB.Password = Properties.Settings.Default.Password.ToString();
                LoginTB.Text = Properties.Settings.Default.Email.ToString();
            toggleRemember.IsChecked = Properties.Settings.Default.Checkmark;
        }

        private async void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            dbUser dbuser = new dbUser();
            string pas = new HashBuilder().GetHash(PassTB.Password);


            if (dbuser.CheckUserEmailandPassword_in_DB(LoginTB.Text, pas))
            {
                if (toggleRemember.IsChecked == true)
                {
                    Properties.Settings.Default.Password = PassTB.Password;
                    Properties.Settings.Default.Email = LoginTB.Text;
                    Properties.Settings.Default.Checkmark = true;
                }
                if (toggleRemember.IsChecked == false)
                {
                    Properties.Settings.Default.Password = "";
                    Properties.Settings.Default.Email = "";
                    Properties.Settings.Default.Checkmark = false;
                }
                Properties.Settings.Default.Save();

                CurrentUser.user = await dbuser.GetUser(LoginTB.Text, pas);

                MainWin mainWin = new MainWin();
                mainWin.Show();
                AuthWindow.authWin.Close();
            }
            else
            {
                MessageBox.Show("Данные не корректны");
            }
        }

        private void ForgotTbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AuthWindow.authWin.AuthFrame.Navigate(new ForgotPassPage());
        }

        private void RegTbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AuthWindow.authWin.AuthFrame.Navigate(new RegPage());
        }
    }
}
