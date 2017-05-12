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
    public sealed partial class ReadyPage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public ReadyPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (ViewModels.MatchesViewModel)e.Parameter;
            Player1.Text = ViewModel.SelectSpecialSet.Server;
            Player2.Text = ViewModel.SelectSpecialSet.Receiver;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectSpecialSet != null)
            {
                string[] tmp = new string[2] { Player1.Text, Player2.Text };
                if (tmp[Server.SelectedIndex] != Player1.Text)
                {
                    Server.PlaceholderText = tmp[Server.SelectedIndex];
                    ViewModel.SelectSpecialSet.Server = Player2.Text;
                    ViewModel.SelectSpecialSet.Receiver = Player1.Text;
                }
            }
            Frame.Navigate(typeof(DetailPage), ViewModel);
        }
    }
}
