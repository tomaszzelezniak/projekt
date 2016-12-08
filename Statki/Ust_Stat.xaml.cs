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
    
    public partial class Ust_Stat : UserControl
    {
        public event EventHandler graj;
        enum Orientation { VERTICAL, HORIZONTAL };
        Orientation orientacja = Orientation.HORIZONTAL;
        SolidColorBrush niezaznaczona = new SolidColorBrush(Colors.Orange);
        SolidColorBrush zaznaczona = new SolidColorBrush(Colors.Green);
        String statek = "";
        int roz;
        int okr_rozmieszczone;
        Path Ost_Okr;
        Path[] statki;
        Polygon ost_srzalka;

        public Grid[] gracz_grid;

        SolidColorBrush[] kolor = new SolidColorBrush[] {(SolidColorBrush)(new BrushConverter().ConvertFrom("#88cc00")), (SolidColorBrush)(new BrushConverter().ConvertFrom("#33cc33")),
                                                                  (SolidColorBrush)(new BrushConverter().ConvertFrom("#00e64d")),(SolidColorBrush)(new BrushConverter().ConvertFrom("#00cc00")),
                                                                  (SolidColorBrush)(new BrushConverter().ConvertFrom("#00e600"))};

        public  Ust_Stat()
        {
            InitializeComponent();
            gracz_grid = new Grid[] { gridA1, gridA2, gridA3, gridA4, gridA5, gridA6, gridA7,gridA8,gridA9,gridA10,
                                gridB1, gridB2, gridB3, gridB4, gridB5, gridB6, gridB7,gridB8,gridB9,gridB10,
                                gridC1, gridC2, gridC3, gridC4, gridC5, gridC6, gridC7,gridC8,gridC9,gridC10,
                                gridD1, gridD2, gridD3, gridD4, gridD5, gridD6, gridD7,gridD8,gridD9,gridD10,
                                gridE1, gridE2, gridE3, gridE4, gridE5, gridE6, gridE7,gridE8,gridE9,gridE10,
                                gridF1, gridF2, gridF3, gridF4, gridF5, gridF6, gridF7,gridF8,gridF9,gridF10,
                                gridG1, gridG2, gridG3, gridG4, gridG5, gridG6, gridG7,gridG8,gridG9,gridG10,
                                gridH1, gridH2, gridH3, gridH4, gridH5, gridH6, gridH7,gridH8,gridH9,gridH10,
                                gridI1, gridI2, gridI3, gridI4, gridI5, gridI6, gridI7,gridI8,gridI9,gridI10,
                                gridJ1, gridJ2, gridJ3, gridJ4, gridJ5, gridJ6, gridJ7,gridJ8,gridJ9,gridJ10 };
            statki = new Path[] { niszczyciel, krazownik, podwodny, pancernik, lotniskowiec };
            reset();

        }
        
        //resetowanie okrętów
        private void reset()
        {
            if (ost_srzalka != null)
            {
                ost_srzalka.Stroke = niezaznaczona;
            }
            ost_srzalka = w_prawo;
            w_prawo.Stroke = zaznaczona;
            foreach(var element in gracz_grid)
            {
                element.Tag = "woda";
                element.Background = new SolidColorBrush(Colors.White);
            }
            foreach(var element in statki)
            {
                element.IsEnabled = true;
                element.Opacity = 100;
                if (element.Stroke != niezaznaczona)
                {
                    element.Stroke = niezaznaczona;
                }
            }
            okr_rozmieszczone = 0;
            Ost_Okr = null;
        }
        //rozmiwszczanie okrętów myszką
        private void statek_klik(object sender, MouseButtonEventArgs e)
        {
            Path okr_Path = (Path)sender;
            if(!okr_Path.IsEnabled)
            {
                return;
            }
            if (Ost_Okr != null)
            {
                Ost_Okr.Stroke = niezaznaczona;
            }
            Ost_Okr = okr_Path;
            statek = okr_Path.Name;
            okr_Path.Stroke = zaznaczona;

            switch(statek)
            {
                case "lotniskowiec":
                    roz = 5;
                    break;
                case "pancernik":
                    roz = 4;
                    break;
                case "podwodny":
                case "krazownik":
                    roz = 3;
                    break;
                case "niszczyciel":
                    roz = 2;
                    break;
            }
        }
        //wybieranie strzałką orientacji statku
        private void orientacja_statku (object sender, MouseButtonEventArgs e)
        {
            Polygon strzalka = (Polygon)sender;
            ost_srzalka.Stroke = niezaznaczona;
            ost_srzalka = strzalka;
            strzalka.Stroke = zaznaczona;
            if (strzalka.Name.Equals("w_prawo") || strzalka.Name.Equals("w_lewo"))
            {
                orientacja = Orientation.HORIZONTAL;
            }
            else
            {
                orientacja = Orientation.VERTICAL;
            }
        }
        //Sprawdzanie czy statek może zostać umieszczony
        //jeśli tak to go umieść 
        private void klik(object sender, MouseButtonEventArgs e)
        {
            Grid plac = (Grid)sender;
            int index=-1;
            int zmienna;
            int licznik=1;
            //sprawdzanie czy statek został wybrany
            if (Ost_Okr == null)
            {
                MessageBox.Show("Muszisz wybrać okręt", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //sprawdzanie czy statek został umieszczony
            if (!plac.Tag.Equals("woda"))
            {
                return;
            }
            //znajduje wybrany kwadrat i sprawdza czy nie wychodzi po za zakres
            index = Array.IndexOf(gracz_grid, plac);

            if (orientacja.Equals(Orientation.HORIZONTAL))
            {
                try
                {
                    licznik = 1;
                    for (int i = 0; i < roz; i++)
                    {
                        if (index + i <= 99)
                        {
                            //sprawdza czy obiekt jest w obrębie siatki
                            if (!gracz_grid[index + i].Tag.Equals("woda"))
                            {
                                throw new IndexOutOfRangeException("Złe rozmieszczenie statku, za mało miejsca");
                            }
                        }
                        //sprawdza czy jest tam miejsce
                        else
                        {
                            if (!gracz_grid[index - licznik].Tag.Equals("woda"))
                            {
                                throw new IndexOutOfRangeException("Złe rozmieszczenie statku");
                            }
                            licznik++;
                        }
                    }
                } catch (IndexOutOfRangeException chuj)
                {
                    MessageBox.Show(chuj.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                try
                {
                    licznik = 10;
                    for (int i = 0; i < roz * 10; i += 10)
                    {
                        if (index + i <= 99)
                        {
                            if (!gracz_grid[index + i].Tag.Equals("woda"))
                            {
                                throw new IndexOutOfRangeException("Złe rozmieszczenie statku");
                            }
                        }
                        else
                        {
                            if (!gracz_grid[index - licznik].Tag.Equals("woda"))
                            {
                                throw new IndexOutOfRangeException("Złe rozmieszczenie statku");
                            }
                            licznik += 10;
                        }
                    }
                    if ((index / 10) + (roz * 10) > 100)
                    {
                        throw new IndexOutOfRangeException("Złe rozmieszczenie statku, brak miejsca");
                    }
                }
                catch (IndexOutOfRangeException chuj)
                {
                    MessageBox.Show(chuj.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            //ustawianie statków w siatce
            if (orientacja.Equals(Orientation.HORIZONTAL))
                //w 2 rzędach
                if ((index + roz - 1) % 10 < roz - 1)
                {
                    licznik = 0;
                    zmienna = 1;

                    while ((index + licznik) % 10 > 1)
                    {
                        gracz_grid[index + licznik].Background = wybierz_kolor();
                        gracz_grid[index + licznik].Tag = statek;
                        licznik++;
                    }
                    for (int i = licznik; i < roz; i++)
                    {
                        gracz_grid[index - zmienna].Background = wybierz_kolor();
                        gracz_grid[index - zmienna].Tag = statek;
                        zmienna++;
                    }
                }
                //w jednym rzędzie
                else
                {
                    for (int i = 0; i < roz; i++)
                    {
                        gracz_grid[index + i].Background = wybierz_kolor();
                        gracz_grid[index + i].Tag = statek;
                    }
                }
        
            else
            {
                //w dwóch kolumnach
                if (index + (roz* 10) > 100)
                {
                    licznik = 0;
                    zmienna = 10;
                    while ((index / 10 + licznik ) % 100 < 10)
                    {
                        gracz_grid[index + licznik * 10].Background = wybierz_kolor();
                        gracz_grid[index + licznik * 10].Tag = statek;
                        licznik++;
                    }
                    for (int i = licznik; i<roz; i++)
                    {
                        gracz_grid[index - zmienna].Background = wybierz_kolor();
                        gracz_grid[index - zmienna].Tag = statek;
                        zmienna += 10;
                    }
                }
                //w jedniej kolumnie
                else
                {
                    licznik = 0;
                    for (int i = 0; i<roz* 10; i += 10)
                    {
                        gracz_grid[index + i].Background = wybierz_kolor();
                        gracz_grid[index + i].Tag = statek;
                    }
                }
            }
            Ost_Okr.IsEnabled = false;
            Ost_Okr.Opacity = 0.5;
            Ost_Okr.Stroke = niezaznaczona;
            Ost_Okr = null;
            okr_rozmieszczone++;

        }
            

            //przyciski
            //przycisk zatwierdź
        private void przycisk_zatwierdz(object sender, RoutedEventArgs e)
        {
            if (okr_rozmieszczone !=5)
            {
                return;
            }
            graj(this, e);
        }
        //przycisk reset
        private void przycisk_reset(object sender, RoutedEventArgs e)
        {
            reset();
        }
        //przycisk losowo
        private void przycisk_losowo(object sender, RoutedEventArgs e)
        {
            reset();
            Random losowo = new Random();
            int[] roz_stat = new int[] { 2, 3, 3, 4, 5 };
            string[] nazwa_statku = new string[] { "niszczyciel", "krazownik", "podwodny", "pancernik", "lotniskowiec" };
            int roz, index;
            string statek;
            Orientation orientacja;
            bool unavailableIndex = true;


            for (int i = 0; i < roz_stat.Length; i++)
            {
                //wybierz rozmiar i typ okręty
                roz = roz_stat[i];
                statek = nazwa_statku[i];
                unavailableIndex = true;

                if (losowo.Next(0, 2) == 0)
                    orientacja = Orientation.HORIZONTAL;
                else
                    orientacja = Orientation.VERTICAL;

                //wybierz okręt
                if (orientacja.Equals(Orientation.HORIZONTAL))
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
                            if (index + j > 99 || !gracz_grid[index + j].Tag.Equals("woda"))
                            {
                                index = losowo.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < roz; j++)
                    {
                        gracz_grid[index + j].Tag = statek;
                        gracz_grid[index + j].Background = kolor[i];
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
                            if (index + j > 99 || !gracz_grid[index + j].Tag.Equals("woda"))
                            {
                                index = losowo.Next(0, 100);
                                unavailableIndex = true;
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < roz * 10; j += 10)
                    {
                        gracz_grid[index + j].Tag = statek;
                        gracz_grid[index + j].Background = kolor[i];
                    }
                }

            }
            okr_rozmieszczone = 5;
            foreach (var element in statki)
            {
                element.IsEnabled = false;
                element.Opacity = .5;
                if (element.Stroke != niezaznaczona)
                {
                    element.Stroke = niezaznaczona;
                }

            }

        }
        private SolidColorBrush wybierz_kolor()
        {
            switch (statek)
            {
                case "niszczyciel":
                    return kolor[0];
                case "krazownik":
                    return kolor[1];
                case "podwodny":
                    return kolor[2];
                case "lotniskowiec":
                    return kolor[3];
                case "pancernik":
                    return kolor[4];
            }
            return kolor[0];
        }
    }
    }

    

