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

            while (ViewModel.AllSpecialSets.Count > 0)
            {
                ViewModel.AllSpecialSets.RemoveAt(0);
            }

            if (ViewModel.SelectMatch != null)
            {
                for (int i = 0; i < ViewModel.AllMatches.Count; i++)
                {
                    if (ViewModel.AllMatches.ElementAt(i).MatchTitle == ViewModel.SelectMatch.MatchTitle)
                    {
                        for (int j = 0; j < ViewModel.AllMatches.ElementAt(i).Game.Count; j++) {
                            var tmp = ViewModel.AllMatches.ElementAt(i).Game;
                            ViewModel.AddSpecialGame(tmp[j].SetID, tmp[j].Server, tmp[j].Receiver, tmp[j].Category, tmp[j].Umpire, tmp[j].Lineman, tmp[j].Date, tmp[j].Start_Time, tmp[j].End_Time, tmp[j].Court, tmp[j].Round, tmp[j].Result, tmp[j].GameScore, tmp[j].Status);
                        }
                    }
                }
            }
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewGamePage), ViewModel);
        }

        private void games_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
