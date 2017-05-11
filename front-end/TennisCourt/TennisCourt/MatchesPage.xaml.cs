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

namespace myTransfer
{
    //public class StatusConverter : IValueConverter
    //{
    //    object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        string res = "";
    //        string s = (string)value;
    //        if (s == "-1") res = "Prepare";
    //        else if (s == "0") res = "Play";
    //        else res = "End";
    //        return res;
    //    }

    //    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class CategoriesConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string res = "";
            string s = (string)value;
            if (s[0] == '1') res += "Man-Single";
            if (s[1] == '1') res += "Woman-Single";
            if (s[2] == '1') res += "Men-Double";
            if (s[3] == '1') res += "Women-Dobule";
            if (s[4] == '1') res += "Mix-Double";
            return res;
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
    public sealed partial class MatchesPage : Page
    {
        public MatchesPage()
        {
            this.InitializeComponent();
        }

        ViewModels.MatchesViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if ((string)e.Parameter == "User")
            {
                CommandBar.Visibility = Visibility.Collapsed;
            }
        }

        private void AddMatch_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewMatchPage));
        }

        private void matches_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
