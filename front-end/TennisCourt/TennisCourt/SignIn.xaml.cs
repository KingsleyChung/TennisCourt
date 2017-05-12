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
    public sealed partial class SignIn : Page
    {
        private ViewModels.MatchesViewModel ViewModel;

        public SignIn()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Message.Opacity = 0;
            ViewModel = ((ViewModels.MatchesViewModel)e.Parameter);
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            //前段验证输入框是否为空
            var username = UsernameInput.Text;
            var password = PasswordInput.Text;
            var studentid = StudentIDInput.Text;
            if (username == "")
            {
                Message.Opacity = 1;
                Message.Text = "Username can not be empty!";
                return;
            }
            if (password == "")
            {
                Message.Opacity = 1;
                Message.Text = "Password can not be empty!";
                return;
            }
            if (studentid == "")
            {
                Message.Opacity = 1;
                Message.Text = "StudentID can not be empty!";
                return;
            }
            //向后端注册用户
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string,string>("username", username),
                        new KeyValuePair<string,string>("studentid", studentid),
                        new KeyValuePair<string,string>("password", password)
                    };
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3000/signin", new FormUrlEncodedContent(kvp));
                    if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var userinfo = JObject.Parse(responseBody);
                        //正确时跳转
                        if ((string)userinfo["ok"] != "0")
                        {
                            var name = (string)userinfo["username"];
                            var ps = (string)userinfo["password"];
                            var sid = (string)userinfo["studentId"];
                            var mode = (string)userinfo["mode"];
                            ViewModel.AddUser(name, sid, ps, mode);
                            Frame.Navigate(typeof(MainPage), ViewModel);
                        }
                        //不正确时输出错误信息
                        else
                        {
                            Message.Opacity = 1;
                            Message.Text = (string)userinfo["error"];
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
