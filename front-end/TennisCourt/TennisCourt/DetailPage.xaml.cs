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

            while (ViewModel.AllGameScore.Count > 0)
            {
                ViewModel.AllGameScore.RemoveAt(0);
            }

            Server.Text = ViewModel.SelectSpecialSet.Server;
            Receiver.Text = ViewModel.SelectSpecialSet.Receiver;
            Umpire.Text = ViewModel.SelectSpecialSet.Umpire;
            Lineman.Text = ViewModel.SelectSpecialSet.Lineman;
            Category.Text = ViewModel.SelectSpecialSet.Category;
            Round.Text = ViewModel.SelectSpecialSet.Round;

            create_set("1", "0", "0");
            //ViewModel.SelectGameScore = ViewModel.AllGameScore.ElementAt(ViewModel.AllGameScore.Count - 1);
        }

        private void create_set(string flag, string set1, string set2)
        {
            var count = ViewModel.AllGameScore.Count;
            var totalGames = 0;
            if (count == 0) totalGames = 1;
            else totalGames = ViewModel.AllGameScore.ElementAt(ViewModel.AllGameScore.Count-1).TotalGames + 1;
            var gameID = ViewModel.SelectSpecialSet.SetID + "/" + totalGames.ToString();
            var serverName = "";
            var receiverName = "";
            var ballFlag = flag;
            serverName = ViewModel.SelectSpecialSet.Server;
            receiverName = ViewModel.SelectSpecialSet.Receiver;
            var serverSet = set1;
            var receiverSet = set2;
            var serverScore = "00";
            var receiverScore = "00";
            ViewModel.AddGameScore(gameID, totalGames, serverName, receiverName, ballFlag, serverSet, receiverSet, serverScore, receiverScore, ballFlag);

            var one = int.Parse(serverSet);
            var two = int.Parse(receiverSet);
            //判断是否进行下一局
            if ((one == 6 && two < 5) || (one == 7) || (two == 6 && one < 5) || (two == 7))
            {
                if (ballFlag == "1")
                    ballFlag = "0";
                else
                    ballFlag = "1";
                ViewModel.ChangeButtonFlag(gameID, ballFlag);
                GameOver.Visibility = Visibility.Visible;
            }
        }

        private void Server_Add_Click(object sender, RoutedEventArgs e)
        { 
            var select = ((Button)e.OriginalSource).DataContext as Models.Score;
            var selectgame = select.TotalGames;
            var selectserverset = select.ServerSet;
            var selectreceiverset = select.ReceiverSet;
            var selectserverscore = select.ServerScore;
            var selectreceiverscore = select.ReceiverScore;
            var ballflag = select.BallFlag;
            AddPoint(0, selectgame, select.ServerName, select.ReceiverName, selectserverset, selectreceiverset, selectserverscore, selectreceiverscore, ballflag);
        }

        private void Receiver_Add_Click(object sender, RoutedEventArgs e)
        {
            var select = ((Button)e.OriginalSource).DataContext as Models.Score;
            var selectgame = select.TotalGames;
            var selectserverset = select.ServerSet;
            var selectreceiverset = select.ReceiverSet;
            var selectserverscore = select.ServerScore;
            var selectreceiverscore = select.ReceiverScore;
            var ballflag = select.BallFlag;
            AddPoint(1, selectgame, select.ServerName, select.ReceiverName, selectreceiverset, selectserverset, selectreceiverscore, selectserverscore, ballflag);
        }

        private async void AddPoint(int my, int selectgame, string serverName, string receiverName, string selectserverset, string selectreceiverset, string selectserverscore, string selectreceiverscore, string ballflag)
        {
            int flag = 1;
            if (selectserverscore == "00") selectserverscore = "15";
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
                //判断是否进行下一局
                if ((set1 == 6 && set2 < 5) || (set1 == 7))
                {
                    flag = -1;
                }
                else
                {
                    flag = 0;
                    set1++;
                    selectgame++;
                    selectserverscore = "00";
                    selectreceiverscore = "00";
                }
                selectserverset = set1.ToString();
            }

            using (HttpClient client = new HttpClient())
            {
                string res = "", sco = "";
                if (my == 0)
                {
                    res = selectserverset + "-" + selectreceiverset;
                    sco = selectserverscore + "-" + selectreceiverscore;
                }
                else if (my == 1)
                {
                    res = selectreceiverset + "-" + selectserverset;
                    sco = selectreceiverscore + "-" + selectserverscore;
                }
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectSpecialSet.SetID),
                        new KeyValuePair<string,string>("game", selectgame.ToString()),
                        new KeyValuePair<string,string>("result", res),
                        new KeyValuePair<string,string>("score", sco)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://www.zhengweimumu.cn:3000/changescore", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var gameinfo = JObject.Parse(responseBody);
                        if ((string)gameinfo["ok"] != "0")
                        {
                            var game = --selectgame;
                            var id = ViewModel.SelectSpecialSet.SetID + "/" + game.ToString();
                            var ball = ballflag;
                            var serverset = selectserverset;
                            var receiverset = selectreceiverset;
                            var serverscore = selectserverscore;
                            var receiverscore = selectreceiverscore;

                            if (flag == 0)
                            {
                                ViewModel.ChangeButtonFlag(id, "0");
                                string tmp = "";
                                if (ball == "1") tmp = "0";
                                else tmp = "1";
                                if (my == 0) create_set(tmp, serverset, receiverset);
                                else if (my == 1) create_set(tmp, receiverset, serverset);
                            }
                            else
                            {
                                game++;
                                id = ViewModel.SelectSpecialSet.SetID + "/" + game.ToString();
                                if (my == 0) ViewModel.UpdateGameScore(id, serverscore, receiverscore);
                                else if (my == 1) ViewModel.UpdateGameScore(id, receiverscore, serverscore);
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

        private void Recall_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void GameOver_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectSpecialSet.SetID),
                        new KeyValuePair<string,string>("status", "2")
                    };
                    HttpResponseMessage response = await client.PostAsync("http://www.zhengweimumu.cn:3000/changegame", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var info = JObject.Parse(responseBody);
                        if ((string)info["ok"] != "0")
                        {
                            
                            Frame.Navigate(typeof(GamesPage), ViewModel);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
            }
        }
    }
}
