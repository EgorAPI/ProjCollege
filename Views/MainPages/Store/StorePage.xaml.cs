using Launcher0._2.Classes;
using Launcher0._2.Data;
using Launcher0._2.Models;
using Launcher0._2.Views.MainPages.Strore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Launcher0._2.Pages.MainPages
{
    /// <summary>
    /// Логика взаимодействия для StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        public StorePage()
        {
            InitializeComponent();

            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.RemoveBackEntry();
            }
        }
        public List<Apps> mainListApps { get; set; }
        public List<Apps> searchListApps = new List<Apps>();
        public List<AppFavorite> listfavorite { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           CreateItems();
        }

        private async void CreateItems()
        {
            dbApp dbApp = new dbApp();
            mainListApps = await dbApp.GetApps();
            listApps.ItemsSource = mainListApps;

            dbAppFavorite appFavorite = new dbAppFavorite();
            listfavorite = appFavorite.GetAppsFavorite();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void listApps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listfavorite.Where(x => x.App_id == ((Apps)listApps.SelectedItem).ID).Count() > 0)
                {
                    FrameManager.mainFrame.Navigate(new AppPage((Apps)listApps.SelectedItem, 1));
                }
                else
                {
                    FrameManager.mainFrame.Navigate(new AppPage((Apps)listApps.SelectedItem, 0));
                }
            }
            catch (Exception)
            {
            }
            
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchApps();
        }

        private void togDate_Checked(object sender, RoutedEventArgs e)
        {
            SearchApps();
        }

        private void togDate_Unchecked(object sender, RoutedEventArgs e)
        {
            SearchApps();
        }

        private void togFavorite_Checked(object sender, RoutedEventArgs e)
        {
            SearchApps();
        }

        private void togFavorite_Unchecked(object sender, RoutedEventArgs e)
        {
            SearchApps();
        }
        private void SearchApps()
        {
            //поиск
            searchListApps = mainListApps.Where(x => x.NameApp.ToLower().Contains(tbSearch.Text.ToLower())).ToList();

            if (togDate.IsChecked is true)
            {
                searchListApps = searchListApps.OrderBy(x => x.DateOfCreated).ToList();
            }
            else
            {
                searchListApps = searchListApps.OrderByDescending(x => x.DateOfCreated).ToList();
            }

            try
            {
                //поиск среди избранных
                if (togFavorite.IsChecked is true)
                {
                    searchListApps.RemoveAll(x => x.ID > -1);
                    for (int i = 0; i < listfavorite.Count; i++)
                    {
                        searchListApps.Add(mainListApps.Where(x => x.ID == listfavorite.ElementAt(i).App_id).FirstOrDefault());
                    }

                    searchListApps = searchListApps.Where(x => x.NameApp.ToLower().Contains(tbSearch.Text.ToLower())).ToList();

                    if (togDate.IsChecked is true)
                    {
                        searchListApps = searchListApps.OrderBy(x => x.DateOfCreated).ToList();
                    }
                    else
                    {
                        searchListApps = searchListApps.OrderByDescending(x => x.DateOfCreated).ToList();
                    }
                }
            }
            catch (Exception)
            {
            }
            listApps.ItemsSource = searchListApps;
        }
    }
}
