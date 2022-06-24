using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

        Image image;
        Button button;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;

            image = img;
            button = btnSuper;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            slider.Value = media.Position.TotalSeconds;
            lblCurrentTime.Content = media.Position.Duration().ToString(@"mm\:ss");
        }

        void LoadElements()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (media.Source == null)
            {
                fileDialog.Filter = "Videos|*.mp4";
                if (fileDialog.ShowDialog() == true)
                {
                    media.Source = new Uri(fileDialog.FileName);

                    media.LoadedBehavior = MediaState.Manual;
                    media.UnloadedBehavior = MediaState.Manual;
                    media.Volume = 1;
                    media.Play();
                    media.Pause();
                }
            }
            if (JsonHelper.Instance.JsonFile == null)
            {
                fileDialog.Filter = "Json Files|*.json";
                if (fileDialog.ShowDialog() == true)
                {
                    JsonHelper.Instance.LoadJson(fileDialog.FileName);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadElements();
                BmpHelper.Instance.Refresh();
                startSecond = (int)media.Position.TotalMilliseconds;
                this.media.Play();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar datos");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stopSecond = (int)media.Position.TotalMilliseconds; 
            this.media.Pause();
            foreach (var item in JsonHelper.Instance.GetFromTime(startSecond, stopSecond))
            {
                BmpHelper.Instance.ColorCircle(item.X, item.Y);
            }
            LoadToImg();
            //Thread thead = new Thread(new ThreadStart(LoadToImg));
            //thead.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LoadElements();
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
            media.Play();
            media.Pause();
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            JsonHelper.Instance.Duration = (int)media.NaturalDuration.TimeSpan.TotalMilliseconds;
            lblTotalTime.Content = media.NaturalDuration.TimeSpan.Duration().ToString(@"mm\:ss");
            timer.Start();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void VideoBtnTrigg_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = this.FindResource("VideoFull") as Storyboard;
            storyboard.Begin();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (gdImage.Children.Contains(image))
            {
                gdImage.Children.Remove(image);
                gdVideo.Children.Add(image);
                gdImage.Visibility = Visibility.Collapsed;
                uImg.Children.Remove(button);
                button.Content = "Separar";
                uVideo.Children.Add(button);
            }
            else
            {
                gdVideo.Children.Remove(image);
                gdImage.Children.Add(image);
                gdImage.Visibility = Visibility.Visible;
                uVideo.Children.Remove(button);
                button.Content = "Sobreponer";
                uImg.Children.Add(button);
            }            
        }

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
                //img.Dispatcher.Invoke(new Action(() => img.Source = source));
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }
    }
}
