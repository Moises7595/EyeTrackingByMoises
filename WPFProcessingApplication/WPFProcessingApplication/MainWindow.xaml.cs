using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFProcessingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        int startSecond = 0, stopSecond = 0;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            slider.Value = media.Position.TotalSeconds;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BmpHelper.Instance.Refresh();
            startSecond = (int)media.Position.TotalMilliseconds;
            this.media.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stopSecond = (int)media.Position.TotalMilliseconds;
            this.media.Pause();
            foreach (var item in JsonHelper.Instance.GetFromTime(startSecond, stopSecond))
            {
                BmpHelper.Instance.ColorBitmap(item.X, item.Y);
                LoadToImg();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                JsonHelper.Instance.LoadJson(fileDialog.FileName);
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = TimeSpan.FromSeconds(slider.Value);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string fileName = (e.Data as DataObject).GetFileDropList()[0].ToString();
            media.Source = new Uri(fileName);

            media.LoadedBehavior = MediaState.Manual;
            media.UnloadedBehavior = MediaState.Manual;
            media.Volume = 1;
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            JsonHelper.Instance.Duration = (int)media.NaturalDuration.TimeSpan.TotalMilliseconds;
            timer.Start();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        void LoadToImg()
        {
            IntPtr hBitmap = BmpHelper.Instance.Bmp.GetHbitmap();

            try
            {
                var source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
                    );
                img.Source = source;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }
    }
}
