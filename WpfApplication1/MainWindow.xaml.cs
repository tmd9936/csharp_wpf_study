using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication1.Utill;

namespace WpfApplication1
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkThreadManager wtm;
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

            solidColorBrushes = new SolidColorBrush[3];
            solidColorBrushes[(int)WORK_STATE.START] = new SolidColorBrush(Colors.Green);
            solidColorBrushes[(int)WORK_STATE.PAUSE] = new SolidColorBrush(Colors.Yellow);
            solidColorBrushes[(int)WORK_STATE.STOP] = new SolidColorBrush(Colors.Red);

            List<ProgressBar> _progressBar = new List<ProgressBar>(9)
            {
                ProgressBar0,
                ProgressBar1,
                ProgressBar2,
                ProgressBar3,
                ProgressBar4,
                ProgressBar5,
                ProgressBar6,
                ProgressBar7,
                ProgressBar8
            };

            List<TextBlock> _boxes = new List<TextBlock>(9)
            {
                Box0,
                Box1,
                Box2,
                Box3,
                Box4,
                Box5,
                Box6,
                Box7,
                Box8
            };

            wtm = new WorkThreadManager(ref _progressBar, ref _boxes);
        }

        private void Btn_Start(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = solidColorBrushes[(int)WORK_STATE.START];
            wtm.WorkStart();
        }

        private void Btn_Pause(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = solidColorBrushes[(int)WORK_STATE.PAUSE];
            wtm.WorkPuase();
        }

        private void Btn_WorkEnd(object sender, RoutedEventArgs e)
        {
            WorkingMark.Fill = solidColorBrushes[(int)WORK_STATE.STOP];
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
