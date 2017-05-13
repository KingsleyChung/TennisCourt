using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    public sealed partial class DetailPage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (ViewModels.MatchesViewModel)e.Parameter;

            Server.Text = ViewModel.SelectSpecialSet.Server;
            Receiver.Text = ViewModel.SelectSpecialSet.Receiver;
            Umpire.Text = ViewModel.SelectSpecialSet.Umpire;
            Lineman.Text = ViewModel.SelectSpecialSet.Lineman;
            Category.Text = ViewModel.SelectSpecialSet.Category;
            Round.Text = ViewModel.SelectSpecialSet.Round;


            var totalGames = 1;
            var gameID = ViewModel.SelectSpecialSet.SetID + totalGames.ToString();
            var serverName = ViewModel.SelectSpecialSet.Server;
            var receiverName = ViewModel.SelectSpecialSet.Receiver;
            var ballFlag = "1";
            var serverSet = "0";
            var receiverSet = "0";
            var serverScore = "0";
            var receiverScore = "0";
            ViewModel.AddGameScore(gameID, totalGames, serverName, receiverName, ballFlag, serverSet, receiverSet, serverScore, receiverScore);
        }

        private void ScoreDisplay_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void Server_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Receiver_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Recall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GameOver_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
