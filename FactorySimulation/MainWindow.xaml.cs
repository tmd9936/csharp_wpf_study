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
using FactorySimulation.Service;

namespace FactorySimulation
{
    public partial class MainWindow : Window
    {
        private TransferService transfer = null;

        private readonly SolidColorBrush[] solidColorBrushes;

        private enum WORK_STATE
        {
            START,
            PAUSE,
            STOP
        };

        public MainWindow()
        {
            InitializeComponent();

            transfer = TransferService.Instance;

            solidColorBrushes = new SolidColorBrush[3];
            solidColorBrushes[(int)WORK_STATE.START] = new SolidColorBrush(Colors.Green);
            solidColorBrushes[(int)WORK_STATE.PAUSE] = new SolidColorBrush(Colors.Yellow);
            solidColorBrushes[(int)WORK_STATE.STOP] = new SolidColorBrush(Colors.Red);

            List<ProgressBar> _progressBar = new List<ProgressBar>(9)
            {
                ProgressBar1,
                ProgressBar2,
                ProgressBar3,
                ProgressBar4,
                ProgressBar5,
                ProgressBar6,
                ProgressBar7
            };

            List<TextBlock> _boxes = new List<TextBlock>(9)
            {
                Box1,
                Box2,
                Box3,
                Box4,
                Box5,
                Box6,
                Box7
            };

            transfer.Initialize(_progressBar, _boxes);
        }

        private void Closed_Window(object sender, EventArgs e)
        {
            if (transfer != null)
                transfer.WorkFinish();
        }

        private void Btn_Start(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = solidColorBrushes[(int)WORK_STATE.START];

            transfer.WorkStart();
        }

        private void Btn_Pause(object sender, RoutedEventArgs e)
        {
            if (transfer.WorkPuase())
            {
                WorkingMark.Fill = solidColorBrushes[(int)WORK_STATE.PAUSE];
            }
        }

        private void Btn_Stop(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = solidColorBrushes[(int)WORK_STATE.STOP];

            transfer.WorkStop();
        }

        private void InputObject(object sender, MouseButtonEventArgs e)
        {
            transfer.InputObject();
        }

        private void ForceRemoval(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            int index = int.Parse(block.Tag.ToString());
            transfer.ForceRemoval(index);
        }

        private void ForceInputObject(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            int index = int.Parse(block.Tag.ToString());
            transfer.ForceInput(index);
        }
    }
}
