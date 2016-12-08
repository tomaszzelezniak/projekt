using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Gra_Komp : UserControl
    {
        public event EventHandler gra_ponowna;

        public Poz_trud poz_trud;
        public string imie;
        public Grid[] gracz_grid;
        public Grid[] comp_Grid;
        public List<int> lista_trafien;
        int licznik;
        public Random losowo = new Random();

        int gracz_lot = 5, comp_lot = 5;
        int gracz_panc = 4, comp_panc = 4;
        int gracz_podw = 3, comp_podw = 3;
        int gracz_kraz = 3, comp_kraz = 3;
        int gracz_niszcz = 2, comp_niszcz = 2;

        public Gra_Komp(Poz_trud poz_trud, Grid[] gracz_grid, string imie)
        {
            InitializeComponent();

            this.imie = imie;
            this.poz_trud = poz_trud;
            pocz_gry(gracz_grid);
            lista_trafien = new List<int>();


        }
        /// początek gry
        private void pocz_gry(Grid[] uzyt_grid)
        {
            //ustawia siatke komputera
            comp_Grid = new Grid[100];
            Comp_Grid.Children.CopyTo(comp_Grid, 0);
            for (int i = 0; i < 100; i++)
            {
                comp_Grid[i].Tag = "woda";
            }
            setupCompGrid();
            //ustawia siatke użytkownika
            gracz_grid = new Grid[100];
            Gracz_grid.Children.CopyTo(gracz_grid, 0);

            //ustawia statki
            for (int i = 0; i < 100; i++)
            {
                gracz_grid[i].Background = uzyt_grid[i].Background;
                gracz_grid[i].Tag = uzyt_grid[i].Tag;
            }

        }

     //Ustawianie Statków komputera
        private void setupCompGrid()
        {
            Random losowo = new Random();
            int[] roz_stat = new int[] { 2, 3, 3, 4, 5 };
            string[] statki = new string[] { "niszczyciel", "krazownik", "podwodny", "pancernik", "lotniskowiec" };
            int roz, index;
            string statek;
            Orientation orientacja;
            bool unavailableIndex = true;

            for (int i = 0; i < roz_stat.Length; i++)
            {
                //ustawia rozmiar i typ statku
                roz = roz_stat[i];
                statek = statki[i];
                unavailableIndex = true;

                if (losowo.Next(0, 2) == 0)
                    orientacja = Orientation.Horizontal;
                else
                    orientacja = Orientation.Vertical;

                //wybirea statek
                if (orientacja.Equals(Orientation.Horizontal))
                {
                    index = losowo.Next(0, 100);
                    while (unavailableIndex == true)
                    {
                        unavailableIndex = false;

                        while ((index + roz - 1) % 10 < roz - 1)
                        {
                            index = losowo.Next(0, 100);
                        }

                        for (int j = 0; j < roz; j++)
                        {
                            if (index + j > 99 || !comp_Grid[index + j].Tag.Equals("woda"))
                            {
                                index = losowo.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < roz; j++)
                    {
                        comp_Grid[index + j].Tag = statek;
                    }
                }
                else
                {
                    index = losowo.Next(0, 100);
                    while (unavailableIndex == true)
                    {
                        unavailableIndex = false;

                        while (index / 10 + roz * 10 > 100)
                        {
                            index = losowo.Next(0, 100);
                        }

                        for (int j = 0; j < roz * 10; j += 10)
                        {
                            if (index + j > 99 || !comp_Grid[index + j].Tag.Equals("woda"))
                            {
                                index = losowo.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < roz * 10; j += 10)
                    {
                        comp_Grid[index + j].Tag = statek;
                    }
                }

            }


        }

        /// Atak przez użytkownika
        private void klik(object sender, MouseButtonEventArgs e)
        {
            //wybór kwadratu w który strzela gracz 
            Grid plac = (Grid)sender;
            if (licznik % 2 != 0)
            {
                return;
            }
            switch (plac.Tag.ToString())
            {
                case "woda":
                    plac.Tag = "pudlo";
                    plac.Background = new SolidColorBrush(Colors.LightGreen);
                    licznik++;
                    tura_comp();
                    return;

                case "pudlo":
                case "trafienie":
 
                    return;
                case "niszczyciel":
                    comp_niszcz--;
                    break;
                case "krazownik":
                    comp_kraz--;
                    break;
                case "podwodny":
                    comp_podw--;
                    break;
                case "pancernik":
                    comp_panc--;
                    break;
                case "lotniskowiec":
                    comp_lot--;
                    break;
            }
            plac.Tag = "trafiony";
            plac.Background = new SolidColorBrush(Colors.Red);
            licznik++;
            zniszcz_stat();
            tura_comp();

        }

        private void tura_comp()
        {
            if (poz_trud == Poz_trud.Latwy)
            {
                poz_latwy();
            }
            else
            {
                poz_trudny();
            }
            licznik++;
            zniszcz_statki_comp();
        }
        private void zniszcz_stat()
        {
            if (comp_lot == 0)
            {
                comp_lot = -1;
                MessageBox.Show("Zniszczyłeś mój Lotniskowiec!");
            }
            if (comp_kraz == 0)
            {
                comp_kraz = -1;
                MessageBox.Show("Zniszczyłeś mój Krążownik");
            }
            if (comp_niszcz == 0)
            {
                comp_niszcz = -1;
                MessageBox.Show("Zniszczyłeś mój Niszczyciel");
            }
            if (comp_panc == 0)
            {
                comp_panc = -1;
                MessageBox.Show("Zniszczyłeś mój Pancernik");
            }
            if (comp_podw == 0)
            {
                comp_podw = -1;
                MessageBox.Show("Zniszczyłeś mój Okręt Podwodny");
            }

            if (comp_lot == -1 && comp_panc == -1 && comp_podw == -1 &&
                comp_kraz == -1 && comp_niszcz == -1)
            {
                MessageBox.Show("Gratulacje wygrałeś talon");
                wyl_Grid();

            }
        }



        private void zniszcz_statki_comp()
        {
            if (gracz_lot == 0)
            {
                gracz_lot = -1;
                MessageBox.Show("Twój Lotniskowiec został zniszczony");
            }
            if (gracz_kraz == 0)
            {
                gracz_kraz = -1;
                MessageBox.Show("Twój Krążownik został zniszczony");
            }
            if (gracz_niszcz == 0)
            {
                gracz_niszcz = -1;
                MessageBox.Show("Twój Niszczyciel został zniszczony");
            }
            if (gracz_panc == 0)
            {
                gracz_panc = -1;
                MessageBox.Show("Twój Pancernik został zniszczony");
            }
            if (gracz_podw == 0)
            {
                gracz_podw = -1;
                MessageBox.Show("Twój Okręt podwodny został zniszczony");
            }

            if (gracz_lot == -1 && gracz_panc == -1 && gracz_podw == -1 &&
                gracz_kraz == -1 && gracz_niszcz == -1)
            {
                MessageBox.Show("Przegrałeś");
                wyl_Grid();

            }
        }
        private void wyl_Grid()
        {
            foreach (var element in comp_Grid)
            {
                if (element.Tag.Equals("woda"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGray);
                }
                else if (element.Tag.Equals("lotniskowiec") || element.Tag.Equals("krazownik") ||
                  element.Tag.Equals("niszczyciel") || element.Tag.Equals("pancernik") || element.Tag.Equals("podwodny"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGreen);
                }
                element.IsEnabled = false;
            }
            foreach (var element in gracz_grid)
            {
                if (element.Tag.Equals("woda"))
                {
                    element.Background = new SolidColorBrush(Colors.LightGray);
                }
                element.IsEnabled = false;
            }


        }
       
        /// Ruchy komputera na łatwym poziomie(strzela na slepo jak 2 letnie dziecko)
        //poziom trudny(komputer stara sie zniszczyc statek po trafieniu)
        private void poz_trudny()
        {
            //jeśli nie ma pól do uderzenia
            if (lista_trafien.Count == 0)
            {
                poz_latwy();
            }
            
            else
                trudny();
        }
        

       
        //poziom latwy strzela na slepo jak murzyn w jaskini
  
        private void poz_latwy()
        {
            int pozycja;
            do
            {
                pozycja = losowo.Next(100);
            } while ((gracz_grid[pozycja].Tag.Equals("pudlo")) || (gracz_grid[pozycja].Tag.Equals("Trafienie")));


            if (poz_trud == Poz_trud.Latwy)
            {
                latwy(pozycja);
            }
            else
            {
                pozycja_strzalu(pozycja);
            }

        }

      
        //Poziom łatwy strzela losowo bez algortymu
       
        private void latwy(int pozycja)
        {
            if (!(gracz_grid[pozycja].Tag.Equals("woda")))
            {
                // zaznacza trafiony statek
                switch (gracz_grid[pozycja].Tag.ToString())
                {
                    case "niszczyciel":
                        gracz_niszcz--;
                        break;
                    case "krazownik":
                        gracz_kraz--;
                        break;
                    case "podwodny":
                        gracz_podw--;
                        break;
                    case "pancernik":
                        gracz_panc--;
                        break;
                    case "lotniskowiec":
                        gracz_lot--;
                        break;
                }
                // zaznacza trafione pole
                gracz_grid[pozycja].Tag = "trafiony";
                gracz_grid[pozycja].Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                gracz_grid[pozycja].Tag = "pudlo";
                gracz_grid[pozycja].Background = new SolidColorBrush(Colors.Black);
            }
        }


        private void pozycja_strzalu(int pozycja)
        {
            //jezeli pozycja zawiera element statku(bez wody, pudlo, trafienie)
            if (!(gracz_grid[pozycja].Tag.Equals("woda")))
            {
                // jezeli pole zawiera trafienie 
               if (lista_trafien != null && lista_trafien.Contains(pozycja))
                   lista_trafien.Remove(pozycja);

                // jezeli trafil statek zaznacza go
                switch (gracz_grid[pozycja].Tag.ToString())
                {
                    case "niszczyciel":
                        gracz_niszcz--;
                        break;
                    case "krazownik":
                        gracz_kraz--;
                        break;
                    case "podwodny":
                        gracz_podw--;
                        break;
                    case "pancernik":
                        gracz_panc--;
                        break;
                    case "lotniskowiec":
                        gracz_lot--;
                        break;
                }
                // zaznacz trafione pole
                gracz_grid[pozycja].Tag = "trafiony";
                gracz_grid[pozycja].Background = new SolidColorBrush(Colors.Red);

                // jezeli zniszczy statek wraca do poziomu latwego
                if (gracz_niszcz == 0 || gracz_kraz == 0 || gracz_podw == 0 || gracz_panc == 0 || gracz_lot == 0)
                {
                    lista_trafien.Clear();
                }
                // jezeli statek nie jest zniszczony dodaj pola do listy trafien
                else
                {
                    // jezeli komputer trafi statek dodaj do listy trafien
                    // jezeli pozycj jest po lewej stronie
                    if (pozycja % 10 == 0)
                        lista_trafien.Add(pozycja + 1);
                    // jezeli pozycja jest po prawej stronie
                    else if (pozycja % 10 == 9)
                        lista_trafien.Add(pozycja - 1);
                    // jezeli pozycja nie jest ani po lewej ani po prawej
                    else
                    {
                        lista_trafien.Add(pozycja + 1);
                        lista_trafien.Add(pozycja - 1);
                    }
                    // jezeli pozycja jest na gorze
                    if (pozycja < 10)
                        lista_trafien.Add(pozycja + 10);
                    // jezeli pozycja jest na dole
                    else if (pozycja > 89)
                        lista_trafien.Add(pozycja - 10);
                    // jezeli pozycja nie jest ani na gorze ani na dole
                    else
                    {
                        lista_trafien.Add(pozycja + 10);
                        lista_trafien.Add(pozycja - 10);
                    }

                    // sprawia ze komputer nie strzela w na oslep 
                    try
                    {
                        lista_trafien.Remove(pozycja - 11);
                    }
                    catch (Exception e) { }
                    try
                    {
                        lista_trafien.Remove(pozycja - 9);
                    }
                    catch (Exception e) { }
                    try
                    {
                        lista_trafien.Remove(pozycja + 9);
                    }
                    catch (Exception e) { }
                    try
                    {
                        lista_trafien.Remove(pozycja + 11);
                    }
                    catch (Exception e) { }
                }
            }
            else
            {
                gracz_grid[pozycja].Tag = "pudlo";
                gracz_grid[pozycja].Background = new SolidColorBrush(Colors.LightGray);
            }
        }



       
        private void trudny()
        {
            int pozycja;
            // przygotowanie strzelow do losowego trafiania
            do
            {
                pozycja = losowo.Next(lista_trafien.Count);
            } while (gracz_grid[lista_trafien[pozycja]].Tag.Equals("pudlo") || gracz_grid[lista_trafien[pozycja]].Tag.Equals("trafienie"));

            //szuka indeksu do strzalu
            pozycja_strzalu(lista_trafien[pozycja]);

        }
        private void znowu(object sender, RoutedEventArgs e)
        {
            gra_ponowna(this, e);
        }

    }
}
