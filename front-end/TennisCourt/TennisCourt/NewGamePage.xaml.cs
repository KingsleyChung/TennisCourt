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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //ViewModel = ((ViewModels.MatchesViewModel)e.Parameter);

            //MatchTitle.Text = ViewModel.SelectMatch.MatchTitle;
            //string catagory = "";
            //char[] emptycourt = new char[6] {'1','1','1','1','1','1'};
            //for (int i = 0; i < ViewModel.AllMatches.Count; i++)
            //{
            //    if (ViewModel.AllMatches.ElementAt(i).MatchTitle == ViewModel.SelectMatch.MatchTitle)
            //    {
            //        catagory = ViewModel.AllMatches.ElementAt(i).Categories;
            //        var tmp = ViewModel.AllMatches.ElementAt(i).Game;
            //        for (int j = 0; j < tmp.Count; j++)
            //        {
            //            if (tmp[j].Status == "0" || tmp[j].Status == "-1")
            //            {
            //                int num = int.Parse(tmp[j].Court);
            //                emptycourt[num] = '0';
            //            }
            //        }
            //    }

            //}

            //if (catagory[0] == '0') ManSingle.Visibility = Visibility.Collapsed;
            //if (catagory[1] == '0') WomanSingle.Visibility = Visibility.Collapsed;
            //if (catagory[2] == '0') MenDouble.Visibility = Visibility.Collapsed;
            //if (catagory[3] == '0') WomenDobule.Visibility = Visibility.Collapsed;
            //if (catagory[4] == '0') MixDouble.Visibility = Visibility.Collapsed;

            //if (emptycourt[0] == '0') court1.Visibility = Visibility.Collapsed;
            //if (emptycourt[1] == '0') court2.Visibility = Visibility.Collapsed;
            //if (emptycourt[2] == '0') court3.Visibility = Visibility.Collapsed;
            //if (emptycourt[3] == '0') court4.Visibility = Visibility.Collapsed;
            //if (emptycourt[4] == '0') court5.Visibility = Visibility.Collapsed;
            //if (emptycourt[5] == '0') court6.Visibility = Visibility.Collapsed;
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            //if (ServerInput.Text == "")
            //{
            //    Message.Text = "Server can not be empty";
            //    return;
            //}
            //if (ReceiverInput.Text == "")
            //{
            //    Message.Text = "Receiver can not be empty";
            //    return;
            //}
            //if (UmpireInput.Text == "")
            //{
            //    Message.Text = "Umpire can not be empty";
            //    return;
            //}
            //if (LinemanInput.Text == "")
            //{
            //    Message.Text = "Lineman can not be empty";
            //    return;
            //}

            //var matchId = ViewModel.SelectMatch.MatchID;
            //var server = ServerInput.Text;
            //var receiver = ReceiverInput.Text;
            //var umpire = UmpireInput.Text;
            //var lineman = LinemanInput.Text;
            //var court = AvaliableCourt.SelectedItem.ToString();
            //var round = RoundSelector.SelectedItem.ToString();
            //var selectcatagory = CategoriesSelector.SelectedItem.ToString();
            //CategoriesSelector.PlaceholderText = selectcatagory;
            //AvaliableCourt.PlaceholderText = court;
            //RoundSelector.PlaceholderText = round;
            //using (HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        var kvp = new List<KeyValuePair<string, string>>
            //        {
            //            new KeyValuePair<string,string>("matchId", matchId),
            //            new KeyValuePair<string,string>("player1", server),
            //            new KeyValuePair<string,string>("player2", receiver),
            //            new KeyValuePair<string,string>("catagory", selectcatagory),
            //            new KeyValuePair<string,string>("umpire", umpire),
            //            new KeyValuePair<string,string>("lineman", lineman),
            //            new KeyValuePair<string,string>("court", court),
            //            new KeyValuePair<string,string>("round", round),
            //        };
            //        HttpResponseMessage response = await client.PostAsync("http://localhost:3000/creategame", new FormUrlEncodedContent(kvp));
            //        if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
            //        {
            //            string responseBody = await response.Content.ReadAsStringAsync();
            //            var gameinfo = JObject.Parse(responseBody);
            //            //正确时创建赛事成功
            //            if ((string)gameinfo["ok"] != "0")
            //            {
            //                var all = gameinfo;
            //                var setId = (string)gameinfo["matchId"];
            //                var player1 = (string)gameinfo["player1"];
            //                var player2 = (string)gameinfo["player2"];
            //                var cata = (string)gameinfo["catagory"];
            //                var ump = (string)gameinfo["umpire"];
            //                //var line = (string)gameinfo["lineman"];
            //                var line = lineman;
            //                var cour = (string)gameinfo["court"];
            //                var rou = (string)gameinfo["round"];
            //                var date = System.DateTime.Now;
            //                List<string> score = new List<string>();
            //                ViewModel.AddGame(setId, player1, player2, cata, ump, line, date, date, date, cour, rou, "0-0", score, "-1");

            //                Frame.Navigate(typeof(GamesPage), ViewModel);
            //            }
            //            //不正确时输出错误信息
            //            else
            //            {
            //                Message.Text = (string)gameinfo["error"];
            //            }
            //        }
            //    }
            //    catch (HttpRequestException ex)
            //    {
            //        await new MessageDialog(ex.Message).ShowAsync();
            //    }
            //}

            //Frame.Navigate(typeof(GamesPage), ViewModel);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
