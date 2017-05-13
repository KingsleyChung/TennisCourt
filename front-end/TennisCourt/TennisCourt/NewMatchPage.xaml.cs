﻿using Newtonsoft.Json.Linq;
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
using System.Diagnostics;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewMatchPage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public NewMatchPage()
        {
            this.InitializeComponent();
            Message.Opacity = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Message.Opacity = 0;
            if ((ViewModels.MatchesViewModel)e.Parameter != null)
            {
                ViewModel = (ViewModels.MatchesViewModel)e.Parameter;
                if (ViewModel.SelectMatch != null)
                {
                    Title.Text = ViewModel.SelectMatch.MatchTitle;
                    List<CheckBox> checkBoxList = new List<CheckBox>();
                    checkBoxList.Add(MenSingle);
                    checkBoxList.Add(WomenSingle);
                    checkBoxList.Add(MenDouble);
                    checkBoxList.Add(WomenDouble);
                    checkBoxList.Add(MixDouble);
                    for (int i = 0; i < 5; i++)
                    {
                        if (ViewModel.SelectMatch.Categories[i] == '1')
                        {
                            checkBoxList.ElementAt(i).IsChecked = true;
                        }
                    }
                    StartDate.Date = ViewModel.SelectMatch.Start_Date;
                    EndDate.Date = ViewModel.SelectMatch.End_Date;
                    TotalPlayers.Text = ViewModel.SelectMatch.Total_Player;
                }
                else
                {
                    Finish.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var title = Title.Text;
            var totalplayer = TotalPlayers.Text;
            if (title == "")
            {
                Message.Opacity = 1;
                Message.Text = "Title can not be empty!";
                return;
            }
            if (totalplayer == "")
            {
                Message.Opacity = 1;
                Message.Text = "Player can not be zero!";
                return;
            }
            var box1 = MenSingle.IsChecked;
            var box2 = WomenSingle.IsChecked;
            var box3 = MenDouble.IsChecked;
            var box4 = WomenDouble.IsChecked;
            var box5 = MixDouble.IsChecked;
            if (box1 == false && box2 == false && box3 == false && box4 == false && box5 == false)
            {
                Message.Opacity = 1;
                Message.Text = "Catalory can not empty!";
                return;
            }
            string s = "";
            if (box1 == false) s += "0";
            else s += "1";
            if (box2 == false) s += "0";
            else s += "1";
            if (box3 == false) s += "0";
            else s += "1";
            if (box4 == false) s += "0";
            else s += "1";
            if (box5 == false) s += "0";
            else s += "1";
            string startdate = StartDate.Date.DateTime.ToString();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchTitle", title),
                        new KeyValuePair<string,string>("date", startdate),
                        new KeyValuePair<string,string>("totalPlayers", totalplayer),
                        new KeyValuePair<string,string>("status", "1"),
                        new KeyValuePair<string,string>("category", s)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3000/creatematch", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(responseBody);
                        var matchinfo = JObject.Parse(responseBody);
                        //正确时创建赛事成功
                        if ((string)matchinfo["ok"] != "0")
                        {
                            var all = matchinfo;
                            var matchTitle = (string)matchinfo["matchTitle"];
                            var fa = (string)matchinfo["date"];
                            var date = Convert.ToDateTime(fa);
                            var totalPlayers = (string)matchinfo["totalPlayers"];
                            var status = (string)matchinfo["status"];
                            var category = (string)matchinfo["category"];
                            var matchId = (string)matchinfo["matchId"];
                            List<Games> gameslist = new List<Games>();
                            ViewModel.AddMatch(matchTitle, matchId, date, date, category, totalPlayers, status, gameslist);

                            //ViewModel.SelectedItem = (Models.MatchesViewModel)(e.ClickedItem);
                            Frame.Navigate(typeof(MatchesPage), ViewModel);
                        }
                        //不正确时输出错误信息
                        else
                        {
                            Message.Opacity = 1;
                            Message.Text = (string)matchinfo["error"];
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
            {
                Frame.GoBack();
            }
        }

        private async void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectMatch.MatchID != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectMatch.MatchID)
                    };
                        HttpResponseMessage response = await client.PostAsync("http://localhost:3000/endmatch", new FormUrlEncodedContent(kvp));
                        if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine(responseBody);
                            var matchinfo = JObject.Parse(responseBody);
                            //正确时创建赛事成功
                            if ((string)matchinfo["ok"] != "0")
                            {
                                Frame.Navigate(typeof(MatchesPage), ViewModel);
                            }
                            //不正确时输出错误信息
                            else
                            {
                                Message.Opacity = 1;
                                Message.Text = (string)matchinfo["error"];
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
}
