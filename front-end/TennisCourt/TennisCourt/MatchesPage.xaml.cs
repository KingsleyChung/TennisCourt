using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using TennisCourt.Models;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
    public sealed partial class MatchesPage : Page
    {
        public MatchesPage()
        {
            this.InitializeComponent();
        }

        private ViewModels.MatchesViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            ViewModel = (ViewModels.MatchesViewModel)e.Parameter;
            
            //remove AllMatches
            while (ViewModel.AllMatches.Count > 0)
            {
                ViewModel.AllMatches.RemoveAt(0);
            }
            
            //user mode
            if (ViewModel.CurrentUser.Mode == "0")
            {
                CommandBar.Visibility = Visibility.Collapsed;
            }

            //show all matches
            show_allMatches();
            
        }

        private void UpdateTile()
        {
            var updator = TileUpdateManager.CreateTileUpdaterForApplication();
            updator.Clear();
            foreach (var match in ViewModel.AllMatches)
            {
                XmlDocument tileXml = new XmlDocument();
                tileXml.LoadXml(File.ReadAllText("Tile.xml"));
                var tileText = tileXml.GetElementsByTagName("text");
                for (int i = 0; i < tileText.Count(); i = i + 3)
                {
                    ((XmlElement)tileText[i]).InnerText = match.MatchTitle;
                    if (match.Status == "0")
                    {
                        ((XmlElement)tileText[i + 1]).InnerText = "进行中";
                    }
                    else
                    {
                        ((XmlElement)tileText[i + 1]).InnerText = "已结束";
                    }
                    ((XmlElement)tileText[i + 2]).InnerText = match.Start_Date.ToString("yyyy-MM-dd");
                }
                TileNotification notification = new TileNotification(tileXml);
                updator.Update(notification);
            }
            updator.EnableNotificationQueue(true);
        }

        private async void show_allMatches()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://www.zhengweimumu.cn:3000/allmatch");
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var matchinfo = JObject.Parse(responseBody);
                        if ((string)matchinfo["ok"] != "0")
                        {
                            var allmatch = (JArray)matchinfo["match"];
                            for (int i = 0; i < allmatch.Count; i++)
                            {
                                var matchTitle = (string)allmatch[i]["matchTitle"];
                                var d1 = (string)allmatch[i]["startTime"];
                                var d2 = (string)allmatch[i]["endTime"];
                                var date1 = Convert.ToDateTime(d1);
                                var date2 = date1;
                                if (d2 != "") date2 = Convert.ToDateTime(d2);
                                var totalPlayers = (string)allmatch[i]["totalPlayers"];
                                var status = (string)allmatch[i]["status"];
                                var category = (string)allmatch[i]["category"];
                                var matchId = (string)allmatch[i]["matchId"];
                                //var allgames = allmatch[i]["games"];
                                List<Games> gameslist = new List<Games>();
                                ViewModel.AddMatch(matchTitle, matchId, date1, date2, category, totalPlayers, status, gameslist);
                            }


                            //Frame.Navigate(typeof(MainPage), ViewModel);
                        }
                        //update live tile
                        UpdateTile();
                    }
                }
                catch (HttpRequestException ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
            }
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
