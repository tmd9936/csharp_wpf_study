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


namespace FactorySimulation.View
{
    /// <summary>
    /// CycleButtonWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CycleButtonWindow : UserControl
    {
        private static CycleButtonWindow instence = null;
        public static CycleButtonWindow Instance
        {
            get
            {
                if (instence == null)
                {
                    instence = new CycleButtonWindow();
                }
                return instence;
            }
        }

        private CycleButtonWindow()
        {
            InitializeComponent();

            //<Button Margin="0" Background="#FFFF4545" FontSize="15" FontWeight="Bold" Foreground="White" Content="Cycle" Tag="3" Click="CycleEvent" >

            CycleButtons = new List<Button>(9);

            Grid mainGrid = new Grid();
            Grid.SetRow(mainGrid, 0);

            //FFFF4545
            Color color = new Color
            {
                A = 0xFF,
                R = 0xFF,
                G = 0x45,
                B = 0x45
            };

            for (int i = 0; i < 9; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mainGrid.ColumnDefinitions[i].Width = new GridLength(10, GridUnitType.Star);

                Grid childGrid = new Grid();
                Grid.SetColumn(childGrid, i);

                Button button = new Button();
                button.Name = "CycleButton" + i;
                button.Margin = new Thickness(0);
                button.Background = new SolidColorBrush(color);
                button.FontSize = 15;
                button.FontWeight = FontWeight.FromOpenTypeWeight(FontWeights.Bold.ToOpenTypeWeight());
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Content = "Cycle";
                button.Tag = i;

                button.Click += new RoutedEventHandler(CycleEvent);

                CycleButtons.Add(button);
                childGrid.Children.Add(button);
                mainGrid.Children.Add(childGrid);
            }

            CycleButtons[0].Visibility = Visibility.Hidden;
            CycleButtons[8].Visibility = Visibility.Hidden;

            AddChild(mainGrid);

        }

        private void CycleEvent(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = int.Parse(btn.Tag.ToString());
            TransferService.Instance.CycleProduct(index);
        }

        public List<Button> CycleButtons { get; set; }
    }
}
