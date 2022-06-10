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
        private string _photopath { get; set; }
        private int _state = 0;

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

        //Добавление App на сервер и в БД
        private async Task UploadData()
        {
            int state = 0;
            //загрузка файла
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/zip";
                client.UploadProgressChanged += (sender, e) => { pbProgress.Value = e.ProgressPercentage*2;
                    tbPercent.Text = $"{e.ProgressPercentage*2}% ({((double)e.BytesSent / 1048576).ToString("#.#")}мб)"; };

                tbNameGame.IsEnabled = false;
                tbCategory.IsEnabled = false;
                tbDescription.IsEnabled = false;
                _state = 1;

                client.UploadFileCompleted += (s,e) => { tbNameGame.IsEnabled=true;
                    tbCategory.IsEnabled = true;
                    tbDescription.IsEnabled = true;
                    _state = 0;
                };

                var resp = client.UploadFileTaskAsync(new Uri("https://cryptorin.ru/files/API/UploadFile.php"), _filepath);
                await ImgSend(_photopath);

                if (Encoding.UTF8.GetString(await resp).Replace(" ","") != "")
                {
                    MessageBox.Show(Encoding.UTF8.GetString(await resp));
                    state = 1;
                }
            }

            //добавление в БД
            if (state == 0)
            {
                
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
        }

        //Выбор фото
        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    _photopath = Path.GetFullPath(openFileDialog.FileName);
                    imgPhoto.Source = new BitmapImage(new Uri(_photopath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        //Загрузка фото на сервер
        private async Task ImgSend(string path)
        {
            byte[] imgBytes = File.ReadAllBytes(path);
            string imgBase64 = Convert.ToBase64String(imgBytes);

            NameValueCollection param = new NameValueCollection();

            param.Add("name", tbNameGame.Text);
            param.Add("image", imgBase64);

            using (WebClient client = new WebClient())
            {
                var response = await client.UploadValuesTaskAsync(new Uri("https://cryptorin.ru/files/API/ImageAppSetter.php"), "POST", param);

                if (Encoding.UTF8.GetString(response).Replace(" ", "") != "")
                {
                    MessageBox.Show(Encoding.UTF8.GetString(response));
                }
            }
        }

        List<AppCategory> categoryList { get; set; }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dbCategory db = new dbCategory();
            categoryList = await db.GetCategories();

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

        private void tbCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (categoryList != null)
            {
                lvCategory.ItemsSource = categoryList.Where(x => x.NameCategory.ToLower().Contains(tbCategory.Text.ToLower())).ToList();
            }
        }
    }
}
