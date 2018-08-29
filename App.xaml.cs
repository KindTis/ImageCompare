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
            var item01 = new CompareListItem() { strImagePath = "d:\\Projects\\ImageCompare\\src.jpg", rtRegion = new System.Drawing.Rectangle(0, 0, 300, 300), strKey = "Key" };
            var item02 = new CompareListItem() { strImagePath = "d:\\Projects\\ImageCompare\\dst.jpg", rtRegion = new System.Drawing.Rectangle(10, 100, 500, 500), strKey = "Key" };
            Items = new ObservableCollection<CompareListItem>()
            {
                item01, item02
            };
        }
    }
}
