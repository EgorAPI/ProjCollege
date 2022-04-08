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
using Launcher0._2.Classes;

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
        }

        private async void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            dbUser dbuser = new dbUser();

            if (await dbuser.CheckUserEmailandPassword_in_DB(LoginTB.Text, PassTB.Password))
            {
                CurrentUser user = new CurrentUser();
                user.SetCurrentUser(await dbuser.GetUser(LoginTB.Text, PassTB.Password));

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
