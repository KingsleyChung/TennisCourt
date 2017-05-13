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

        private async void Start_Click(object sender, RoutedEventArgs e)
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

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchId", ViewModel.SelectSpecialSet.SetID)
                    };
                        HttpResponseMessage response = await client.PostAsync("http://www.zhengweimumu.cn:3000/matchgame", new FormUrlEncodedContent(kvp));
                        if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var info = JObject.Parse(responseBody);
                            if ((string)info["ok"] != "0")
                            {
                                Frame.Navigate(typeof(DetailPage), ViewModel);
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
