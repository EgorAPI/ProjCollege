using Launcher0._2.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Net;
using Launcher0._2.Data;
using Launcher0._2.Models;
using System.Security.Cryptography;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Launcher0._2.Pages.MainPages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();

            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.RemoveBackEntry();
            }
        }

        private string _gamename { get; set; }
        private string[] _games { get; set; }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = CurrentUser.user;

            try
            {
                _games = Directory.GetDirectories("VeneraApps/");
                for (int i = 0; i < _games.Length; i++)
                {
                    _games[i] = _games[i].Replace("VeneraApps/", "");
                }
                lvMyGame.ItemsSource = _games;
            }
            catch (Exception)
            {
            }
        }

        //Выбор фото с помощью OpenFileDialog
        private async void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            try
            {
                
                if (openFileDialog.ShowDialog() == true)
                {
                    string path = Path.GetFullPath(openFileDialog.FileName);
                    if (new FileInfo(path).Length / (1024*1024) <= 2)
                    {

                        await ImgSend(path);
                        await LoadPhoto();
                    }
                    else { MessageBox.Show("Файл превышает 2 мб"); }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Получение base64 фото с сервера и его конвертация в bitmapImage
        private async Task LoadPhoto()
        {
            byte[] imgBytes;
            string imgBase64;

            NameValueCollection param = new NameValueCollection();

            param.Add("id", CurrentUser.user.ID.ToString());
            try
            {
                using (WebClient client = new WebClient())
                {
                    var response = await client.UploadValuesTaskAsync(new Uri("https://cryptorin.ru/files/API/ImageGetter.php"), "POST", param);

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Конвертация в base64 и отправка фото на сервер
        private async Task ImgSend(string path)
        {
            byte[] imgBytes = File.ReadAllBytes(path);
            string imgBase64 = Convert.ToBase64String(imgBytes);

            NameValueCollection param = new NameValueCollection();

            param.Add("id", CurrentUser.user.ID.ToString());
            param.Add("image", imgBase64);

            using (WebClient client = new WebClient())
            {
                var response = await client.UploadValuesTaskAsync(new Uri("https://cryptorin.ru/files/API/ImageSetter.php"), "POST", param);

                string res = Encoding.UTF8.GetString(response);
                MessageBox.Show(res);
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var user = new User()
            {
                ID = CurrentUser.user.ID,
                UserName = tbName.Text,
                Email = tbEmail.Text,
                Password = new HashBuilder().GetHash(tbPassword.Text),
            };

            var db = new dbUser();
            int res = await db.UpdateUser(user);
            if (res == 1)
            {
                MessageBox.Show("success");
            }
        }

        private void btnWallpaper_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void lvMyGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnPlay.IsEnabled = true;
            btnRemove.IsEnabled = true;
            _gamename = lvMyGame.SelectedItem as string;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] games = Directory.GetDirectories($"VeneraApps/{_gamename}");
                if (games.Length <2)
                {
                    string game = games.First();

                    string[] files = Directory.GetFiles($"{game}", "*.exe");
                    string path = files.Where(x => !x.Equals("UnityCrashHandler64.exe")).First();

                    Process.Start(path);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось найти файл!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show($"Удалить {_gamename}?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                if (Directory.Exists($"VeneraApps/{_gamename}"))
                {
                    Directory.Delete($"VeneraApps/{_gamename}", true);
                }

                try
                {
                    _games = Directory.GetDirectories("VeneraApps/");
                    for (int i = 0; i < _games.Length; i++)
                    {
                        _games[i] = _games[i].Replace("VeneraApps/", "");
                    }
                    lvMyGame.ItemsSource = _games;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
