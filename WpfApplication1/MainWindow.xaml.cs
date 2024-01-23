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
using WpfApplication1.Utill;

namespace WpfApplication1
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isInit = false;
        private WorkThreadManager wtm;
        public MainWindow()
        {
            InitializeComponent();

            List<ProgressBar> _progressBar = new List<ProgressBar>(9);
            _progressBar.Add(ProgressBar0);
            _progressBar.Add(ProgressBar1);
            _progressBar.Add(ProgressBar2);
            _progressBar.Add(ProgressBar3);
            _progressBar.Add(ProgressBar4);
            _progressBar.Add(ProgressBar5);
            _progressBar.Add(ProgressBar6);
            _progressBar.Add(ProgressBar7);
            _progressBar.Add(ProgressBar8);

            List<TextBlock> _boxes = new List<TextBlock>(9);
            _boxes.Add(Box0);
            _boxes.Add(Box1);
            _boxes.Add(Box2);
            _boxes.Add(Box3);
            _boxes.Add(Box4);
            _boxes.Add(Box5);
            _boxes.Add(Box6);
            _boxes.Add(Box7);
            _boxes.Add(Box8);

            wtm = new WorkThreadManager(_progressBar, _boxes);

            Rectangle rect = new Rectangle();
        }

        private void Btn_Start(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = new SolidColorBrush(Colors.Green);
            wtm.WorkStart();
        }

        private void Btn_Pause(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = new SolidColorBrush(Colors.Yellow);
            wtm.WorkPuase();
        }

        private void Btn_WorkEnd(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = new SolidColorBrush(Colors.Red);
            wtm.WorkStop();
        }

        private void Closed_Window(object sender, EventArgs e)
        {
            if (wtm != null)
                wtm.WorkingFinish();
        }

        private void ForceRemoval(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            
            int index = int.Parse(block.Tag.ToString());

            wtm.ForceStop(index);
        }
    }
}
