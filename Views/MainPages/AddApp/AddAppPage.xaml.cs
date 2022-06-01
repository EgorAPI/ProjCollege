using Launcher0._2.Classes;
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
using Launcher0._2.Data;
using Launcher0._2.Models;
using System.Net;
using System.Threading;
using System.Collections.Specialized;

namespace Launcher0._2.Pages.MainPages
{
    /// <summary>
    /// Логика взаимодействия для AddGamePage.xaml
    /// </summary>
    public partial class AddAppPage : Page
    {
        public AddAppPage()
        {
            InitializeComponent();
        }
        private string _img { get; set; }
        private string _filepath { get; set; }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbCategory.Text != "" && tbNameGame.Text != "" && tbDescription.Text != "" && _filepath != "" && _img != "")
            {
                await UploadData();

                NameValueCollection param = new NameValueCollection();
                param.Add("originalname", new FileInfo(_filepath).Name);
                param.Add("setname", tbNameGame.Text);

                using (var client = new WebClient())
                    await client.UploadValuesTaskAsync(new Uri("https://cryptorin.ru/files/API/UploadFile.php"), "POST", param);
            }
            else { MessageBox.Show("не все данные..."); }
        }

        private async Task UploadData()
        {
            //загрузка файла
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/zip";
                client.UploadProgressChanged += (sender, e) => { pbProgress.Value = e.ProgressPercentage;
                    tbPercent.Text = $"{e.ProgressPercentage}% ({((double)e.BytesSent / 1048576).ToString("#.#")}мб)"; };

                var resp = client.UploadFileTaskAsync(new Uri("https://cryptorin.ru/files/API/UploadFile.php"), _filepath);

                MessageBox.Show(Encoding.UTF8.GetString(await resp));
            }

            //добавление в БД
            Apps app = new Apps()
            {
                NameApp = tbNameGame.Text,
                Description = tbDescription.Text,
                AppCategory_id = ((AppCategory)lvCategory.SelectedItem).ID,
                Author_id = CurrentUser.user.ID
            };

            dbApp db = new dbApp();
            int res = await db.InsertApp(app);

            if (res == 1)
            {
                MessageBox.Show("Данные добавлены!");
            }
            else
            {
                MessageBox.Show("Что-то пошло не так!");
            }
        }

        //Установка фото
        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    path = Path.GetFullPath(openFileDialog.FileName);
                    imgPhoto.Source = new BitmapImage(new Uri(path));

                    byte[] imgBytes = File.ReadAllBytes(path);
                    _img = Convert.ToBase64String(imgBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dbCategory db = new dbCategory();
            List<AppCategory> categoryList = await db.GetCategories();

            lvCategory.ItemsSource = categoryList;

            tbCategory.GotFocus += (s,a) => { lvCategory.Visibility = Visibility.Visible; };

            lvCategory.SelectionChanged += (s,a) => { tbCategory.Text = ((AppCategory)lvCategory.SelectedItem).NameCategory;
                lvCategory.Visibility = Visibility.Hidden;
            };
        }

        //Установка файла (архива)
        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "file (*.zip)|*.zip";
            try
            {

                if (openFileDialog.ShowDialog() == true)
                {
                    string path = Path.GetFullPath(openFileDialog.FileName);
                    if (new FileInfo(path).Length / (1024 * 1024) <= 120)
                    { 
                        _filepath = path;;
                        tbfile.Text = openFileDialog.FileName;
                    }
                    else { MessageBox.Show("Файл превышает 120 мб"); }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
