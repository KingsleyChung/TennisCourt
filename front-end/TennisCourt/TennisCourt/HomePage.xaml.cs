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

namespace myTransfer
{
    public class StatusConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string res = "";
            string s = (string)value;
            if (s == "1") res = "准备中";
            else if (s == "0") res = "进行中";
            else res = "已结束";
            return res;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CategoriesConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string res = "";
            string s = (string)value;
            if (s[0] == '1') res += "男单  ";
            if (s[1] == '1') res += "女单  ";
            if (s[2] == '1') res += "男双  ";
            if (s[3] == '1') res += "女双  ";
            if (s[4] == '1') res += "混双  ";
            return res;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TotalConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string res = "";
            int num = (int)value;
            res = "Game " + num.ToString();
            return res;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Flag1Converter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string s = (string)value;
            if (s == "1") return 1.0;
            else return 0.0;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Flag2Converter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string s = (string)value;
            if (s == "1") return 0.0;
            else return 1.0;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ButtonConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string s = (string)value;
            if (s == "1") return true;
            else return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        ViewModels.MatchesViewModel ViewModel { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
            this.ViewModel = new ViewModels.MatchesViewModel();
            DispatcherTimer time = new DispatcherTimer();
            time.Interval = new TimeSpan(0, 0, 5);
            time.Tick += Time_Tick;
            time.Start();

            DispatcherTimer time1 = new DispatcherTimer();
            time1.Interval = new TimeSpan(0, 0, 30);
            time1.Tick += User_Reload;
            time1.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            reload();
        }

        private async void reload()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:3000/returngame");
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
                                var date1 = Convert.ToDateTime(d1);
                                var date2 = date1;
                                var d2 = (string)allgame[i]["endTime"];
                                if (d2 != "") date2 = Convert.ToDateTime(d2);
                                List<string> score = new List<string>();
                                ViewModel.AddSpecialGame(setId, player1, player2, cata, ump, line, date, date1, date2, cour, rou, res, score, status);

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

        private void User_Reload(object sender, object e)
        {
            reload();
        }

        private void Time_Tick(object sender, object e)
        {
            int i = PhotoGallery.SelectedIndex;
            i++;
            if (i >= PhotoGallery.Items.Count)
            {
                i = 0;
            }
            PhotoGallery.SelectedIndex = i;
        }

        private void MatchOverview_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectSpecialSet = (Models.Games)(e.ClickedItem);
            Frame.Navigate(typeof(UserDetailPage), ViewModel);
        }
    }
}
