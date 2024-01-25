using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FactorySimulation.PModel;

namespace FactorySimulation.ViewModels
{
    public class ProductViewModel : ObservableObject
    {
        private IList<Product> _product;
        public IList<Product> Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public ProductViewModel()
        {
            Product = new List<Product>
            {
                new Product{ID = 1000, IsOK = true, Name="Bolt"},
                new Product{ID = 1001, IsOK = true, Name="Machine"},
                new Product{ID = 1002, IsOK = true, Name="Monitor"},
            };
        }
    }
}
