using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FactorySimulation.Model
{
    public class Product
    {
        public int ID { get; set; }
        public bool IsOK { get; set; }

        public Product(int _id)
        {
            ID = _id;
            IsOK = true;
        }

    }
}

namespace FactorySimulation.PModel
{
    public class Product : ObservableObject
    {
        private int _id;
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private bool _isOK;
        public bool IsOK
        {
            get => _isOK;
            set => SetProperty(ref _isOK, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

    }
}