using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BitmapWorkstation.Helper;
using Image = System.Windows.Controls.Image;

namespace BitmapWorkstation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UsingBitmapData_Click(object sender, RoutedEventArgs e)
        {
            string str = "王炎生";

            Encoding GB18030Encoding = Encoding.GetEncoding(54936);
            Encoding Windows1252Encoding = Encoding.GetEncoding(1252);
            var byteData = GB18030Encoding.GetBytes(str);
            string tStr = "";
            if (byteData != null)
            {
                for (int i = 0; i < byteData.Length; i++)
                {
                    tStr += byteData[i].ToString("X2");
                }
            }
            Console.WriteLine(tStr);

            string GB18030EncodingStr = GB18030Encoding.GetString(byteData);
            Console.WriteLine(GB18030EncodingStr);

            string Windows1252EncodingStr = Windows1252Encoding.GetString(byteData);
            Console.WriteLine(Windows1252EncodingStr);
            var covertData = Encoding.UTF8.GetBytes(Windows1252EncodingStr);

            var t1 = Encoding.Convert(Encoding.UTF8, Windows1252Encoding, covertData);
            var t2 = Encoding.Convert(GB18030Encoding, Encoding.UTF8, t1);
            var t3 = Encoding.UTF8.GetString(t2);
            Console.WriteLine(t3);



            string returnStr = "";
            if (covertData != null)
            {
                for (int i = 0; i < covertData.Length; i++)
                {
                    returnStr += covertData[i].ToString("X2");
                }
            }
            Console.WriteLine(returnStr);

        }
    }
}
