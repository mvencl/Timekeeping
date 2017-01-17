using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;
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

namespace Timekeeping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port;
        DispatcherTimer theTimer;
        Stopwatch stopWatch;

        Brush ready = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF00FF3A");
        Brush active = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFF0000");

        bool IsRunningA = false;
        bool IsRunningB = false;

        DataClass _data = null;

        PrintWindow show = null;

        public MainWindow()
        {
            var setting = new SelectPort(SerialPort.GetPortNames());
            setting.ShowDialog();
            if (setting.DialogResult != true)
            {
                this.Close();
                return;
            }
                
            
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            try
            {
                // Open port. We assume plugged into COM1
                this.port = new SerialPort(setting.PortName);
                this.port.Open();
                // Pull switch input line to idle state
                this.port.RtsEnable = true;
                stopWatch = new Stopwatch();
                this.theTimer = new DispatcherTimer();
                this.theTimer.Interval = TimeSpan.FromMilliseconds(10);
                this.theTimer.Tick += new EventHandler(timer_Tick);
                theTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při inicializaci portu. /n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (show != null)
                show.Close();
        }

        /// <summary>
        /// Timer Event Handler.
        /// Process each timer interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            SetTime(String.Format("{0:00}:{1:00}.{2:00}", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10));     

            if (port.CtsHolding == true)
            {
                IsRunningA = false;
                Stop(false);
                powerA.Fill = active;
            }
            else
                powerA.Fill = ready;

            if (port.DsrHolding == true)
            {
                IsRunningB = false;
                Stop(false);
                powerB.Fill = active;
            }
            else
                powerB.Fill = ready;

            if (port.CDHolding == true)
            {
                if (IsRunningA || IsRunningB)
                    return;
                Start();
            }


        }

        private void Start()
        {
            stopWatch.Reset();
            stopWatch.Start();           
            IsRunningA = true;
            IsRunningB = true;
        }

        private void Stop(bool force)
        {
            if ((!IsRunningA && !IsRunningB) || force)
            {
                if (stopWatch.IsRunning)
                {
                    stopWatch.Stop();
                    IsRunningA = false;
                    IsRunningB = false;
                    Data.Runs.Add(new DataClass.Run() { CreatedDate = DateTime.Now, Name = "", TimeA = labelA.Content.ToString(), TimeB = labelB.Content.ToString() });
                    Data.Save();
                }
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunningA || IsRunningB)
                return;
            Start();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Stop(true);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunningA || IsRunningB)
                return;

            stopWatch.Reset();
            string time = String.Format("{0:00}:{1:00}.{2:00}", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds / 10);

            labelA.Content = time;
            labelB.Content = time;

            if (show != null)
            {
                show.firstTime = time;
                show.endTime = time;
            }

        }

        private void SetTime(string time)
        {
            if (IsRunningA)
                labelA.Content = time;
            if (IsRunningB)
                labelB.Content = time;

            //for show
            if(show != null)
            {
                if(IsRunningA && IsRunningB)
                {
                    show.firstTime = time;
                    show.endTime = time;
                }
                if (!IsRunningA && IsRunningB || IsRunningA && !IsRunningB)
                {
                    show.endTime = time;
                }
            }

        }

        private DataClass Data
        {
            get
            {
                if (_data == null)
                    _data = DataClass.Load();
                return _data;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if(show == null)
            {
                show = new PrintWindow();
                show.Closed += Show_Closed;
            }            
                
            show.Show();
        }

        private void Show_Closed(object sender, EventArgs e)
        {
            show = null;
        }
    }
}
