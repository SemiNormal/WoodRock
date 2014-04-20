using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WoodRock
{
    /// <summary>
    /// Interaction logic for _UnitDisplay.xaml
    /// </summary>
    public partial class _UnitDisplay : UserControl
    {
        public Models.Human Human { get; set; }
        
        public _UnitDisplay()
        {
            InitializeComponent();
        }
    }
}
