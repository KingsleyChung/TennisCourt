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
using TennisCourt.Models;
using System.Net.Http;
using Windows.UI.Popups;
using Newtonsoft.Json.Linq;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GamesPage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public GamesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = ((ViewModels.MatchesViewModel)e.Parameter);
            
            //remove all specialsets
            while (ViewModel.AllSpecialSets.Count > 0)
            {
                ViewModel.AllSpecialSets.RemoveAt(0);
            }

            //user mode
            if (ViewModel.CurrentUser.Mode == "0")
            {
                CommandBar.Visibility = Visibility.Collapsed;
            }

            if (ViewModel.SelectMatch != null)
            {
                //show allgames
                show_allGames();
            }
        }

        private async void show_allGames()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectMatch.MatchID)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://www.zhengweimumu.cn:3000/matchgame", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var gameinfo = JObject.Parse(responseBody);
                        if ((string)gameinfo["ok"] != "0")
                        {
                            var allgame = (JArray)gameinfo["games"];
                            for (int i = 0; i < allgame.Count; i++)
                            {
                                var setId = (string)allgame[i]["matchId"];
                                var player1 = (string)allgame[i]["player1"];
                                var player2 = (string)allgame[i]["player2"];
                                var cata = (string)allgame[i]["catagory"];
                                var ump = (string)allgame[i]["umpire"];
                                var line = (string)allgame[i]["lineman"];
                                var res = (string)allgame[i]["result"];
                                var cour = (string)allgame[i]["court"];
                                var rou = (string)allgame[i]["round"];
                                var status = (string)allgame[i]["status"];
                                var dd = (string)allgame[i]["date"];
                                var date = Convert.ToDateTime(dd);
                                var d1 = (string)allgame[i]["startTime"];
                                var d2 = (string)allgame[i]["endTime"];
                                var date1 = Convert.ToDateTime(d1);
                                var date2 = date1;
                                if (d2 != "") date2 = Convert.ToDateTime(d2);
                                List<string> score = new List<string>();
                                ViewModel.AddSpecialGame(setId, player1, player2, cata, ump, line, date, date1, date2, cour, rou, res, score, status);

                            }


                            //Frame.Navigate(typeof(MainPage), ViewModel);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
            }
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewGamePage), ViewModel);
        }

        private void games_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectSpecialSet = (Models.Games)(e.ClickedItem);
            Frame.Navigate(typeof(ReadyPage), ViewModel);
        }

        private void EditMatch_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewMatchPage), ViewModel);
        }
    }
}
