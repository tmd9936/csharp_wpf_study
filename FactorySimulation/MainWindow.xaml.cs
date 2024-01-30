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
using FactorySimulation.Utill;
using FactorySimulation.View;


/*
 * 1. 버퍼는 말그대로 버퍼라 아무것도 안 함
 * 2. 투입구 배출구도 공정이라 아무것도 없는게 아님
 * 3. 2개의 물품이 작업 중이라면 2개가 모두 완료된 상태에서 트랜스퍼가 움직일 수 있음
 * 4. 일시정지 중에 물건을 빼면 프로그래스 바는 그대로 둬야 함
 * 5. 다시 투입되면 그 프로그래스바 그대로 공정이 동작
 * 6. 일지 정지중에 싸이클 버튼을 누르면 그 스테이지의 작업만 원료 시킴, 자동 이동 X
 * 7. 트랜스퍼 앞으로 가고 뒤로도 갈 수 있어야 함
 */

namespace FactorySimulation
{
    public partial class MainWindow : Window
    {
        private readonly TransferService transfer = null;

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

            transfer.Initialize(_progressBar, _boxes);
            LogManager.Instance.Initialize(LogTextBox);

            ProductWindow.Instance.Width = ProductResultGrid.Width * 0.9;
            ProductWindow.Instance.Height = ProductResultGrid.Height * 0.9;

            ProductResultGrid.Children.Add(ProductWindow.Instance);

            //MainCanvas.Children.Add(rectangle);
            //Canvas.SetTop(rectangle, 276);
            //Canvas.SetLeft(rectangle, 27);
        }

        private void Closed_Window(object sender, EventArgs e)
        {
            if (transfer != null)
            {
                transfer.Destroyed();
            }
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
            transfer.InputObject(ProductNameTextBox.Text);
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
            transfer.ForceInput(ProductNameTextBox.Text, index);
        }

        private void OpenProductData(object sender, RoutedEventArgs e)
        {
            if (ProductWindow.Instance.Visibility == Visibility.Visible)
                ProductWindow.Instance.Visibility = Visibility.Hidden;
            else
                ProductWindow.Instance.Visibility = Visibility.Visible;
        }

        private void CanvasMouseEvent(object sender, MouseButtonEventArgs e)
        {
            Point point = Mouse.GetPosition(Application.Current.MainWindow);
        }

        private void CycleEvent(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = int.Parse(btn.Tag.ToString());
            transfer.CycleProduct(index);
        }
    }
}
