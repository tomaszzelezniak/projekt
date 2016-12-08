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

namespace Statki
{
    public enum Poz_trud { Latwy, Trudny }
    public partial class Menu_Gl : UserControl
    {
        public event EventHandler graj;
        public string imie;
        public Poz_trud poz_trud = Poz_trud.Latwy;
        public Menu_Gl()
        {
            InitializeComponent();
        }
        private void przycisk_Start(object sender, RoutedEventArgs e)
        {
            imie = txtboxImie.Text;
            if (imie == "")
            {
                MessageBox.Show("Muszisz podać imie", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                graj(this, e);
            }
        }
        //poziom łatwy
        private void przycisk_Latwy(object sender, RoutedEventArgs e)
        {
            poz_trud = Poz_trud.Latwy;
        }
        //poziom trudny
        private void przycisk_Trudny(object sender, RoutedEventArgs e)
        {
            poz_trud = Poz_trud.Trudny;
        }
        private void przycisk_zaznaczony(object sender, RoutedEventArgs e)
        {

        }
    }
}
