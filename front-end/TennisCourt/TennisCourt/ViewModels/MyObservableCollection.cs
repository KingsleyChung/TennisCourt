using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.ViewModels
{
    class MyObservableCollection<T> : ObservableCollection<T>
    {
        new public void SetItem(int index, T item)
        {
            base.SetItem(index, item);
        }
    }
}
