using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChosePort.xaml
    /// </summary>
    public partial class SelectPort : Window
    {
        public SelectPort(string[] availablePort)
        {
            InitializeComponent();
            foreach (var a in availablePort)
                listBox.Items.Add(a);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectPort_Ready();
        }

        private void SelectPort_Ready()
        {
            if (listBox.SelectedValue == null)
                return;
            this.DialogResult = true;
        }

        public string PortName { get { return listBox.SelectedValue.ToString(); } }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectPort_Ready();
        }
    }
}
