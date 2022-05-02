using Launcher0._2.Classes;
using Launcher0._2.Data;
using Launcher0._2.Models;
using Launcher0._2.Views.MainPages.Strore;
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
           int count = await CreateItems();
        }

        private async Task<int> CreateItems()
        {
            dbApp apps = new dbApp();

            List<Apps> appsList = await apps.GetApps();

            wpGame.DataContext = appsList;
            gNewApp.DataContext = appsList[0];

            int count = 0;

            try
            {
                foreach (Apps item in appsList)
                {

                    StackPanel stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(15),
                    };
                    stackPanel.MouseLeftButtonDown += (s, a) => { FrameManager.mainFrame.Navigate(new AppPage(item)); };

                    Border border = new Border
                    {
                        Width = 300,
                        Height = 300,
                        Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(item.Photo)),
                            Stretch = Stretch.UniformToFill
                        },

                        Effect = new System.Windows.Media.Effects.DropShadowEffect
                        {
                            BlurRadius = 15
                        }
                    };

                    TextBlock textBlock = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 24,
                        FontWeight = FontWeights.Medium,
                        Foreground = Brushes.LightGray,
                        Text = item.NameApp
                    };

                    TextBlock textBlock1 = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 20,
                        FontWeight = FontWeights.Medium,
                        Foreground = Brushes.Gray,
                        Text = item.AppCategory.ToString()
                    };

                    wpGame.Children.Add(stackPanel);
                    stackPanel.Children.Add(border);
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(textBlock1);

                    count++;
                }
            }
            catch (Exception)
            {

            }


            return count;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
