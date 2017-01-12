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

namespace GOL
{
    /// <summary>
    /// Interaction logic for AboutGolWin.xaml
    /// </summary>
    public partial class AboutGolWin : Window
    {
        public AboutGolWin()
        {
            InitializeComponent();
        }

        private void aboutClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
