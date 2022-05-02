using Launcher0._2.Classes;
using Launcher0._2.Pages.MainPages;
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
using System.Windows.Shapes;
using System.IO;
using System.Collections.Specialized;
using System.Net;

namespace Launcher0._2
{
    /// <summary>
    /// Логика взаимодействия для MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        public MainWin()
        {
            InitializeComponent();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            AuthWin authWin = new AuthWin();
            authWin.Show();
            this.Close();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrameManager.mainFrame = MainFrame;
            switch (lvMunuElmt.SelectedIndex)
            {
                case 0: MainFrame.Navigate(new ProfilePage()); break;
                case 1: MainFrame.Navigate(new StorePage()); break;
                case 2: MainFrame.Navigate(new Views.MainPages.About.AboutPage()); break;
                case 3: MainFrame.Navigate(new AddAppPage()); break;
                case 4: MainFrame.Navigate(new Views.MainPages.Manage.ManagePage()); break;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FrameManager.mainFrame = MainFrame;

            this.DataContext = CurrentUser.user;

            LoadPhoto();

            if (CurrentUser.user.UserStatus_id == "Admin")
            {
                itmCreate.Visibility = Visibility.Visible;
                itmManage.Visibility = Visibility.Visible;
            }

        }

        private void LoadPhoto()
        {
            byte[] imgBytes;
            string imgBase64;

            NameValueCollection param = new NameValueCollection();

            param.Add("id", CurrentUser.user.ID.ToString());

            using (WebClient client = new WebClient())
            {
                var response = client.UploadValues(new Uri("https://cryptorin.ru/files/API/ImageGetter.php"), "POST", param);

                imgBase64 = Encoding.UTF8.GetString(response);
                imgBytes = Convert.FromBase64String(imgBase64);

                var img = new BitmapImage();
                using (var memStream = new MemoryStream(imgBytes))
                {
                    memStream.Position = 0;
                    img.BeginInit();
                    img.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.UriSource = null;
                    img.StreamSource = memStream;
                    img.EndInit();
                }

                CurrentUser.user.Photo = img;
            };
        }
    }
}
