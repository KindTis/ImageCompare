using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using ShareX.ScreenCaptureLib;
using XnaFan.ImageComparison;

namespace ImageCompare
{
    public enum Key
    {
        MOUSE_LEFT,
        MOUSE_RIGHT,
        KEY_S
    }

    public class CompareListItem
    {
        public string strImagePath { get; set; }
        public Rectangle rtRegion { get; set; }
        public string strKey { get; set; }
        public Key Key { get; set; }
        public System.Drawing.Point MousePoint { get; set; }
    }

    public partial class MainWindow : Window
    {
        MouseHook mMouseHook = new MouseHook();
        KeyboardHook mKeyboardHook = new KeyboardHook();
        public Rectangle CaptureRegion = new Rectangle();
        private Screenshot mScreenShot = new Screenshot();
        private List<CompareListItem> mCompareListItems = new List<CompareListItem>();
        private string mSubDir = "Images";
        private int mCaptureCount = 0;
        private Timer mRunTimer;

        public MainWindow()
        {
            InitializeComponent();

            mMouseHook.LeftButtonUp += MouseHookCallback;
            mKeyboardHook.KeyUp += KeyboardHookCallback;

            lbCompareList.ItemsSource = mCompareListItems;
            Directory.CreateDirectory(mSubDir);
        }

        private void ScreenCapture(object sender, RoutedEventArgs e)
        {
            RegionCaptureOptions option = new RegionCaptureOptions();
            RegionCaptureTasks.GetRectangleRegion(out CaptureRegion, option);
            Image captureImage = mScreenShot.CaptureRectangle(CaptureRegion);
            if (captureImage != null)
            {
                string savePath = mSubDir + "\\Capture" + mCaptureCount.ToString("d3") + ".jpg";
                mCaptureCount++;
                captureImage.Save(savePath);
                mCompareListItems.Add(new CompareListItem()
                {
                    strImagePath = Path.GetFullPath(savePath),
                    rtRegion = CaptureRegion,
                    strKey = ""
                });
                lbCompareList.Items.Refresh();
            }
        }

        private void SetKey(object sender, RoutedEventArgs e)
        {
            if (lbCompareList.Items.Count == 0 ||
                lbCompareList.SelectedIndex == -1)
                return;

            mMouseHook.Install();
            mKeyboardHook.Install();

            btSetKey.Content = "Waiting...";
        }

        private void MouseHookCallback(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            if (!mMouseHook.IsInstalled() || !mKeyboardHook.IsInstalled())
            {
                Debug.WriteLine("Mouse Hook is not installed!!");
                return;
            }

            CompareListItem item = lbCompareList.Items[lbCompareList.SelectedIndex] as CompareListItem;
            item.strKey = Key.MOUSE_LEFT.ToString();
            item.Key = Key.MOUSE_LEFT;
            item.MousePoint = new System.Drawing.Point(mouseStruct.pt.x, mouseStruct.pt.y);

            lbCompareList.Items.Refresh();
            mMouseHook.Uninstall();
            mKeyboardHook.Uninstall();
            btSetKey.Content = "Set Key";
        }

        private void KeyboardHookCallback(KeyboardHook.VKeys key)
        {
            if (!mMouseHook.IsInstalled() || !mKeyboardHook.IsInstalled())
            {
                Debug.WriteLine("Mouse Hook is not installed!!");
                return;
            }

            CompareListItem item = lbCompareList.Items[lbCompareList.SelectedIndex] as CompareListItem;
            item.strKey = Key.KEY_S.ToString();
            item.Key = Key.KEY_S;

            lbCompareList.Items.Refresh();
            mMouseHook.Uninstall();
            mKeyboardHook.Uninstall();
            btSetKey.Content = "Set Key";
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            if (mRunTimer == null)
            {
                mRunTimer = new Timer();
                mRunTimer.Tick += new EventHandler(ExcuteList);
                mRunTimer.Interval = 2000;
                mRunTimer.Start();
                btRun.Content = "Stop";
            }
            else
            {
                mRunTimer.Stop();
                mRunTimer = null;
                btRun.Content = "Run";
            }
        }
        
        private void ExcuteList(object sender, EventArgs e)
        {
            //Process[] notepads = Process.GetProcessesByName("fifa4zf");
            //if (notepads.Length == 0) return;
            //if (notepads[0] != null)
            //{
            //    SendMessage(notepads[0].MainWindowHandle, 0x100, 0x1B, null);
            //    SendMessage(notepads[0].MainWindowHandle, 0x104, 0x1B, null);
            //}

            int i = 0;
            foreach (CompareListItem item in mCompareListItems)
            {
                Image copyImg = mScreenShot.CaptureRectangle(item.rtRegion);
                if (copyImg == null)
                {
                    Debug.WriteLine("failed to Capture {0}", i);
                    return;
                }
                string tempImagePath = mSubDir + "\\temp" + i.ToString() + ".jpg";
                copyImg.Save(tempImagePath);

                float diff = ImageTool.GetPercentageDifference(item.strImagePath, tempImagePath);

                if (diff < 0.4)
                {
                    Debug.WriteLine("{0} diff [{1}] E", i, diff);
                    if (item.Key == Key.MOUSE_LEFT)
                    {
                        Debug.WriteLine("mouse point {0}, {1}", (uint)item.MousePoint.X, (uint)item.MousePoint.Y);
                        MouseHook.SetCursorPos((uint)item.MousePoint.X, (uint)item.MousePoint.Y);
                        MouseHook.mouse_event(0x0002 | 0x0004, 0, 0, 0, 0);
                    }
                    else if (item.Key == Key.KEY_S)
                    {
                        KeyboardHook.keybd_event(0x1B, 0x45, 0, 0);
                    }
                }
                else
                {
                    Debug.WriteLine("{0} diff [{1}]", i, diff);
                }

                i++;
            }
        }
    }
}
