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
using System.IO.Ports;

namespace Wpf_Port_20190504
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort com = new SerialPort("COM6", 115200);
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                com.Open();
                add("串口打开成功");
                com.DataReceived += Com_DataReceived;
            }
            catch (Exception e)
            {
                add("串口打开失败\n" + e.Message);
            }
        }

        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(100);
                int byl = com.BytesToRead;
                byte[] by = new byte[byl];
                int count = com.Read(by, 0, by.Length);
                Console.WriteLine("Len={0} co={1}", byl, count);
                if (count > 1)
                {
                    string str = "\r" + Encoding.GetEncoding("GBK").GetString(by);
                    Dispatcher.Invoke(new Action(() => {
                        add(str);
                    }));
                }
            }
            catch (Exception)
            {
            }
        }

        private void add(string v)
        {
            tbR.Text += v;
            tbR.ScrollToEnd();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Button_Click(null, null);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = "\n" + tbW.Text.ToString().Trim();
                add(str);
                byte[] by = Encoding.GetEncoding("GBK").GetBytes(str);
                com.Write(by, 0, by.Length);
            }
            catch (Exception)
            {
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                com.Close();
                Close();
            }
            catch (Exception)
            {
            }
            
        }
    }
}
