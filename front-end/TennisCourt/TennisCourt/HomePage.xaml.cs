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
    public class StatusConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            string res = "";
            string s = (string)value;
            if (s == "-1") res = "准备中";
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
            if (s[0] == '1') res += "Man-Single ";
            if (s[1] == '1') res += "Woman-Single ";
            if (s[2] == '1') res += "Men-Double ";
            if (s[3] == '1') res += "Women-Dobule ";
            if (s[4] == '1') res += "Mix-Double";
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
            ViewModel.SelectSet = (Models.Games)(e.ClickedItem);
        }
    }
}
