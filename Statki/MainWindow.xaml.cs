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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Statki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid grid = new Grid();

        private Menu_Gl menu_Gl;
        private Ust_Stat ust_Stat;
        private Gra_Komp gra_Komp;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }
        //inicjalizacja gry
        private void InitializeGame()
        {
            Content = grid;
            this.MinHeight = 380;
            this.MinWidth = 350;
            this.Height = 380;
            this.Width = 350;
            //inicjalizacja menu gł
            menu_Gl = new Menu_Gl();
            grid.Children.Add(menu_Gl);

            menu_Gl.graj += new EventHandler(plansza);
        }
        //inicjalizacja planszy do ustawiania statków
        private void plansza(object sender, EventArgs e)
        {
            //zamykanie menu Gł
            grid.Children.Clear();
            //zmiana rozmiaru okna
            this.MinWidth = 460;
            this.MinHeight = 530;
            this.Width = 460;
            this.Height = 530;
            //inicjalizacja planszy do rozmieszczenia statków
            ust_Stat = new Ust_Stat();
            grid.Children.Add(ust_Stat);
            ust_Stat.graj += new EventHandler(gra);
        }
        private void gra (object sender, EventArgs e)
        {
            //zamykanie zormieszczenia statków
            grid.Children.Clear();
            //zmiana rozmiaru okna
            this.MinWidth = 955;
            this.MinHeight = 480;
            this.Width = 955;
            this.Height = 480;
            //inicjalizacja gry
            gra_Komp = new Gra_Komp(menu_Gl.poz_trud,ust_Stat.gracz_grid,menu_Gl.imie);
            grid.Children.Add(gra_Komp);
            gra_Komp.gra_ponowna += new EventHandler(gra_ponowna);
        }
        private void gra_ponowna(object sender, EventArgs e)
        {
            grid.Children.Clear();
            InitializeGame();
        }
    }

}
