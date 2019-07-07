using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, UIElement> gridovi = new Dictionary<string, UIElement>();
        int globalniKorisnikID = 0;

        public MainWindow()
        {
            InitializeComponent();

            List<int> beskorisno = dohvatiDatotekuPodatke();

            gridovi.Add("gridLogin",gridLogin);

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

            gridovi.Add("gridOpcije", gridOpcije);
            gridovi.Add("gridStavkePredIstekom", gridStavkePredIstekom);
            gridovi.Add("gridStatistika", gridStatistika);
        }

        void promjeniGrid(string nazivGrida)
        {
            foreach (KeyValuePair<string, UIElement> grid in gridovi)
            {
                if (grid.Key == nazivGrida) grid.Value.Visibility = Visibility.Visible;
                else grid.Value.Visibility = Visibility.Collapsed;
            }
        }

        private List<int> dohvatiDatotekuPodatke()
        {
            List<int> podaciDatoteke = new List<int>();
            var putanja = Directory.GetCurrentDirectory();
            putanja += "\\postavke.txt";
            int darkTheme = 1;
            int brojDana = 3;
            if (File.Exists(putanja))
            {
                string[] lines = File.ReadAllLines(putanja);
                foreach (string line in lines)
                {
                    string[] col = line.Split(new char[] { ',' });
                    brojDana = Convert.ToInt32(col[0]);
                    darkTheme = Convert.ToInt32(col[1]);
                }

                if (darkTheme == 1)postaviDark();
                else postaviLight();
            }
            else postaviDark();
            podaciDatoteke.Add(brojDana);
            podaciDatoteke.Add(darkTheme);
            return podaciDatoteke;
        }
        #region Prikazi
        public void PrikaziProstorije()
        {
            dgProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
        }

        public void PrikaziSpremnike()
        {
            cmbProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();

            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();


        }

        public void PrikaziStavke()
        {
            cmbSpremnici.ItemsSource = PrikazSpremnici.dohvatiNaziveSpremnika();
            dgStavke.ItemsSource = PrikazStavke.dohvatiStavke();
        }

        #endregion

        #region MenuItems

        private void btnPrijava_Click(object sender, RoutedEventArgs e)
        {
            string korisnickoIme = txbKorisnickoIme.Text;
            string sLozinka = txbLozinka.Password;

            using (var db = new SSDB())
            {
                int query = (from k in db.korisnik
                             where k.korisnicko_ime == korisnickoIme && k.lozinka == sLozinka
                             select k.id_korisnik).FirstOrDefault();
                if(query != null)
                {
                    globalniKorisnikID = query;
                }
            }

            if (globalniKorisnikID != 0)//ako login prođe
            {
                gridGlavni.Visibility = Visibility.Visible;//ovo mora bit tako,nemoj premjestat u listu gridova
                promjeniGrid("gridPocetna");
            }
            else
            {
                MessageBox.Show("Pogrešan Login");
            }
            
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

        private void menuIzlaz_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void menuStavkePredIstekom_Click(object sender, RoutedEventArgs e)
        {
            naslovLabel.Content = "Stavke pred istekom";
            promjeniGrid("gridStavkePredIstekom");

            var putanja = Directory.GetCurrentDirectory();
            putanja += "\\postavke.txt";
            int brojDana = 3;
            if (File.Exists(putanja))
            {
                string[] lines = File.ReadAllLines(putanja);
                foreach (string line in lines)
                {
                    string[] col = line.Split(new char[] { ',' });
                    brojDana = Convert.ToInt32(col[0]);
                }
                dgStavkePredIstekom.ItemsSource = PrikazStavke.dohvatiStavkePredIstekom(brojDana);
            }
            else
            {
                dgStavkePredIstekom.ItemsSource = PrikazStavke.dohvatiStavkePredIstekom(brojDana);
            }

            dgStavkeIstekle.ItemsSource = PrikazStavke.stavkeIstecenogRoka();
        }

        private void menuOpcije_Click(object sender, RoutedEventArgs e)
        {
            naslovLabel.Content = "Opcije";
            promjeniGrid("gridOpcije");
            List<int> podaciDatoteke = dohvatiDatotekuPodatke();
            if (podaciDatoteke[1] == 1)
            {
                chkDarkTheme.IsChecked = true;
            }
            else
            {
                chkDarkTheme.IsChecked = false;
            }
            BrojDana.Text = Convert.ToString(podaciDatoteke[0]);

        }

        private void menuStatistika_Click(object sender, RoutedEventArgs e)
        {
            naslovLabel.Content = "Dnevnik";
            promjeniGrid("gridStatistika");
            dgStatistika.ItemsSource = PrikazStatistika.dohvatiStatistike();
        }
        #endregion


        private void cmbProstorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            PrikazProstorije selektiranaProstorija = (PrikazProstorije)cmbProstorije.SelectedItem;

            if (selektiranaProstorija == null)
            {
                dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
            }
            else
            {
                dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike(selektiranaProstorija);
            }
        }

        private void cmbSpremnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpremnici.SelectedItem != null) {
                string nazivSpremnika = cmbSpremnici.SelectedItem.ToString();
                dgStavke.ItemsSource = PrikazStavke.dohvatiStavke(nazivSpremnika);
            }
            
        }

        #region Prostorije
        private void BtnkreirajProstoriju_Click_1(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajProstoriju");
            naslovLabel.Content = "Kreiraj prostoriju";
        }

        private void btnKreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            if (txtNazivProstorije.Text != "")
            {
                int broj;
                if (txtBrojProstorija.Text == "")
                {
                    broj = 1;
                    PrikazProstorije.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text, broj);
                    txtNazivProstorije.Clear();
                    txtOpisProstorije.Clear();
                    txtNapomeneProstorije.Clear();
                    naslovLabel.Content = "Prostorije";
                    PrikaziProstorije();
                    promjeniGrid("gridProstorije");
                }
                else
                {
                    if (int.TryParse(txtBrojProstorija.Text, out broj))
                    {
                        if (broj > 0)
                        {
                            PrikazProstorije.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text, broj);
                            txtNazivProstorije.Clear();
                            txtOpisProstorije.Clear();
                            txtNapomeneProstorije.Clear();
                            txtBrojProstorija.Clear();
                            naslovLabel.Content = "Prostorije";
                            PrikaziProstorije();
                            promjeniGrid("gridProstorije");
                        }
                        else
                        {
                            MessageBox.Show("Unijeli ste nevažeću brojevnu vrijednost!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Niste unijeli brojčanu vrijednost!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Niste unijeli ime prostorije!");
            }

        }

        private void btnKreirajProstorijukOdustani_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridProstorije");
            naslovLabel.Content = "Prostorije";
            PrikaziProstorije();
        }

        private void BtnizmjeniProstoriju_Click_1(object sender, RoutedEventArgs e)
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

        private void btnIzmjeniProstorijukOdustani_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridProstorije");
            naslovLabel.Content = "Prostorije";
            PrikaziProstorije();
        }

        private void BtnObrisiProstoriju_Click_1(object sender, RoutedEventArgs e)
        {

            PrikazProstorije prikaz = new PrikazProstorije();
            if (dgProstorije.SelectedItems != null)
            {
                foreach (PrikazProstorije p in dgProstorije.SelectedItems)
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
        }

        private void btnPrikaziSveSpremnike_Click(object sender, RoutedEventArgs e)
        {
            cmbProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
            cmbProstorije.SelectedIndex = -1;
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
        }

        private void btnKreirajSpremnikSpremi_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije selektiranaProstorija = new PrikazProstorije();
            selektiranaProstorija = (PrikazProstorije)cmbProstorijeKreiranjeSpremnika.SelectedItem;
            string zapremninaS = txbNoviSpremnikZapremnina.Text;
            double zapremnina;
            int brojUnosa;
            if (txbNoviSpremnikNaziv.Text != "")
            {
                if (zapremninaS != "")
                {
                    if (double.TryParse(zapremninaS, out zapremnina))
                    {
                        if (selektiranaProstorija != null)
                        {
                            if (txbNoviSpremnikKvarljivost.Text.ToLower()=="da" || txbNoviSpremnikKvarljivost.Text.ToLower() == "ne")
                            {
                                if (txbBrojSpremnikaUnos.Text != "")
                                {
                                    if (int.TryParse(txbBrojSpremnikaUnos.Text, out brojUnosa))
                                    {
                                        if (brojUnosa > 0)
                                        {
                                            PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije,txbNoviSpremnikKvarljivost.Text.ToLower(), brojUnosa);
                                            txbNoviSpremnikNaziv.Clear();
                                            txbNoviSpremnikOpis.Clear();
                                            txbNoviSpremnikZapremnina.Clear();
                                            cmbProstorijeKreiranjeSpremnika.SelectedItem = null;
                                            naslovLabel.Content = "Spremnici";
                                            PrikaziSpremnike();
                                            promjeniGrid("gridSpremnici");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Unesite pozitivnu vrijednost broja unosa spremnika!");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Pogrešna vrijednost u polju broj spremnika!");
                                    }
                                }
                                else
                                {
                                    PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije,txbNoviSpremnikKvarljivost.Text.ToLower());
                                    txbNoviSpremnikNaziv.Clear();
                                    txbNoviSpremnikOpis.Clear();
                                    txbNoviSpremnikZapremnina.Clear();
                                    cmbProstorijeKreiranjeSpremnika.SelectedItem = null;
                                    naslovLabel.Content = "Spremnici";
                                    PrikaziSpremnike();
                                    promjeniGrid("gridSpremnici");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Niste unjeli da/ne");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Morate odabrati prostoriju");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Neispravna vrijednost u zapremnini");
                    }
                }
                else
                {
                    MessageBox.Show("Unesite zapremninu");
                }
            }
            else
            {
                MessageBox.Show("Morate unjeti naziv");
            }
        }

        private void btnObrisiSpremnik_Click(object sender, RoutedEventArgs e)
        {
            if (dgSpremnici.SelectedItems != null)
            {
                foreach (PrikazSpremnici p in dgSpremnici.SelectedItems)
                {
                    PrikazSpremnici.obrisiSpremnik(p.idSpremnika, globalniKorisnikID);
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
                txbStaraProstorijaID.Text = s.prostorija_id.ToString();
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
            int IdProstorije;
            if (cmbProstorijeIzmjenaSpremnika.SelectedItem != null)
            {
                odabranaProstorija = (PrikazProstorije)cmbProstorijeIzmjenaSpremnika.SelectedItem;
                IdProstorije = odabranaProstorija.idProstorije;   
            }
            else
            {
                IdProstorije = int.Parse(txbStaraProstorijaID.Text);
            }            
            PrikazSpremnici.izmjeniSpremnik(Convert.ToInt32(txbSpremnikID.Text), txbSpremnikNoviNaziv.Text, Convert.ToDouble(txbSpremnikNovaZapremnina.Text), txbSpremnikNoviOpis.Text, IdProstorije);
            txbSpremnikNoviNaziv.Clear();
            txbSpremnikNoviOpis.Clear();
            txbSpremnikNovaZapremnina.Clear();
            txbSpremnikID.Clear();
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
            promjeniGrid("gridSpremnici");
        }

        private void btnIzmjeniSpremnikOdustani_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridSpremnici");
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
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

        private void dgSpremnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazSpremnici spremnik = (PrikazSpremnici)dgSpremnici.SelectedItem;
            if (spremnik != null)
            {

                double[] popunjenost = PrikazSpremnici.dohvatiPopunjenost(spremnik.idSpremnika);
                double postotakZauz = popunjenost[1] / popunjenost[0];
                if (popunjenost[0] != 0) {
                gridPopunjenost.Visibility = Visibility.Visible;
                postotakZauzetosti.Width = new GridLength(postotakZauz, GridUnitType.Star);
                postotakSlobodnog.Width = new GridLength(1 - postotakZauz, GridUnitType.Star);
                postotakZauzetostiText.Content = 100 * Math.Round(postotakZauz, 4) + "%(" + popunjenost[1] + "/" + popunjenost[0] + ")";
                }
            }
            else
            {
                gridPopunjenost.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Stavke
        private void btnPrikaziSveStavke_Click(object sender, RoutedEventArgs e)
        {
            PrikaziStavke();
            cmbSpremnici.SelectedIndex = -1;
        }

        private void DgStavke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazStavke stavka = (PrikazStavke)dgStavke.SelectedItem;
            if (stavka != null)
            {
                lbOznakeStavke.ItemsSource = PrikazOznakaStavka.dohvatiOznakeStavke(stavka.idStavke);
            }

        }

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

            bool unosiKvarljivi = false;
            List<PrikazOznaka> selektiraneOznake = new List<PrikazOznaka>();
            foreach (var item in lbOznakeKreirajStavku.SelectedItems)
            {
                PrikazOznaka oznake = (PrikazOznaka)item;
                selektiraneOznake.Add(oznake);
            }

            foreach(var item in selektiraneOznake)
            {
                if (item.kvarljivost == "da")
                {
                    unosiKvarljivi = true;
                }
            }

            if (txbStavkaNoviNaziv.Text!="")
            {
                if (cmbSpremniciKreirajStavku.SelectedItem != null)
                {
                    if (txbZauzimaKreirajStavku.Text!="")
                    {
                        double zauzima = Convert.ToDouble(txbZauzimaKreirajStavku.Text);
                        double[] zapremninaSpremnika = PrikazSpremnici.dohvatiPopunjenost(selektiranSpremnik.idSpremnika);
                        if (zapremninaSpremnika[1] + zauzima <= zapremninaSpremnika[0])
                        {
                            if (selektiraneOznake.Count() != 0)
                            {
                                if ((unosiKvarljivi==true  && selektiranSpremnik.kvarljivost=="da") || (unosiKvarljivi == false && selektiranSpremnik.kvarljivost == "da") || (unosiKvarljivi == false && selektiranSpremnik.kvarljivost == "ne")) {
                                    if (dpStavkaIstekRoka.SelectedDate != null)
                                    {
                                        PrikazStavke.kreirajStavku(txbStavkaNoviNaziv.Text, selektiranSpremnik.idSpremnika, selektiraneOznake, dpStavkaIstekRoka.SelectedDate.Value.Date, zauzima, globalniKorisnikID);
                                        txbStavkaNoviNaziv.Clear();
                                        txbZauzimaKreirajStavku.Clear();
                                        dpStavkaIstekRoka.SelectedDate = null;
                                        cmbSpremniciKreirajStavku.SelectedItem = null;
                                        lbOznakeKreirajStavku.SelectedItem = null;
                                        naslovLabel.Content = "Stavke";
                                        PrikaziStavke();
                                        promjeniGrid("gridStavke");
                                    }
                                    else
                                    {
                                        DateTime? odabranDatum = null;
                                        PrikazStavke.kreirajStavku(txbStavkaNoviNaziv.Text, selektiranSpremnik.idSpremnika, selektiraneOznake, odabranDatum, zauzima, globalniKorisnikID);
                                        txbStavkaNoviNaziv.Clear();
                                        txbZauzimaKreirajStavku.Clear();
                                        dpStavkaIstekRoka.SelectedDate = null;
                                        cmbSpremniciKreirajStavku.SelectedItem = null;
                                        lbOznakeKreirajStavku.SelectedItem = null;
                                        naslovLabel.Content = "Stavke";
                                        PrikaziStavke();
                                        promjeniGrid("gridStavke");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Ne mozete unjeti kvarljiv proizvod u spremnik koji ne prihvača kvarljive proizvode");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Odaberite barem jednu oznaku");
                            }
                        }
                        else
                        {
                            MessageBox.Show("U odabrani spremnik ne stane zadana kolicina");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unesite kolicinu");
                    }
                }
                else
                {
                    MessageBox.Show("Odaberite spremnik");
                }
            }
            else
            {
                MessageBox.Show("Unesite naziv stavke");
            }
        }

        private void BtnObrisiStavku_Click(object sender, RoutedEventArgs e)
        {

            if (dgStavke.SelectedItems != null)
            {
                foreach (PrikazStavke p in dgStavke.SelectedItems)
                {
                    PrikazStavke.obrisiStavku(p.idStavke, globalniKorisnikID);
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

                txbIzmjeniStavkuNoviNaziv.Text = s.naziv;
                txbIzmjeniStavkuNovoZauzece.Text = s.zauzeće.ToString();
                txbStavkaID.Text = s.id_stavka.ToString();
                txbIzmjeniStavkuSpremnikID.Text = s.spremnik_id.ToString();
                txbIzmjeniStavkuStavkaIdSkriven.Text = Convert.ToString(s.id_stavka);

                List<PrikazOznaka> sveOznake = new List<PrikazOznaka>();
                sveOznake.AddRange(PrikazOznaka.dohvatiOznake());
                lbIzmjeniStavkuOznake.ItemsSource = sveOznake;

                List<PrikazOznakaStavka> oznakeZaOdabranuStavku = new List<PrikazOznakaStavka>();
                oznakeZaOdabranuStavku.AddRange(PrikazOznakaStavka.dohvatiOznakeStavke(s.id_stavka));
                lbIzmjeniStavkuNjeneOznake.ItemsSource = oznakeZaOdabranuStavku;

                if (s.datum_roka.HasValue)
                {
                    var inMyString = s.datum_roka.Value.ToShortDateString();
                    dpIzmjeniStavkuNoviIstekRoka.SelectedDate = DateTime.Parse(inMyString);
                }

                cmbProstorijeIzmjenaStavke.ItemsSource = PrikazProstorije.dohvatiProstorije();
            }
            else
            {
                MessageBox.Show("Niste odabrali stavku za izmjenu!");
            }

        }

        private void lbIzmjeniStavkuNjeneOznake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazOznakaStavka odabranaOznaka = new PrikazOznakaStavka();
            odabranaOznaka = (PrikazOznakaStavka)lbIzmjeniStavkuNjeneOznake.SelectedItem;
            if (odabranaOznaka != null)
            {
                int idStavke = Convert.ToInt32(txbIzmjeniStavkuStavkaIdSkriven.Text);
                PrikazOznakaStavka.obrisiStavkuOznaku(odabranaOznaka.idStavka, odabranaOznaka.idOznaka);
                osvjeziStavkineOznake();

            }

        }

        private void lbIzmjeniStavkuOznake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazOznaka odabranaOznaka = new PrikazOznaka();
            odabranaOznaka = (PrikazOznaka)lbIzmjeniStavkuOznake.SelectedItem;

            if (odabranaOznaka != null)
            {
                int idStavke = Convert.ToInt32(txbIzmjeniStavkuStavkaIdSkriven.Text);
                if (PrikazOznakaStavka.dodajStavkuOznaku(idStavke, odabranaOznaka.id_oznaka) == 0)
                {
                    MessageBox.Show("Unjeli ste oznaku koja vec vrijedi za tu stavku");
                }

                osvjeziStavkineOznake();
            }
        }

        private void osvjeziStavkineOznake()
        {
            int idStavke = Convert.ToInt32(txbIzmjeniStavkuStavkaIdSkriven.Text);
            List<PrikazOznakaStavka> oznakeZaOdabranuStavku = new List<PrikazOznakaStavka>();
            oznakeZaOdabranuStavku.AddRange(PrikazOznakaStavka.dohvatiOznakeStavke(idStavke));
            lbIzmjeniStavkuNjeneOznake.ItemsSource = oznakeZaOdabranuStavku;

            List<PrikazOznaka> sveOznake = new List<PrikazOznaka>();
            sveOznake.AddRange(PrikazOznaka.dohvatiOznake());
            lbIzmjeniStavkuOznake.ItemsSource = sveOznake;
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

        private void StatistikaSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            string tekst = StatistikaSearch.Text;
            dgStatistika.ItemsSource = PrikazStatistika.dohvatiStatistike(tekst);
        }

        private void btnIzmjeniStavkuOdustani_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridStavke");
            naslovLabel.Content = "Stavke";
            PrikaziStavke();
        }

        private void btnIzmjeniStavkuSpremi_Click(object sender, RoutedEventArgs e)
        {

            PrikazSpremnici odabranSpremnik = new PrikazSpremnici();
            odabranSpremnik = (PrikazSpremnici)cmbSpremniciOdabraneProstorije.SelectedItem;

            List<PrikazOznaka> selektiraneOznake = new List<PrikazOznaka>();

            double zauzima = Convert.ToDouble(txbIzmjeniStavkuNovoZauzece.Text);
            foreach (var item in lbIzmjeniStavkuNjeneOznake.SelectedItems)
            {
                PrikazOznaka oznake = (PrikazOznaka)item;
                selektiraneOznake.Add(oznake);
            }

            int idSpremnika;
            if (odabranSpremnik == null)
            {
                idSpremnika = int.Parse(txbIzmjeniStavkuSpremnikID.Text);
            }
            else
            {
                idSpremnika = odabranSpremnik.idSpremnika;
            }
            if (dpStavkaIstekRoka.SelectedDate != null)
            {
                PrikazStavke.izmjeniStavku(Convert.ToInt32(txbStavkaID.Text), txbIzmjeniStavkuNoviNaziv.Text, idSpremnika, dpIzmjeniStavkuNoviIstekRoka.SelectedDate.Value.Date, zauzima, globalniKorisnikID);
            }
            else
            {
                DateTime? datum = null;
                PrikazStavke.izmjeniStavku(Convert.ToInt32(txbStavkaID.Text), txbIzmjeniStavkuNoviNaziv.Text, idSpremnika, datum, zauzima, globalniKorisnikID);
            }

               

            naslovLabel.Content = "Stavke";
            PrikaziStavke();
            promjeniGrid("gridStavke");

        }


        #endregion

        #region Postavke

        private void postaviLight()
        {
            this.Resources["labelsColor"] = new SolidColorBrush(Color.FromRgb(7, 7, 7));
            this.Resources["rectBg"] = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            this.Resources["headerRectBg"] = new SolidColorBrush(Color.FromRgb(169, 170, 173));
            this.Resources["menuBg"] = new SolidColorBrush(Color.FromRgb(169, 170, 173));

            this.Resources["dataGridBG"] = new SolidColorBrush(Color.FromRgb(232, 232, 232));
            this.Resources["dataGridFG"] = new SolidColorBrush(Color.FromRgb(7, 7, 7));
            this.Resources["dataGridRowBG"] = new SolidColorBrush(Color.FromRgb(232, 232, 232));

            this.Resources["spBtnBg"] = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            this.Resources["spBtnFg"] = new SolidColorBrush(Color.FromRgb(7, 7, 7));
            this.Resources["spBtnBorderB"] = new SolidColorBrush(Color.FromRgb(247, 247, 247));
            this.Resources["spBtnHover"] = new SolidColorBrush(Color.FromRgb(193, 193, 193));
        }

        private void postaviDark()
        {
            this.Resources["labelsColor"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            this.Resources["rectBg"] = new SolidColorBrush(Color.FromRgb(58, 58, 58));
            this.Resources["headerRectBg"] = new SolidColorBrush(Color.FromRgb(33, 34, 39));
            this.Resources["menuBg"] = new SolidColorBrush(Color.FromRgb(33, 34, 39));

            this.Resources["dataGridBG"] = new SolidColorBrush(Color.FromRgb(58, 58, 58));
            this.Resources["dataGridFG"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            this.Resources["dataGridRowBG"] = new SolidColorBrush(Color.FromRgb(58, 58, 58));

            this.Resources["spBtnBg"] = new SolidColorBrush(Color.FromRgb(43, 43, 43));
            this.Resources["spBtnFg"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            this.Resources["spBtnBorderB"] = new SolidColorBrush(Color.FromRgb(43, 43, 43));
            this.Resources["spBtnHover"] = new SolidColorBrush(Color.FromRgb(130, 130, 130));
        }

        private void btnSpremiPostavke_Click(object sender, RoutedEventArgs e)
        {
            var putanja = Directory.GetCurrentDirectory();
            putanja += "\\postavke.txt";
            int brojDana;
            int dark;

            if ((bool)chkDarkTheme.IsChecked)
            {
                dark = 1;
            }
            else
            {
                dark = 0;
            }

            if (int.TryParse(BrojDana.Text, out brojDana))
            {
                using (StreamWriter file = new StreamWriter(putanja))
                {
                    file.WriteLine(BrojDana.Text+","+dark);
                    file.Close();
                }
            }

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        #endregion

        private void chkDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            if (chkDarkTheme.IsChecked == true)
            {
                postaviDark();
            }
            else
            {
                postaviLight();
            }

            var putanja = Directory.GetCurrentDirectory();
            putanja += "\\postavke.txt";
            int brojDana;
            int dark;

            if ((bool)chkDarkTheme.IsChecked)
            {
                dark = 1;
            }
            else
            {
                dark = 0;
            }

            if (int.TryParse(BrojDana.Text, out brojDana))
            {
                using (StreamWriter file = new StreamWriter(putanja))
                {
                    file.WriteLine(BrojDana.Text + "," + dark);
                    file.Close();
                }
            }
        }


    }
}


