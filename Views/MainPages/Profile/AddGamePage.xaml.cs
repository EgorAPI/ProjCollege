﻿using Launcher0._2.Classes;
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
    /// Логика взаимодействия для AddGamePage.xaml
    /// </summary>
    public partial class AddGamePage : Page
    {
        public AddGamePage()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameManager.mainFrame.CanGoBack)
            {
                FrameManager.mainFrame.GoBack();
            }
        }
    }
}
