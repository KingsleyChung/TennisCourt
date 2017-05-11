﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();
        }


        ViewModels.MatchesViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            myFrame.Navigate(typeof(MatchesPage), "Administrator");
            ViewModel = (ViewModels.MatchesViewModel)e.Parameter;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myFrame == null)
            {
                return;
            }
            switch (AdminMenu.SelectedIndex)
            {
                case 0:
                    myFrame.Navigate(typeof(MatchesPage), "Administrator");
                    break;
                case 1:
                    myFrame.Navigate(typeof(GamesPage));
                    break;
            }
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
