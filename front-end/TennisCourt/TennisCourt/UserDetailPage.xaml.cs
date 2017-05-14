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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserDetailPage : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public UserDetailPage()
        {
            this.InitializeComponent();

            DispatcherTimer time1 = new DispatcherTimer();
            time1.Interval = new TimeSpan(0, 0, 10);
            time1.Tick += User_Reload;
            time1.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            reload(e);
            Server.Text = ViewModel.SelectSpecialSet.Server;
            Receiver.Text = ViewModel.SelectSpecialSet.Receiver;
            Umpire.Text = ViewModel.SelectSpecialSet.Umpire;
            Lineman.Text = ViewModel.SelectSpecialSet.Lineman;
            Category.Text = ViewModel.SelectSpecialSet.Category;
            Round.Text = ViewModel.SelectSpecialSet.Round;
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }

        private void User_Reload(object sender, object e)
        {
            tmp();
        }

        private async void tmp()
        {
            while (ViewModel.AllGameScore.Count > 0)
            {
                ViewModel.AllGameScore.RemoveAt(0);
            }

            if (ViewModel.SelectSpecialSet != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("http://www.zhengweimumu.cn:3000/returngame");
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
                                    if (setId == ViewModel.SelectSpecialSet.SetID)
                                    {
                                        var allresult = (JArray)allgame[i]["allresult"];
                                        var ballflag = (string)allgame[i]["flag"];
                                        var count = allresult.Count;
                                        int num = 1;
                                        string server = "", receiver = "";
                                        if (ballflag == "1")
                                        {
                                            server = (string)allgame[i]["player1"];
                                            receiver = (string)allgame[i]["player2"];
                                        }
                                        else if (ballflag == "0")
                                        {
                                            server = (string)allgame[i]["player2"];
                                            receiver = (string)allgame[i]["player1"];
                                        }
                                        for (int j = 0; j < count; j++)
                                        {
                                            var id = ViewModel.SelectSpecialSet.SetID + "/" + num.ToString();
                                            
                                            string res = (string)allresult[num - 1];
                                            var set1 = res.Substring(0, 1);
                                            var set2 = res.Substring(2, 1);
                                            var tmp = "game" + num.ToString();
                                            var allscore = (JArray)allgame[i][tmp];
                                            if (allscore == null) return;
                                            var score = (string)allscore[allscore.Count - 1];
                                            var score1 = score.Substring(0, 2);
                                            var score2 = score.Substring(3, 2);
                                            if (ballflag == "1") ballflag = "0";
                                            else ballflag = "1";
                                            ViewModel.AddGameScore(id, num, server, receiver, ballflag, set1, set2, score1, score2, "0");
                                            num++;
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
            }
        }

        private void reload(NavigationEventArgs e)
        {
            ViewModel = (ViewModels.MatchesViewModel)e.Parameter;
            tmp();
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            var photoFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Poster/Poster1.jpg"));
            dp.Properties.Title = "对阵双方：" + ViewModel.SelectSpecialSet.Server + " VS " + ViewModel.SelectSpecialSet.Receiver;
            dp.Properties.Description = "来自Tennis Court的赛事分享";
            dp.SetText("赛况：\n局分：" + ViewModel.AllGameScore[ViewModel.AllGameScore.Count - 1].ServerSet + "-" + ViewModel.AllGameScore[ViewModel.AllGameScore.Count - 1].ReceiverSet);
            dp.SetStorageItems(new List<StorageFile> { photoFile });
            deferral.Complete();
        }
    }
}
