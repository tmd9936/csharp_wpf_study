using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;


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

        public Point Position { get; set; }
        public Rectangle Rect { get; set; }

        public Product()
        {
            Rect = new Rectangle();
            Rect.Fill = new SolidColorBrush(Colors.Red);
            Rect.Width = 40;
            Rect.Height = 40;
        }
    }
}