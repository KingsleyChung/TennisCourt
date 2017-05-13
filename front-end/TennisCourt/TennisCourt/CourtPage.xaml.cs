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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TennisCourt
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CourtPage : Page
    {
        public CourtPage()
        {
            this.InitializeComponent();
        }

        ViewModels.MatchesViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (ViewModels.MatchesViewModel)e.Parameter;
            ViewModel.initializeCourtStatus();


            //for testing
            //ViewModel = new ViewModels.MatchesViewModel();
            //ViewModel.AddUser("Kingsley", "15331429", "123123", "0");
            //List<Models.Games> tempGameList = new List<Models.Games>();
            //List<string> tempGameSocre = new List<string>();
            //tempGameList.Add(new Models.Games("1231", "鍾文杰", "费德勒", "Man's Single", "郑伟林", "", DateTime.Now, DateTime.Now, DateTime.Now, "1", "2nd", "0-5", tempGameSocre, "-1"));
            //tempGameList.Add(new Models.Games("1231", "纳达尔", "费德勒", "Man's Single", "郑伟林", "", DateTime.Now, DateTime.Now, DateTime.Now, "4", "2nd", "0-5", tempGameSocre, "-1"));
            //tempGameList.Add(new Models.Games("1231", "迪米", "费德勒", "Man's Single", "郑伟林", "", DateTime.Now, DateTime.Now, DateTime.Now, "6", "2nd", "0-5", tempGameSocre, "-1"));
            //ViewModel.AddMatch("中东网球公开赛", "1321123213", DateTime.Now, DateTime.Now, "Man's Single", "40", "-1", tempGameList);


            
        }
    }
}
