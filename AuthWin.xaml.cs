using Launcher0._2.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для AuthWin.xaml
    /// </summary>
    public partial class AuthWin : Window
    {
        public AuthWin()
        {
            InitializeComponent();
            AuthWindow.authWin = this;
            AuthFrame.Navigate(new AuthPage());
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AuthFrame.CanGoBack)
            {
                AuthFrame.GoBack();
            }
        }

        private void AuthFrame_ContentRendered(object sender, EventArgs e)
        {
            if (!AuthFrame.CanGoBack)
            {
                BackBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
