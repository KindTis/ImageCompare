using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using ShareX.ScreenCaptureLib;
using XnaFan.ImageComparison;
using System.IO;

namespace ImageCompare
{
    public class CompareListItem
    {
        public string ImagePath { get; set; }
        public string Region { get; set; }
        public string Key { get; set; }
    }

    public partial class MainWindow : Window
    {
        public Rectangle CaptureRegion = new Rectangle();
        private Screenshot mScreenShot = new Screenshot();
        private List<CompareListItem> mCompareListItems = new List<CompareListItem>();
        private string mSubDir = "Images";
        private int mCaptureCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            CompareList.ItemsSource = mCompareListItems;
            Directory.CreateDirectory(mSubDir);
        }

        private void ScreenCapture(object sender, RoutedEventArgs e)
        {
            RegionCaptureOptions option = new RegionCaptureOptions();
            RegionCaptureTasks.GetRectangleRegion(out CaptureRegion, option);
            Image copyImg = mScreenShot.CaptureRectangle(CaptureRegion);
            if (copyImg != null)
            {
                string savePath = mSubDir + "\\Capture" + mCaptureCount.ToString("d3") + ".jpg";
                mCaptureCount++;
                copyImg.Save(savePath);
                mCompareListItems.Add(new CompareListItem()
                {
                    ImagePath = Path.GetFullPath(savePath),
                    Region = CaptureRegion.ToString(),
                    Key = ""
                });
                CompareList.Items.Refresh();
            }
        }

        private void DuplicateCapture(object sender, RoutedEventArgs e)
        {
            Image copyImg = mScreenShot.CaptureRectangle(CaptureRegion);
            copyImg.Save("c:\\dst.jpg");
        }

        private void CompareImage(object sender, RoutedEventArgs e)
        {
            int difference = (int)(ImageTool.GetPercentageDifference("c:\\src.jpg",
                "c:\\dst.jpg") * 100);
        }
    }
}
