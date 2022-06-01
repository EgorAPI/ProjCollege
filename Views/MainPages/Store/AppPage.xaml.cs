using Launcher0._2.Classes;
using Launcher0._2.Models;
using Launcher0._2.Pages.MainPages;
using Microsoft.Win32;
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
using System.IO;
using System.Net;
using System.Threading;
using Launcher0._2.Data;
using System.IO.Compression;

namespace Launcher0._2.Views.MainPages.Strore
{
    /// <summary>
    /// Interaction logic for AppPage.xaml
    /// </summary>
    public partial class AppPage : Page
    {
        public AppPage(Apps app, int favoriteState)
        {
            InitializeComponent();

            this.DataContext = app;

            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.RemoveBackEntry();
            }
            this.favoriteState = favoriteState;
        }
        private int favoriteState { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (favoriteState == 1)
            {
                btnFavorite.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Assets/heart2.png")));
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.mainFrame.Navigate(new StorePage());
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //Скачивание
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gridProgress.Visibility = Visibility.Visible;
                Directory.CreateDirectory($@"VeneraApps\{((Apps)this.DataContext).NameApp}\");

                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (s, a) => {
                        pbProgress.Value = a.ProgressPercentage;
                        tbPercent.Text = $"{a.ProgressPercentage}% ({((double)a.BytesReceived / 1048576).ToString("#.#")}мб)";
                    };
                    client.DownloadFileCompleted += Client_DownloadFileCompleted;

                    client.DownloadFileTaskAsync($"https://cryptorin.ru/files/Uploads/{((Apps)this.DataContext).NameApp}.zip", $"{((Apps)this.DataContext).NameApp}.zip");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //По завершению скачивания (распакова)
        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                ZipFile.ExtractToDirectory($"{((Apps)this.DataContext).NameApp}.zip", $@"VeneraApps\{((Apps)this.DataContext).NameApp}\");
            }
            catch (Exception)
            { }
            File.Delete($"{((Apps)this.DataContext).NameApp}.zip");

            gridProgress.Visibility = Visibility.Hidden;
        }

        //Избранное добавление - удаление
        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
            dbAppFavorite appFavorite = new dbAppFavorite();
            if (favoriteState == 0)
            {
                appFavorite.AddFavorite(((Apps)this.DataContext).ID, CurrentUser.user.ID);
                btnFavorite.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Assets/heart2.png")));
                favoriteState = 1;
            }
            else
            {
                appFavorite.RemoveFavorite(((Apps)this.DataContext).ID, CurrentUser.user.ID);
                btnFavorite.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Assets/heart1.png")));
                favoriteState = 0;
            }
        }
    }
}
