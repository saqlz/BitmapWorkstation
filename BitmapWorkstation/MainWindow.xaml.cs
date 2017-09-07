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
using BitmapWorkstation.Helper;

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
            //var bitmap = ByteArrayToBitmap.GeneratedBitmap("../../TestFile/TestFile.txt", 640, 480);
            //var bitmapImage = ByteArrayToBitmap.ConvertBitmapToImage(bitmap);
            var bitmapImage = ByteArrayToBitmap.GetBitmapFromMemory("../../TestFile/TestFile.txt", 640, 480);
            var image = new Image() {Source = bitmapImage };
            this.BitmapShowCanvas.Children.Add(image);
        }
    }
}
