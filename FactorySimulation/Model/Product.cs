using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorySimulation.Model
{
    public class Product
    {
        public int ID { get; set; }
        public bool IsOK { get; set; }

        public Product(int _ID)
        {
            ID = _ID;
            IsOK = true;
        }
    }
}
