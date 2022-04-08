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

        private async void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            dbUser dbuser = new dbUser();

            if (await dbuser.CheckUserEmail_in_DB(EmailTB.Text) == true)
            {
                MessageBox.Show("Пользователь с таким Email уже существует!");
            }
            else
            {
                User user = new User()
                {
                    Email = EmailTB.Text,
                    UserName = LoginTB.Text,
                    Password = PBPass.Password
                };

                int res = await dbuser.InsertUser(user);

                if (res == 0) { MessageBox.Show("Успешно!"); }
                else { MessageBox.Show("Что-то пошло не так:("); }
            }
        }
    }
}
