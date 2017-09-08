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
            var bitmap = BitmapHelpers.GeneratedBitmapByBitmapImageData("../../TestFile/TestFile.txt", 640, 480);
            //            var bitmap = new Bitmap("../../TestFile/Image.bmp");
            var bitmapImage = BitmapHelpers.ConvertByteArrayToBitmapImage(BitmapHelpers.ConvertBitmapToByteArray(bitmap));
            var image = new Image() { Source = bitmapImage };
            this.BitmapShowCanvas.Children.Add(image);
        }
    }
}
