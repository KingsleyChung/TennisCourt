using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TennisCourt.ViewModels;

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

            create_set("1");
            //ViewModel.SelectGameScore = ViewModel.AllGameScore.ElementAt(ViewModel.AllGameScore.Count - 1);
        }

        private void create_set(string flag)
        {
            var count = ViewModel.AllGameScore.Count;
            var totalGames = 0;
            if (count == 0) totalGames = 0;
            else totalGames = ViewModel.AllGameScore.ElementAt(ViewModel.AllGameScore.Count-1).TotalGames;
            var gameID = ViewModel.SelectSpecialSet.SetID + totalGames.ToString();
            var serverName = ViewModel.SelectSpecialSet.Server;
            var receiverName = ViewModel.SelectSpecialSet.Receiver;
            var serverSet = "0";
            var receiverSet = "0";
            var serverScore = "0";
            var receiverScore = "0";
            var ballFlag = flag;
            ViewModel.AddGameScore(gameID, totalGames, serverName, receiverName, ballFlag, serverSet, receiverSet, serverScore, receiverScore);
        }

        private void ScoreDisplay_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private async void Server_Add_Click(object sender, RoutedEventArgs e)
        {
            int flag = 0;
            var select = ((Button)e.OriginalSource).DataContext as Models.Score;
            var selectgame = select.TotalGames;
            var selectserverset = select.ServerSet;
            var selectreceiverset = select.ReceiverSet;
            var selectserverscore = select.ServerScore;
            var selectreceiverscore = select.ReceiverScore;
            var ballflag = select.BallFlag;
            if (selectserverscore == "0") selectserverscore = "15";
            else if (selectserverscore == "15") selectserverscore = "30";
            else if (selectserverscore == "30") selectserverscore = "40";
            else if (selectserverscore == "40" && selectreceiverscore == "40") selectserverscore = "Ad";
            else if (selectserverscore == "40" && selectreceiverscore == "Ad")
            {
                selectserverscore = "40";
                selectreceiverscore = "40";
            }
            else if (selectserverscore == "Ad" || (selectserverscore == "40" && selectreceiverscore != "40"))
            {
                var set1 = int.Parse(selectserverset);
                var set2 = int.Parse(selectreceiverset);
                set1++;
                //判断是否进行下一局
                if ((set1 == 6 && set2 < 5) || (set1 == 7))
                {
                    flag++;
                }
                selectserverset = set1.ToString();
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //var result = ViewModel.SelectGameScore.ServerSet + "-" + ViewModel.SelectGameScore.ReceiverSet;
                    
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectSpecialSet.SetID),
                        new KeyValuePair<string,string>("game", selectgame.ToString()),
                        new KeyValuePair<string,string>("result", selectserverset + "-" + selectreceiverset),
                        new KeyValuePair<string,string>("score", selectserverscore + "-" + selectreceiverscore)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3000/changescore", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var gameinfo = JObject.Parse(responseBody);
                        if ((string)gameinfo["ok"] != "0")
                        {
                            var game = selectgame;
                            var id = ViewModel.SelectSpecialSet.SetID + game.ToString();
                            var servername = select.ServerName;
                            var receivername = select.ReceiverName;
                            var ball = ballflag;
                            var serverset = selectserverset;
                            var receiverset = selectreceiverset;
                            var serverscore = selectserverscore;
                            var receiverscore = selectreceiverscore;

                            ViewModel.AddGameScore(id, game, servername, receivername, ball, serverset, receiverset, serverscore, receiverscore);
                            if (flag != 0)
                            {
                                string tmp = "";
                                if (ball == "1") tmp = "0";
                                else tmp = "1";
                                create_set(tmp);
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
            }

            
        }

        private async void Receiver_Add_Click(object sender, RoutedEventArgs e)
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
