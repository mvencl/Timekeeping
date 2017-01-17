using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Timekeeping
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintWindow()
        {
            InitializeComponent();
        }
        public string firstTime { get { return firstTimeLabel.Content.ToString(); } set { firstTimeLabel.Content = value; } }
        public string endTime { get { return endTimeLabel.Content.ToString(); } set { endTimeLabel.Content = value; } }
    }
   
}
