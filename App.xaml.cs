using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace ImageCompare
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
    }

    public class MockMasterViewModel
    {

        public ObservableCollection<CompareListItem> Items { get; set; }

        public MockMasterViewModel()
        {
            var item01 = new CompareListItem() { ImagePath = "d:\\Projects\\ImageCompare\\src.jpg", Region = "{ x: 100, y: 200, width: 500, height: 500 }", Key = "Key" };
            var item02 = new CompareListItem() { ImagePath = "d:\\Projects\\ImageCompare\\dst.jpg", Region = "{ x: 0, y: 0, width: 300, height: 250 }", Key = "Key" };
            Items = new ObservableCollection<CompareListItem>()
            {
                item01, item02
            };
        }
    }
}
