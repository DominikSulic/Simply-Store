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
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void PrikaziProstorije()
        {
            dgProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
        }

        public void PrikaziSpremnike()
        {
            cmbProstorije.ItemsSource = PrikazProstorije.dohvatiNaziveProstorija();
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();


        }

        private void menuHome_Click(object sender, RoutedEventArgs e)
        {
            gridSpremnici.Visibility = Visibility.Collapsed;
            gridStavke.Visibility = Visibility.Collapsed;
            gridProstorije.Visibility = Visibility.Collapsed;
            gridKreirajProstoriju.Visibility = Visibility.Collapsed;
            gridIzmjeniProstoriju.Visibility = Visibility.Collapsed;
            naslovLabel.Content = "Home";
            gridPocetna.Visibility = Visibility.Visible;
        }

        private void menuProstorije_Click(object sender, RoutedEventArgs e)
        {
            gridSpremnici.Visibility = Visibility.Collapsed;
            gridStavke.Visibility = Visibility.Collapsed;
            gridPocetna.Visibility = Visibility.Collapsed;
            gridKreirajProstoriju.Visibility = Visibility.Collapsed;
            gridIzmjeniProstoriju.Visibility = Visibility.Collapsed;
            naslovLabel.Content = "Prostorije";
            PrikaziProstorije();            
            gridProstorije.Visibility = Visibility.Visible;


        }

        private void menuSpremnici_Click(object sender, RoutedEventArgs e)
        {
            
            gridStavke.Visibility = Visibility.Collapsed;
            gridPocetna.Visibility = Visibility.Collapsed;
            gridProstorije.Visibility = Visibility.Collapsed;
            gridKreirajProstoriju.Visibility = Visibility.Collapsed;
            gridIzmjeniProstoriju.Visibility = Visibility.Collapsed;
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
            gridSpremnici.Visibility = Visibility.Visible;
            
        }

        private void menuStavke_Click(object sender, RoutedEventArgs e)
        {
            
            gridPocetna.Visibility = Visibility.Collapsed;
            gridProstorije.Visibility = Visibility.Collapsed;
            gridSpremnici.Visibility = Visibility.Collapsed;
            gridKreirajProstoriju.Visibility = Visibility.Collapsed;
            gridIzmjeniProstoriju.Visibility = Visibility.Collapsed;
            naslovLabel.Content = "Stavke";
            gridStavke.Visibility = Visibility.Visible;

        }

        private void cmbProstorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            string nazivProstorije = cmbProstorije.SelectedItem.ToString();
            //MessageBoxResult result = MessageBox.Show(nazivProstorije); //Ultimate debugging tool
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike(nazivProstorije);
        }

        private void btnkreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            gridPocetna.Visibility = Visibility.Collapsed;
            gridProstorije.Visibility = Visibility.Collapsed;
            gridSpremnici.Visibility = Visibility.Collapsed;
            gridStavke.Visibility = Visibility.Collapsed;
            gridIzmjeniProstoriju.Visibility = Visibility.Collapsed;
            naslovLabel.Content = "Kreiraj prostoriju";
            gridKreirajProstoriju.Visibility = Visibility.Visible;
            
        }

        private void btnKreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije prikaz = new PrikazProstorije();
            prikaz.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text);
        }

        private void btnizmjeniProstoriju_Click(object sender, RoutedEventArgs e)
        {
            gridPocetna.Visibility = Visibility.Collapsed;
            gridProstorije.Visibility = Visibility.Collapsed;
            gridSpremnici.Visibility = Visibility.Collapsed;
            gridStavke.Visibility = Visibility.Collapsed;
            gridKreirajProstoriju.Visibility = Visibility.Collapsed;
            naslovLabel.Content = "Izmjeni prostoriju";
            gridIzmjeniProstoriju.Visibility = Visibility.Visible;

            PrikazProstorije pp = new PrikazProstorije();
            prostorija p = new prostorija();

            if (dgProstorije.SelectedItem != null)
            {
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
    }
}
