using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using ShareX.ScreenCaptureLib;

namespace ImageCompare
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScreenCapture(object sender, RoutedEventArgs e)
        {
            Rectangle rect = new Rectangle();
            RegionCaptureOptions option = new RegionCaptureOptions();
            RegionCaptureTasks.GetRectangleRegion(out rect, option);
            Image1.Text = rect.ToString();
            Screenshot screen = new Screenshot();
            Image copyImg = screen.CaptureRectangle(rect);
            copyImg.Save("c:\\some.jpg");
        }
    }
}
