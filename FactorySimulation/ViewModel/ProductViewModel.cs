using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FactorySimulation.PModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace FactorySimulation.ViewModel
{
    public class ProductViewModel : ObservableObject
    {
        private ObservableCollection<Product> _product;
        public ObservableCollection<Product> Products
        {
            get => _product;
            set
            {
                _product = value;
                SetProperty(ref _product, value);
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }


        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>
            {
                new Product{ID = 1000, IsOK = true, Name="Bolt"},
                new Product{ID = 1001, IsOK = false, Name="Machine"},
                new Product{ID = 1002, IsOK = true, Name="Monitor"},
            };
        }
    }
}
