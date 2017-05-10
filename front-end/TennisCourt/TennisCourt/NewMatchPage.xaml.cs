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
    public sealed partial class NewMatchPage : Page
    {
        public NewMatchPage()
        {
            this.InitializeComponent();
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var title = Title.Text;
            var totalplayer = TotalPlayers.Text;
            if (title == "") {
                Title.Text = "Title can not be empty!";
                return;
            }
            if (totalplayer == "")
            {
                TotalPlayers.Text = "Player can not be zero!";
                return;
            }
            var box1 = ManSingle.IsChecked;
            var box2 = WomanSingle.IsChecked;
            var box3 = ManDouble.IsChecked;
            var box4 = WomanDouble.IsChecked;
            var box5 = MixDouble.IsChecked;
            /*if (box1 == false && box2 == false && box3 == false && box4 == false && box5 == false)
            {

            }*/
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
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("matchTitle", title),
                        new KeyValuePair<string,string>("date", StartDate.ToString()),
                        new KeyValuePair<string,string>("totalPlayers", totalplayer),
                        new KeyValuePair<string,string>("status", "-1"),
                        new KeyValuePair<string,string>("category", s)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3000/creatematch", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var matchinfo = JObject.Parse(responseBody);
                        //正确时创建赛事成功
                        if ((string)matchinfo["ok"] != "0")
                        {
                            
                        }
                        //不正确时输出错误信息
                        else
                        {
                            //Message.Text = (string)userinfo["error"];
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

        }

        private void test()
        {

        }
    }
}
