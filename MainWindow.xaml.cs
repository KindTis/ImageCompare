using System;
using System.Windows;
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
            Rectangle rect = new Rectangle(100, 100, 640, 480);
            Screenshot screen = new Screenshot();
            Image img = screen.CaptureRectangle(rect);
            img.Save("c:\\some.jpg");
        }
    }
}
