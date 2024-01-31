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
using System.Windows.Shapes;

namespace FactorySimulation.View
{
    /// <summary>
    /// ProgressbarWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProgressbarWindow : UserControl
    {
        private static ProgressbarWindow instence = null;
        public static ProgressbarWindow Instance
        {
            get
            {
                if (instence == null)
                {
                    instence = new ProgressbarWindow();
                }
                return instence;
            }
        }

        private ProgressbarWindow()
        {
            InitializeComponent();

            ProgressBars = new List<ProgressBar>(9);

            Grid mainGrid = new Grid();
            Grid.SetRow(mainGrid, 0);

            for (int i = 0; i < 9; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mainGrid.ColumnDefinitions[i].Width = new GridLength(10, GridUnitType.Star);

                Grid childGrid = new Grid();
                Grid.SetColumn(childGrid, i);

                ProgressBar progressBar = new ProgressBar();
                progressBar.Name = "ProgressBar" + i;
                progressBar.Background = new SolidColorBrush(Colors.Black);
                progressBar.BorderBrush = new SolidColorBrush(Colors.DarkGreen);
                progressBar.Margin = new Thickness(10, 60, 10, 10);

                ProgressBars.Add(progressBar);

                childGrid.Children.Add(progressBar);

                mainGrid.Children.Add(childGrid);
            }

            AddChild(mainGrid);

        }

        public List<ProgressBar> ProgressBars { get; set; }
    }
}
