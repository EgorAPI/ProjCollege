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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Border border = new Border
            {
                BorderBrush = (SolidColorBrush)Application.Current.Resources["MainColorTwo"],
                BorderThickness = new Thickness(2),
                Margin = new Thickness(15),
                Opacity = 0.8
            };
            wpGame.Children.Add(border);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Height = 240;
            stackPanel.Width = 240;
            border.Child = stackPanel;

            Image image = new Image
            {
                Height = 180,
                Margin = new Thickness(10)
            };
            stackPanel.Children.Add(image);

            TextBlock textBlock = new TextBlock
            {
                Tag = "NameGame",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0,5,0,0),
                FontSize = 24,
                Text = "NameGame"
            };
            stackPanel.Children.Add(textBlock);
        }
    }
}
