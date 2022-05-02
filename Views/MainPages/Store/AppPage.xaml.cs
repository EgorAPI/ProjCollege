using Launcher0._2.Classes;
using Launcher0._2.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launcher0._2.Views.MainPages.Strore
{
    /// <summary>
    /// Interaction logic for AppPage.xaml
    /// </summary>
    public partial class AppPage : Page
    {
        public AppPage(Apps app)
        {
            InitializeComponent();

            this.DataContext = app;

            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.RemoveBackEntry();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
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
    }
}
