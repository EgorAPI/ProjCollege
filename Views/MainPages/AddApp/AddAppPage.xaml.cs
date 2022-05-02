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

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Apps app = new Apps()
            {
                NameApp = tbNameGame.Text,
                Description = tbDescription.Text,
                AppCategory_id = ((AppCategory)lvCategory.SelectedItem).ID,
                Author_id = 1
            };

            dbApp db = new dbApp();
            int res = await db.InsertApp(app);

            if (res == 1)
            {
                MessageBox.Show("Успешно!");
            }
            else
            {
                MessageBox.Show("Что-то пошло не так!");
            }
        }

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
    }
}
