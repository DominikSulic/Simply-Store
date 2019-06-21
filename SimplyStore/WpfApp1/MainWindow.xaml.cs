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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, UIElement> gridovi = new Dictionary<string, UIElement>();
        public MainWindow()
        {
            InitializeComponent();
            gridovi.Add("gridPocetna", gridPocetna);

            gridovi.Add("gridProstorije", gridProstorije);
            gridovi.Add("gridKreirajProstoriju", gridKreirajProstoriju);
            gridovi.Add("gridIzmjeniProstoriju", gridIzmjeniProstoriju);

            gridovi.Add("gridSpremnici", gridSpremnici);
            gridovi.Add("gridKreirajSpremnik", gridKreirajSpremnik);

            gridovi.Add("gridStavke", gridStavke);
            
        }

        void promjeniGrid(string nazivGrida)
        {
            foreach (KeyValuePair<string, UIElement> grid in gridovi)
            {
                if (grid.Key == nazivGrida) grid.Value.Visibility = Visibility.Visible;
                else grid.Value.Visibility = Visibility.Collapsed;
            }
        }

        public void PrikaziProstorije()
        {
            dgProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
        }

        public void PrikaziSpremnike()
        {
            List<string> naziviProstorija = PrikazProstorije.dohvatiNaziveProstorija();
            naziviProstorija.Insert(0, "--Sve--");
            cmbProstorije.ItemsSource = naziviProstorija;
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();


        }

        public void PrikaziStavke()
        {
            cmbSpremnici.ItemsSource = PrikazSpremnici.dohvatiNaziveSpremnika();
            dgStavke.ItemsSource = PrikazStavke.dohvatiStavke();
        }

        private void menuHome_Click(object sender, RoutedEventArgs e)
        {
            
            naslovLabel.Content = "Home";
            promjeniGrid("gridPocetna");
        }

        private void menuProstorije_Click(object sender, RoutedEventArgs e)
        {
            
            naslovLabel.Content = "Prostorije";
            PrikaziProstorije();
            promjeniGrid("gridProstorije");
        }

        private void menuSpremnici_Click(object sender, RoutedEventArgs e)
        {
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
            promjeniGrid("gridSpremnici");
            
        }

        private void menuStavke_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridStavke");
            naslovLabel.Content = "Stavke";
            PrikaziStavke();

        }

        private void cmbProstorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            string nazivProstorije = cmbProstorije.SelectedItem.ToString();
            if (nazivProstorije == "--Sve--")
            {
                dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
            }
            else
            {
                dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike(nazivProstorije);
            }
            //MessageBoxResult result = MessageBox.Show(nazivProstorije); //Ultimate debugging tool
        }

        private void cmbSpremnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string nazivSpremnika = cmbSpremnici.SelectedItem.ToString();
            dgStavke.ItemsSource = PrikazStavke.dohvatiStavke(nazivSpremnika);
        }

        private void btnkreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajProstoriju");
            naslovLabel.Content = "Kreiraj prostoriju";
            
        }

        private void btnKreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije prikaz = new PrikazProstorije();
            prikaz.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text);
        }

        private void btnizmjeniProstoriju_Click(object sender, RoutedEventArgs e)
        {
             
            

            PrikazProstorije pp = new PrikazProstorije();
            prostorija p = new prostorija();

            if (dgProstorije.SelectedItem != null)
            {
                naslovLabel.Content = "Izmjeni prostoriju";
                promjeniGrid("gridIzmjeniProstoriju");
                pp = (PrikazProstorije)dgProstorije.SelectedItem;
                p = pp.dohvatiProstoriju(pp.nazivProstorije);
            }
            else
            {
                MessageBox.Show("Niste odabrali prostoriju za izmjenu!");
            }

            txtNoveNapomeneProstorije.Text = p.posebne_napomene;
            txtNoviNazivProstorije.Text = p.naziv_prostorije;
            txtNoviOpisProstorije.Text = p.opis;
            txtIdProstorije.Text = p.id_prostorija.ToString();
        }

        private void btnIzmjeniProstoriju_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije pp = new PrikazProstorije();
            pp.izmjeniProstoriju(int.Parse(txtIdProstorije.Text), txtNoviNazivProstorije.Text, txtNoviOpisProstorije.Text, txtNoveNapomeneProstorije.Text);
            
        }

        private void btnObrisiProstoriju_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije prikaz = new PrikazProstorije();
            if(dgProstorije.SelectedItems != null)
            {
                foreach(PrikazProstorije p in dgProstorije.SelectedItems)
                {
                    prikaz.obrisiProstoriju(p.nazivProstorije);
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali niti jednu prostoriju!");
            }

            PrikaziProstorije();
        }

        private void btnKreirajSpremnik_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajSpremnik");
            naslovLabel.Content = "Kreiraj spremnik";
        }
    }
}
