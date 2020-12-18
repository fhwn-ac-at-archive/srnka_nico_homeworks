using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var conf = new Configuration(Int32.Parse(CanvasHeight.Text), Int32.Parse(CanvasWidth.Text), Int32.Parse(HeightBox.Text), Int32.Parse(WidthBox.Text));
            MazeDisplay mw = new MazeDisplay(conf)
            {
                Owner = GetWindow(this)
            };
            mw.Show();
        }
    }
}
