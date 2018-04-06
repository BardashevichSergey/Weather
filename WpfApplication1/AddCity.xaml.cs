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

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для AddCity.xaml
    /// </summary>
    public partial class AddCity : Window
    {
        public AddCity()
        {
            InitializeComponent();
        }

        private void button_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public string CityName
        {
            get { return textBox.Text; ; }
        }
    }
}
