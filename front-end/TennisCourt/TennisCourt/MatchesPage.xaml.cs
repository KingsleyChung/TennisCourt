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
    public sealed partial class MatchesPage : Page
    {
        public MatchesPage()
        {
            this.InitializeComponent();
        }

        private ViewModels.MatchesViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*
            if ((string)e.Parameter == "User")
            {
                CommandBar.Visibility = Visibility.Collapsed;
            }
            */

            ViewModel = ((ViewModels.MatchesViewModel)e.Parameter);
        }

        private void AddMatch_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewMatchPage), ViewModel);
        }

        private void matches_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectMatch = (Models.Matches)(e.ClickedItem);
            Frame.Navigate(typeof(GamesPage), ViewModel);
        }
    }
}
