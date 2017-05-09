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
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
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
    }
}
