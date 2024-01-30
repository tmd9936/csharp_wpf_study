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
using System.Windows.Threading;
using System.ComponentModel;
using System.Media;
using FactorySimulation.ViewModel;
using FactorySimulation.PModel;

namespace FactorySimulation.View
{
    /// <summary>
    /// ProductWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductWindow : UserControl
    {
        private static ProductWindow instence = null;
        public static ProductWindow Instance
        {
            get
            {
                if (instence == null)
                {
                    instence = new ProductWindow();
                }
                return instence;
            }
        }

        private ProductWindow()
        {
            InitializeComponent();
            ProductViewModels = new ProductViewModel();
            ProductDataGrid.DataContext = ProductViewModels;

        }

        public void AddProductResultInfo(Product _product)
        {
            _ = Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                ProductViewModels.Products.Add(_product);
            }));
        }

        private ProductViewModel ProductViewModels { get; set; }

    }
}
