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
using Launcher0._2.Classes;
using Launcher0._2.Data;
using Launcher0._2.Models;
using System.Collections.Specialized;
using System.Net;

namespace Launcher0._2.Views.MainPages.Manage
{
    /// <summary>
    /// Interaction logic for ManageTwoPage.xaml
    /// </summary>
    public partial class ManageTwoPage : Page
    {
        public ManageTwoPage()
        {
            InitializeComponent();

            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.RemoveBackEntry();
            }

            tbTop5.GotFocus += (s, a) => { lvTop5.Visibility = Visibility.Visible; };

            AppLoad();
            totalApps = listStaticApps.Count.ToString();

            AppInfo();

            string[] topmass = { "Топ 5 скачиваемых", "Топ 5 желаемых" };
            lvTop5.ItemsSource = topmass;
            lvTop5.SelectedIndex = 0;

            DataContext = this;
        }

        dbApp app = new dbApp();
        dbAppFavorite appFavorite = new dbAppFavorite();

        private async void AppLoad()
        {
            listStaticApps = await app.GetApps();
            listFavoriteApps = appFavorite.GetTopFavorite();
        }

        private void AppInfo()
        {
            listQueryApps = listStaticApps.OrderByDescending(x => x.ID).ToList();
            if (tbAppName.Text != "" || tbId.Text != "")
            {
                listQueryApps = listQueryApps.Where(x => x.ID.ToString().StartsWith(tbId.Text)).ToList();
                listQueryApps = listQueryApps.Where(x => x.NameApp.ToLower().StartsWith(tbAppName.Text)).ToList();
            }

            tAppForm.Text = listQueryApps.Count.ToString()+"/"+totalApps;
            lvApps.ItemsSource = listQueryApps;
        }

        //Все приложения
        public List<Apps> listStaticApps { get; set; }
        //Пользовательский список приложения
        public List<Apps> listQueryApps { get; set; }
        //Избранные приложения
        public List<TopFavorite> listFavoriteApps { get; set; }
        public SeriesCollection SeriesPie { get; set; } = new SeriesCollection();
        public string totalApps { get; set; }
        public string totalAppsFrom { get; set; }

        public List<string> listtop = new List<string>() { "Топ 5 скачиваемых", "Топ 5 желаемых"};

        private void lvTop5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbTop5.Text = lvTop5.SelectedItem as string;
            lvTop5.Visibility = Visibility.Hidden;

            if (SeriesPie.Count > 0)
            {
                foreach (var item in SeriesPie)
                {
                    SeriesPie.Remove(item);
                }
            }

            if (lvTop5.SelectedIndex == 0)
            {
                foreach (var item in (from p in listStaticApps
                                      orderby p.Downloads descending
                                      select p).Take(5).ToList())
                {
                    SeriesPie.Add(new PieSeries
                    {
                        Title = item.NameApp,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(item.Downloads) },
                        DataLabels = true
                    });
                }
            }
            else
            {
                foreach (var item in listFavoriteApps.Take(5))
                {
                    SeriesPie.Add(new PieSeries
                    {
                        Title = item.AppName,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(item.FavoriteCount) },
                        DataLabels = true
                    });
                }
            }

            chartPie.Update(true, true);
        }

        private void Button_forvard_click(object sender, RoutedEventArgs e)
        {
            FrameManager.mainFrame.Navigate(new ManagePage());
        }

        private void tbId_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppInfo();
        }

        private void tbAppName_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppInfo();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvApps.SelectedItem != null)
            {
                await app.RemoveApp(((Apps)lvApps.SelectedItem).ID);

                NameValueCollection param = new NameValueCollection();

                param.Add("name", ((Apps)lvApps.SelectedItem).NameApp);
                using (var client = new WebClient())
                {
                    var response = await client.UploadValuesTaskAsync(new Uri("https://cryptorin.ru/files/API/RemoveApp.php"), "POST", param);

                    string res = Encoding.UTF8.GetString(response);
                    if (res.Replace(" ", "") != "")
                    {
                        MessageBox.Show(res);
                    }
                }

                AppLoad();
                lvApps.ItemsSource = listStaticApps;
            }
        }
    }
}
