using System;
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
    public sealed partial class MainPage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = ((ViewModels.MatchesViewModel)e.Parameter);
            Username.Text = ViewModel.CurrentUser.Username;
            StudentID.Text = ViewModel.CurrentUser.StudentID;
            myFrame.Navigate(typeof(HomePage));
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myFrame == null)
            {
                return;
            }
            switch (Menu.SelectedIndex)
            {
                case 0:
                    myFrame.Navigate(typeof(HomePage), ViewModel);
                    break;
                case 1:
                    myFrame.Navigate(typeof(MatchesPage), ViewModel);
                    break;
                case 2:
                    myFrame.Navigate(typeof(DrawsPage), ViewModel);
                    break;
                case 3:
                    myFrame.Navigate(typeof(LatestPage), ViewModel);
                    break;
                case 4:
                    myFrame.Navigate(typeof(HistoryPage), ViewModel);
                    break;
            }
        }

        private void AdministratorButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminPage), ViewModel);
        }
    }
}
