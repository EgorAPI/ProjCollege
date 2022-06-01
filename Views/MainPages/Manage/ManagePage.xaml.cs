using Launcher0._2.Data;
using Launcher0._2.Models;
using System;
using System.Collections.Generic;
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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Data.Linq;
using Launcher0._2.Classes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Launcher0._2.Views.MainPages.Manage
{
    /// <summary>
    /// Interaction logic for ManagePage.xaml
    /// </summary>
    public partial class ManagePage : Page
    {
        dbUser db = new dbUser();

        public ManagePage()
        {
            InitializeComponent();

            GetUsers();

            //Пользователей всего
            totalUsers = listStaticUser.Count.ToString();
            //Фильтры
            UserInfo();

            //Диагрмма пользователей
            int a = 0;
            var values = new ChartValues<int> { };
            Labels = new List<string>();
            for (int i = 0; i > -7; i--)
            {
                int item = listStaticUser.Where(x => x.DateOfCreated.Date >= DateTime.Now.AddDays(i).Date).Count();
                values.Add(item - a);
                a = item;

                Labels.Add(DateTime.Now.AddDays(i).ToShortDateString());
            }

            SeriesColumn.Add(new ColumnSeries
            {
                Values = values
            });


            DataContext = this;

            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.RemoveBackEntry();
            }
        }
        private async void GetUsers()
        {
            listStaticUser = await db.GetUsers();
        }

        private void UserInfo()
        {
            listQueryUser = listStaticUser;
            if (tbDaysUser.Text != "")
            {
                listQueryUser = listQueryUser.Where(x => x.DateOfCreated.Date > DateTime.Now.AddDays(-Convert.ToInt32(tbDaysUser.Text)).Date).OrderByDescending(x => x.ID).ToList();
            }

            if (tbId.Text != "" || tbUserName.Text != "")
            {
                listQueryUser = listQueryUser.Where(x => x.ID.ToString().StartsWith(tbId.Text)).ToList();
                listQueryUser = (from p in listQueryUser where p.UserName.ToLower().StartsWith(tbUserName.Text.ToLower()) || 
                                 p.Email.ToLower().StartsWith(tbUserName.Text.ToLower()) select p).ToList();
            }

            //результат(по критериям)/всего
            totalUsersFrom = listQueryUser.Count + "/" + totalUsers;
            lvUsers.ItemsSource = listQueryUser;
            tTotalFor.Text = totalUsersFrom;
        }

        public List<User> listStaticUser { get; set; }
        public List<User> listQueryUser { get; set; }
        public string totalUsers { get; set; }
        public string totalUsersFrom { get; set; }
        //Диагрмма столбчатая (пользователи)
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesColumn { get; set; } = new SeriesCollection();


        private void Button_forvard_click(object sender, RoutedEventArgs e)
        {

            FrameManager.mainFrame.Navigate(new ManageTwoPage());
        }

        private void tbId_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserInfo();
        }

        private void tbUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserInfo();
        }

        private void tbDaysUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserInfo();
        }
    }
}
