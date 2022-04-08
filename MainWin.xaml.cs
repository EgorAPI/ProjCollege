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
            FrameManager.mainFrame = MainFrame;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
            Frame mainFrame = MainFrame;
            switch (lvMunuElmt.SelectedIndex)
            {
                case 0: mainFrame.Navigate(new ProfilePage()); break;
                case 1: mainFrame.Navigate(new StorePage()); break;
                case 2: mainFrame.Navigate(new SettingsPage()); break;
                case 3: mainFrame.Navigate(new Views.MainPages.Manage.ManagePage()); break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
