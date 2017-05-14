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
using TennisCourt.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewGamePage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public NewGamePage()
        {
            this.InitializeComponent();
            Message.Opacity = 0;

            ManSingle.Visibility = Visibility.Visible;
            WomanSingle.Visibility = Visibility.Visible;
            MenDouble.Visibility = Visibility.Visible;
            WomenDobule.Visibility = Visibility.Visible;
            MixDouble.Visibility = Visibility.Visible;

            court1.Visibility = Visibility.Visible;
            court2.Visibility = Visibility.Visible;
            court3.Visibility = Visibility.Visible;
            court4.Visibility = Visibility.Visible;
            court5.Visibility = Visibility.Visible;
            court6.Visibility = Visibility.Visible;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Message.Opacity = 0;

            ViewModel = ((ViewModels.MatchesViewModel)e.Parameter);

            //插入games到matches
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectMatch.MatchID)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3000/matchgame", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var gameinfo = JObject.Parse(responseBody);
                        if ((string)gameinfo["ok"] != "0")
                        {
                            var allgame = (JArray)gameinfo["games"];
                            for (int i = 0; i < allgame.Count; i++)
                            {
                                var setId = (string)allgame[i]["matchid"];
                                var player1 = (string)allgame[i]["player1"];
                                var player2 = (string)allgame[i]["player2"];
                                var cata = (string)allgame[i]["catagory"];
                                var ump = (string)allgame[i]["umpire"];
                                var line = (string)allgame[i]["lineman"];
                                var res = (string)allgame[i]["result"];
                                var cour = (string)allgame[i]["court"];
                                var rou = (string)allgame[i]["round"];
                                var status = (string)allgame[i]["status"];
                                var fa = (string)allgame[i]["date"];
                                var date = Convert.ToDateTime(fa);
                                List<string> score = new List<string>();
                                ViewModel.AddSpecialGame(setId, player1, player2, cata, ump, line, date, date, date, cour, rou, res, score, status);
                                Games game = ViewModel.AllSpecialSets.ElementAt(ViewModel.AllSpecialSets.Count - 1);
                                for (int j = 0; j < ViewModel.AllMatches.Count; j++)
                                {
                                    if (ViewModel.AllMatches.ElementAt(j).MatchID == ViewModel.SelectMatch.MatchID)
                                    {
                                        var selectMatch = ViewModel.AllMatches.ElementAt(j);
                                        selectMatch.Game.Add(game);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
            }

            MatchTitle.Text = ViewModel.SelectMatch.MatchTitle;
            string catagory = "";
            char[] emptycourt = new char[6] {'1','1','1','1','1','1'};
            for (int i = 0; i < ViewModel.AllMatches.Count; i++)
            {
                if (ViewModel.AllMatches.ElementAt(i).MatchTitle == ViewModel.SelectMatch.MatchTitle)
                {
                    catagory = ViewModel.AllMatches.ElementAt(i).Categories;
                    var tmp = ViewModel.AllMatches.ElementAt(i).Game;
                    for (int j = 0; j < tmp.Count; j++)
                    {
                        if (tmp[j].Status == "0" || tmp[j].Status == "1")
                        {
                            string s = tmp[j].Court;
                            s = s.Substring(6, 1);
                            int num = int.Parse(s);
                            emptycourt[num-1] = '0';
                        }
                    }
                }

            }
            
            if (catagory[0] == '0') ManSingle.Visibility = Visibility.Collapsed;
            if (catagory[1] == '0') WomanSingle.Visibility = Visibility.Collapsed;
            if (catagory[2] == '0') MenDouble.Visibility = Visibility.Collapsed;
            if (catagory[3] == '0') WomenDobule.Visibility = Visibility.Collapsed;
            if (catagory[4] == '0') MixDouble.Visibility = Visibility.Collapsed;

            if (emptycourt[0] == '0') court1.Visibility = Visibility.Collapsed;
            if (emptycourt[1] == '0') court2.Visibility = Visibility.Collapsed;
            if (emptycourt[2] == '0') court3.Visibility = Visibility.Collapsed;
            if (emptycourt[3] == '0') court4.Visibility = Visibility.Collapsed;
            if (emptycourt[4] == '0') court5.Visibility = Visibility.Collapsed;
            if (emptycourt[5] == '0') court6.Visibility = Visibility.Collapsed;
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            if (Player1Input.Text == "")
            {
                Message.Opacity = 1;
                Message.Text = "Player1 can not be empty";
                return;
            }
            if (Player2Input.Text == "")
            {
                Message.Opacity = 1;
                Message.Text = "Player2 can not be empty";
                return;
            }
            if (UmpireInput.Text == "")
            {
                Message.Opacity = 1;
                Message.Text = "Umpire can not be empty";
                return;
            }
            if (LinemanInput.Text == "")
            {
                Message.Opacity = 1;
                Message.Text = "Lineman can not be empty";
                return;
            }

            string[] allcatagory = new string[5] { "男子单打", "女子单打", "男子双打", "女子双打", "混合双打" };
            string[] allcourt = new string[6] { "Court 1", "Court 2", "Court 3", "Court 4", "Court 5", "Court 6" };
            string[] allround = new string[4] { "1st-Round", "2nd-Round", "Semi-Final", "Final" };

            var matchId = ViewModel.SelectMatch.MatchID;
            var server = Player1Input.Text;
            var receiver = Player2Input.Text;
            var umpire = UmpireInput.Text;
            var lineman = LinemanInput.Text;
            var courtnum = AvaliableCourt.SelectedIndex;
            var roundnum = RoundSelector.SelectedIndex;
            var catagorynum = CategoriesSelector.SelectedIndex;
            var court = allcourt[courtnum];
            var round = allround[roundnum];
            var catagory = allcatagory[catagorynum];
            CategoriesSelector.PlaceholderText = catagory;
            AvaliableCourt.PlaceholderText = court;
            RoundSelector.PlaceholderText = round;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", matchId),
                        new KeyValuePair<string,string>("player1", server),
                        new KeyValuePair<string,string>("player2", receiver),
                        new KeyValuePair<string,string>("catagory", catagory),
                        new KeyValuePair<string,string>("umpire", umpire),
                        new KeyValuePair<string,string>("lineman", lineman),
                        new KeyValuePair<string,string>("court", court),
                        new KeyValuePair<string,string>("round", round),
                        new KeyValuePair<string,string>("status", "1"),
                        new KeyValuePair<string,string>("endTime", "")
                    };
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3000/creategame", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var gameinfo = JObject.Parse(responseBody);
                        //正确时创建赛事成功
                        if ((string)gameinfo["ok"] != "0")
                        {
                            var all = gameinfo;
                            var setId = (string)gameinfo["matchId"];
                            var player1 = (string)gameinfo["player1"];
                            var player2 = (string)gameinfo["player2"];
                            var cata = (string)gameinfo["catagory"];
                            var ump = (string)gameinfo["umpire"];
                            var line = (string)gameinfo["lineman"];
                            var cour = (string)gameinfo["court"];
                            var rou = (string)gameinfo["round"];                     
                            var status = (string)gameinfo["status"];
                            var dd = (string)gameinfo["date"];
                            var date = Convert.ToDateTime(dd);
                            var d1 = (string)gameinfo["startTime"];
                            var date1 = Convert.ToDateTime(d1);
                            List<string> score = new List<string>();
                            ViewModel.AddSpecialGame(setId, player1, player2, cata, ump, line, date, date1, date1, cour, rou, "0-0", score, "1");

                            Frame.Navigate(typeof(GamesPage), ViewModel);
                        }
                        //不正确时输出错误信息
                        else
                        {
                            Message.Opacity = 1;
                            Message.Text = (string)gameinfo["error"];
                            Message.Opacity = 1;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
