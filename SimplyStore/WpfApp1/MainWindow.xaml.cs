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
            gridovi.Add("gridIzmjeniSpremnik", gridIzmjeniSpremnik);

            gridovi.Add("gridStavke", gridStavke);
            gridovi.Add("gridKreirajStavku", gridKreirajStavku);
            gridovi.Add("gridIzmjeniStavku", gridIzmjeniStavku);
            
        }

        void promjeniGrid(string nazivGrida)
        {
            foreach (KeyValuePair<string, UIElement> grid in gridovi)
            {
                if (grid.Key == nazivGrida) grid.Value.Visibility = Visibility.Visible;
                else grid.Value.Visibility = Visibility.Collapsed;
            }
        }


        #region Prikazi
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
        #endregion

        #region MenuItems
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

        private void menuIzlaz_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion


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

        #region Prostorije
        private void btnkreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajProstoriju");
            naslovLabel.Content = "Kreiraj prostoriju";
            
        }

        private void btnKreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text);
            txtNazivProstorije.Clear();
            txtOpisProstorije.Clear();
            txtNapomeneProstorije.Clear();
            naslovLabel.Content = "Prostorije";
            PrikaziProstorije();
            promjeniGrid("gridProstorije");
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
            PrikazProstorije.izmjeniProstoriju(int.Parse(txtIdProstorije.Text), txtNoviNazivProstorije.Text, txtNoviOpisProstorije.Text, txtNoveNapomeneProstorije.Text);
            
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

        private void ProstorijeSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            string tekst = ProstorijeSearch.Text;
            dgProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije(tekst);
        }
        #endregion

        #region Spremnici
        private void btnKreirajSpremnik_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajSpremnik");
            naslovLabel.Content = "Kreiraj spremnik";

            cmbProstorijeKreiranjeSpremnika.ItemsSource = PrikazProstorije.dohvatiProstorije();
            cmbTipSpreminka.ItemsSource = PrikazTipSpremnika.dohvatiTipSpremnika();


        }

        private void btnKreirajSpremnikSpremi_Click(object sender, RoutedEventArgs e)
        {
            PrikazTipSpremnika selektiranTipSpremnika = new PrikazTipSpremnika();
            selektiranTipSpremnika = (PrikazTipSpremnika)cmbTipSpreminka.SelectedItem;

            PrikazProstorije selektiranaProstorija = new PrikazProstorije();
            selektiranaProstorija = (PrikazProstorije)cmbProstorijeKreiranjeSpremnika.SelectedItem;
            double zapremnina = Convert.ToDouble(txbNoviSpremnikZapremnina.Text);

            PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije, selektiranTipSpremnika.idTipSpremnika);

            txbNoviSpremnikNaziv.Clear();
            txbNoviSpremnikOpis.Clear();
            txbNoviSpremnikZapremnina.Clear();
            cmbTipSpreminka.SelectedItem =null; 
            cmbProstorijeKreiranjeSpremnika.SelectedItem=null;
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
            promjeniGrid("gridSpremnici");
        }

        private void btnObrisiSpremnik_Click(object sender, RoutedEventArgs e)
        {
            if (dgSpremnici.SelectedItems != null)
            {
                foreach (PrikazSpremnici p in dgSpremnici.SelectedItems)
                {
                    PrikazSpremnici.obrisiSpremnik(p.idSpremnika);
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali spremnik!");
            }

            PrikaziSpremnike();

        }

        private void btnIzmjeniSpremnik_Click(object sender, RoutedEventArgs e)
        {

            if (dgSpremnici.SelectedItem != null)
            {
                naslovLabel.Content = "Izmjeni Spremnik";
                promjeniGrid("gridIzmjeniSpremnik");
                PrikazSpremnici odabraniSpremnik = new PrikazSpremnici();
                odabraniSpremnik = (PrikazSpremnici)dgSpremnici.SelectedItem;
                spremnik s = new spremnik();
                s = PrikazSpremnici.dohvatiSpremnik(odabraniSpremnik.idSpremnika);
                txbSpremnikNoviNaziv.Text = s.naziv_spremnika;
                txbSpremnikNoviOpis.Text = s.opis;
                txbSpremnikNovaZapremnina.Text = s.zapremnina.ToString();
                txbSpremnikID.Text = s.id_spremnik.ToString();
                cmbProstorijeIzmjenaSpremnika.ItemsSource = PrikazProstorije.dohvatiProstorije();
            }
            else
            {
                MessageBox.Show("Niste odabrali spremnik za izmjenu!");
            }
        }

        private void btnIzmjeniSpremnikSpremi_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije odabranaProstorija = new PrikazProstorije();
            odabranaProstorija = (PrikazProstorije)cmbProstorijeIzmjenaSpremnika.SelectedItem;
            PrikazSpremnici.izmjeniSpremnik(Convert.ToInt32(txbSpremnikID.Text), txbSpremnikNoviNaziv.Text,Convert.ToDouble(txbSpremnikNovaZapremnina.Text),txbSpremnikNoviOpis.Text, odabranaProstorija.idProstorije);
            txbSpremnikNoviNaziv.Clear();
            txbSpremnikNoviOpis.Clear();
            txbSpremnikNovaZapremnina.Clear();
            txbSpremnikID.Clear();
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
            promjeniGrid("gridSpremnici");
        }

        private void btnKreirajSpremnikOdustani_Click(object sender, RoutedEventArgs e)
        {

            promjeniGrid("gridSpremnici");
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();

        }

        private void SpremniciSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            string tekst = SpremniciSearch.Text;
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnikeN(tekst);
        }
        #endregion

        #region Stavke
        private void BtnKreirajStavku_Click(object sender, RoutedEventArgs e)
        {
            
            promjeniGrid("gridKreirajStavku");
            naslovLabel.Content = "Kreiraj stavku";

            cmbSpremniciKreirajStavku.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
            lbOznakeKreirajStavku.ItemsSource = PrikazOznaka.dohvatiOznake(); // tu se List<PrikazOznaka>
            
        }

        private void btnKreirajStavkuOdustani_Click(object sender, RoutedEventArgs e)
        {

            promjeniGrid("gridStavke");
            naslovLabel.Content = "Stavke";
            PrikaziStavke();

        }

        private void btnKreirajStavkuSpremi_Click(object sender, RoutedEventArgs e)
        {

            PrikazSpremnici selektiranSpremnik = new PrikazSpremnici();
            selektiranSpremnik = (PrikazSpremnici)cmbSpremniciKreirajStavku.SelectedItem;

            List<PrikazOznaka> selektiraneOznake = new List<PrikazOznaka>(); // stavara listu tipa Prikaz oznaka
            foreach (var item in lbOznakeKreirajStavku.SelectedItems)
            {
                PrikazOznaka oznake = (PrikazOznaka)item;
                selektiraneOznake.Add(oznake);
            } 

            int zauzima = Convert.ToInt32(txbZauzimaKreirajStavku.Text);
            
            PrikazStavke.kreirajStavku(txbStavkaNoviNaziv.Text, selektiranSpremnik.idSpremnika, selektiraneOznake, dpStavkaIstekRoka.DisplayDate, zauzima);
            

        }

        private void BtnObrisiStavku_Click(object sender, RoutedEventArgs e)
        {

            if (dgStavke.SelectedItems != null)
            {
                foreach (PrikazStavke p in dgStavke.SelectedItems)
                {
                    PrikazStavke.obrisiStavku(p.idStavke);
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali stavku!");
            }

            PrikaziStavke();

        }

        private void BtnIzmijeniStavku_Click(object sender, RoutedEventArgs e)
        {

            if (dgStavke.SelectedItem != null)
            {
                naslovLabel.Content = "Izmjeni stavku";
                promjeniGrid("gridIzmjeniStavku");

                PrikazStavke odabranaStavka = new PrikazStavke();
                odabranaStavka = (PrikazStavke)dgStavke.SelectedItem;
                stavka s = new stavka();
                s = PrikazStavke.dohvatiStavku(odabranaStavka.idStavke);
                txbStavkaNoviNaziv.Text = s.naziv;
                txbStavkaNovoZauzece.Text = s.zauzeće.ToString();
                if (s.datum_roka.HasValue)
                {
                    dpStavkaNoviIstekRoka.DisplayDate = s.datum_roka.Value;
                }

                cmbProstorijeIzmjenaStavke.ItemsSource = PrikazProstorije.dohvatiProstorije();
            }
            else
            {
                MessageBox.Show("Niste odabrali stavku za izmjenu!");
            }

        }

        private void CmbProstorijeIzmjenaStavke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazProstorije odabranaProstorija = new PrikazProstorije();
            odabranaProstorija = (PrikazProstorije)cmbProstorijeIzmjenaStavke.SelectedItem;
            string nazivProstorije = odabranaProstorija.nazivProstorije;

            List<PrikazSpremnici> popisSpremnika = new List<PrikazSpremnici>();
            List<PrikazSpremnici> popisSpremnikaPoProstoriji = new List<PrikazSpremnici>();
            popisSpremnika.AddRange(PrikazSpremnici.dohvatiSpremnike());
            foreach (var sprem in popisSpremnika)
            {
                if (sprem.nazivProstorije == nazivProstorije)
                {
                    popisSpremnikaPoProstoriji.Add(sprem);
                }
            }
            cmbSpremniciOdabraneProstorije.ItemsSource = popisSpremnikaPoProstoriji;
        }

        private void StavkeSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            string tekst = StavkeSearch.Text;
            dgStavke.ItemsSource = PrikazStavke.dohvatiStavkeN(tekst);
        }

        private void btnIzmjeniStavkuOdustani_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridStavke");
            naslovLabel.Content = "Stavke";
            PrikaziStavke();
        }

        private void btnIzmjeniStavkuSpremi_Click(object sender, RoutedEventArgs e)
        {


        }

        private void LbOznakeKreirajStavku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion

        
    }

}
