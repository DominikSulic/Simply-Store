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

            gridovi.Add("gridLogin", gridLogin);

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

            gridovi.Add("gridStavkePredIstekom", gridStavkePredIstekom);
            gridovi.Add("gridStatistika", gridStatistika);

            gridovi.Add("gridOznake", gridOznake);
            gridovi.Add("gridKreirajOznaku", gridKreirajOznaku);

            gridovi.Add("gridOpcije", gridOpcije);

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

                if (darkTheme == 1) postaviDark();
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
            promjeniHeaderProstorije();
        }

        private void promjeniHeaderProstorije()
        {
            dgProstorije.Columns[0].Header = "Id prostorije";
            dgProstorije.Columns[1].Header = "Naziv prostorije";
            dgProstorije.Columns[2].Header = "Datum kreiranja";
            dgProstorije.Columns[3].Header = "Opis";
            dgProstorije.Columns[4].Header = "Posebne napomene";
        }


        public void PrikaziSpremnike()
        {
            cmbProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
            promjeniHeaderSpremnici();
            dgSpremnici.Columns[4].Visibility = Visibility.Hidden;
        }

        private void promjeniHeaderSpremnici()
        {
            dgSpremnici.Columns[0].Header = "ID spremnika";
            dgSpremnici.Columns[1].Header = "Naziv spremnika";
            dgSpremnici.Columns[2].Header = "Datum kreiranja";
            dgSpremnici.Columns[3].Header = "Zapremnina";
            // 4. je hidden - zauzece
            dgSpremnici.Columns[5].Header = "Opis";
            dgSpremnici.Columns[6].Header = "Pripadna prostorija";
        }

        public void PrikaziStavke()
        {
            cmbSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
            dgStavke.ItemsSource = PrikazStavke.dohvatiStavke();
            promjeniHeaderStavke();
        }

        private void promjeniHeaderStavke()
        {
            dgStavke.Columns[0].Header = "Id stavke";
            dgStavke.Columns[1].Header = "Naziv stavke";
            dgStavke.Columns[2].Header = "Datum kreiranja";
            dgStavke.Columns[3].Header = "Rok trajanja";
            dgStavke.Columns[4].Header = "Količina";
            dgStavke.Columns[5].Header = "Pripadni spremnik";
            dgStavke.Columns[6].Header = "Pripadna prostorija";
        }

        public void PrikaziOznake()
        {
            dgOznake.ItemsSource = PrikazOznaka.dohvatiSveOznake();
            promjeniHeaderOznake();
        }

        private void promjeniHeaderOznake()
        {
            dgOznake.Columns[0].Header = "Id oznake";
            dgOznake.Columns[1].Header = "Naziv oznake";
            dgOznake.Columns[2].Header = "Predstavlja kvarljivu";
            dgOznake.Columns[3].Header = "Aktivna";
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
                if (query != null)
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
            dgStavkeIstekle.Columns[0].Header = "Id stavke";
            dgStavkeIstekle.Columns[1].Header = "Naziv stavke";
            dgStavkeIstekle.Columns[2].Header = "Datum kreiranja";
            dgStavkeIstekle.Columns[3].Header = "Rok trajanja";
            dgStavkeIstekle.Columns[4].Header = "Količina";
            dgStavkeIstekle.Columns[5].Header = "Pripadni spremnik";
            dgStavkeIstekle.Columns[6].Header = "Pripadna prostorija";

            dgStavkePredIstekom.Columns[0].Header = "Id stavke";
            dgStavkePredIstekom.Columns[1].Header = "Naziv stavke";
            dgStavkePredIstekom.Columns[2].Header = "Datum kreiranja";
            dgStavkePredIstekom.Columns[3].Header = "Rok trajanja";
            dgStavkePredIstekom.Columns[4].Header = "Količina";
            dgStavkePredIstekom.Columns[5].Header = "Pripadni spremnik";
            dgStavkePredIstekom.Columns[6].Header = "Pripadna prostorija";
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
            dgStatistika.Columns[0].Header = "Radnja";
            dgStatistika.Columns[1].Header = "Datum";
            dgStatistika.Columns[2].Header = "Kolicina";
            dgStatistika.Columns[3].Header = "Naziv stavke";
            dgStatistika.Columns[4].Header = "Naziv korisnika";
            dgStatistika.Columns[5].Header = "ID stavke";
            dgStatistika.Columns[6].Header = "Oznake";
        }

        private void menuOznake_Click(object sender, RoutedEventArgs e)
        {
            naslovLabel.Content = "Oznake";
            PrikaziOznake();
            promjeniGrid("gridOznake");


        }
        #endregion


        private void cmbProstorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            PrikazProstorije selektiranaProstorija = (PrikazProstorije)cmbProstorije.SelectedItem;

            if (selektiranaProstorija == null)
            {
                dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
                dgSpremnici.Columns[4].Visibility = Visibility.Hidden;
            }
            else
            {
                dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike(selektiranaProstorija);
                dgSpremnici.Columns[4].Visibility = Visibility.Hidden;
            }
            promjeniHeaderSpremnici();
        }

        private void cmbSpremnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpremnici.SelectedItem != null)
            {
                string nazivSpremnika = cmbSpremnici.SelectedItem.ToString();
                dgStavke.ItemsSource = PrikazStavke.dohvatiStavke(nazivSpremnika);
            }
            promjeniHeaderStavke();

        }


        #region Prostorije
        private void BtnkreirajProstoriju_Click_1(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajProstoriju");
            naslovLabel.Content = "Kreiraj prostoriju";
        }

        private void btnKreirajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            if (txtNazivProstorije.Text != "" && ((from c in txtNazivProstorije.Text where c != ' ' select c).Count() != 0))
            {
                int broj;
                if (txtBrojProstorija.Text == "")
                {
                    broj = 1;
                    PrikazProstorije.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text, broj, globalniKorisnikID);
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
                            PrikazProstorije.kreirajProstoriju(txtNazivProstorije.Text, txtOpisProstorije.Text, txtNapomeneProstorije.Text, broj, globalniKorisnikID);
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

                txtNoveNapomeneProstorije.Text = p.posebne_napomene;
                txtNoviNazivProstorije.Text = p.naziv_prostorije;
                txtNoviOpisProstorije.Text = p.opis;
                txtIdProstorije.Text = p.id_prostorija.ToString();
            }
            else
            {
                MessageBox.Show("Niste odabrali prostoriju za izmjenu!");
            }
        }

        private void btnIzmjeniProstoriju_Click(object sender, RoutedEventArgs e)
        {
            if (txtNoviNazivProstorije.Text != "" && ((from c in txtNoviNazivProstorije.Text where c != ' ' select c).Count() != 0))
            {
                PrikazProstorije.izmjeniProstoriju(int.Parse(txtIdProstorije.Text), txtNoviNazivProstorije.Text, txtNoviOpisProstorije.Text, txtNoveNapomeneProstorije.Text);
                promjeniGrid("gridProstorije");
                naslovLabel.Content = "Prostorije";
                PrikaziProstorije();
            }
            else
            {
                MessageBox.Show("Prostorija mora imati naziv!");
            }

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
                    prikaz.obrisiProstoriju(p.idProstorije, globalniKorisnikID);
                }
            }
            else
            {
                MessageBox.Show("Niste odabrali niti jednu prostoriju!");
            }

            PrikaziProstorije();

        }

        private void ProstorijeSearchEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string tekst = ProstorijeSearch.Text;
                if (tekst == "")
                {
                    dgProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
                    promjeniHeaderProstorije();
                }
                else
                {
                    dgProstorije.ItemsSource = PrikazProstorije.dohvatiProstorijeEnter(tekst);
                    promjeniHeaderProstorije();
                }
            }
        }

        private void BtnkreirajPDFProstorije_Click(object sender, RoutedEventArgs e)
        {
            PDFConverter.ExportPDFProstorije();
        }
        #endregion

        #region Spremnici
        private void btnKreirajSpremnik_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajSpremnik");
            naslovLabel.Content = "Kreiraj spremnik";

            cmbProstorijeKreiranjeSpremnika.ItemsSource = PrikazProstorije.dohvatiProstorije();
            lbxOznakeNovogSpremnika.ItemsSource = PrikazOznaka.dohvatiOznake();
        }

        private void btnPrikaziSveSpremnike_Click(object sender, RoutedEventArgs e)
        {
            
            PrikaziSpremnike();
            cmbProstorije.SelectedIndex = -1;
        }

        private void btnKreirajSpremnikSpremi_Click(object sender, RoutedEventArgs e)
        {
            PrikazProstorije selektiranaProstorija = new PrikazProstorije();
            selektiranaProstorija = (PrikazProstorije)cmbProstorijeKreiranjeSpremnika.SelectedItem;
            string zapremninaS = txbNoviSpremnikZapremnina.Text;
            double zapremnina;
            int brojUnosa;
            if (txbNoviSpremnikNaziv.Text != "" && ((from c in txbNoviSpremnikNaziv.Text where c != ' ' select c).Count() != 0))
            {
                if (zapremninaS != "")
                {
                    if (double.TryParse(zapremninaS, out zapremnina))
                    {
                        if (zapremnina > 0)
                        {
                            if (selektiranaProstorija != null)
                            {
                                if (lbxOznakeNovogSpremnika.SelectedItems.Count != 0)
                                {
                                    if (txbBrojSpremnikaUnos.Text != "")
                                    {
                                        if (int.TryParse(txbBrojSpremnikaUnos.Text, out brojUnosa))
                                        {
                                            if (brojUnosa > 0)
                                            {
                                                List<int> idUnesenihSpremnika = PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije, globalniKorisnikID, brojUnosa);
                                                foreach (var idSpremnika in idUnesenihSpremnika)
                                                {
                                                    foreach (PrikazOznaka item in lbxOznakeNovogSpremnika.SelectedItems)
                                                    {
                                                        PrikazSpremnici.kreirajSpremnikOznaka(idSpremnika, item.id_oznaka);
                                                    }
                                                }
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
                                        List<int> idUnesenihSpremnika = PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije, globalniKorisnikID, 1);
                                        foreach (var idSpremnika in idUnesenihSpremnika)
                                        {
                                            foreach (PrikazOznaka item in lbxOznakeNovogSpremnika.SelectedItems)
                                            {
                                                PrikazSpremnici.kreirajSpremnikOznaka(idSpremnika, item.id_oznaka);
                                            }
                                        }
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
                                    MessageBox.Show("Spremnik mora imati barem jednu oznaku");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Morate odabrati prostoriju");
                            }
                        }
                        else
                        {
                                MessageBox.Show("Zapremnina mora bit pozitivan broj");
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
                MessageBox.Show("Naziv ne može biti prazan");
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
                lbxTrenutneOznakeSpremnikaUkloni.ItemsSource = PrikazOznaka.dohvatiOznakeSpremnika(s.id_spremnik);
                lbxDodaneOznakeSpremnika.ItemsSource = PrikazOznaka.dohvatiOznakeNePripadajuSpremniku(s.id_spremnik);
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
            bool dozvoljenaPromjena = true;
            string naziviTagovaZaBrisanje = "";
            List<PrikazOznaka> oznakeStavke = PrikazSpremnici.provjeraTagovaSpremnikaIStavke(Convert.ToInt32(txbSpremnikID.Text));
            List<PrikazOznaka> oznakeZaBrisanje = new List<PrikazOznaka>();
            foreach (PrikazOznaka item in lbxTrenutneOznakeSpremnikaUkloni.SelectedItems)
            {
                oznakeZaBrisanje.Add(item);
                foreach (PrikazOznaka item2 in oznakeStavke)
                {
                    if (item.id_oznaka == item2.id_oznaka)
                    {
                        naziviTagovaZaBrisanje += item.naziv + " ";
                        dozvoljenaPromjena = false;
                        break;
                    }
                }
                if (!dozvoljenaPromjena)
                {
                    break;
                }
            }
            double zapremnina;
            double[] popunjenost = new double[2];
            popunjenost = PrikazSpremnici.dohvatiPopunjenost(Convert.ToInt32(txbSpremnikID.Text));
            if (txbSpremnikNoviNaziv.Text!= "" && ((from c in txbSpremnikNoviNaziv.Text where c != ' ' select c).Count() != 0))
            {
                if (double.TryParse(txbSpremnikNovaZapremnina.Text, out zapremnina))
                {
                    if (zapremnina > 0)
                    {
                        if (zapremnina >= popunjenost[1])
                        {
                            if (dozvoljenaPromjena)
                            {
                                PrikazSpremnici.izmjeniSpremnik(Convert.ToInt32(txbSpremnikID.Text), txbSpremnikNoviNaziv.Text, Convert.ToDouble(txbSpremnikNovaZapremnina.Text), txbSpremnikNoviOpis.Text, IdProstorije);
                                if (lbxTrenutneOznakeSpremnikaUkloni.SelectedItems != null)
                                {
                                    PrikazSpremnici.obrisiOznakuSpremnika(Convert.ToInt32(txbSpremnikID.Text), oznakeZaBrisanje);
                                }
                                if (lbxDodaneOznakeSpremnika.SelectedItems != null)
                                {
                                    foreach (PrikazOznaka item in lbxDodaneOznakeSpremnika.SelectedItems)
                                    {
                                        PrikazSpremnici.kreirajSpremnikOznaka(Convert.ToInt32(txbSpremnikID.Text), item.id_oznaka);
                                    }
                                }
                                txbSpremnikNoviNaziv.Clear();
                                txbSpremnikNoviOpis.Clear();
                                txbSpremnikNovaZapremnina.Clear();
                                txbSpremnikID.Clear();
                                naslovLabel.Content = "Spremnici";
                                PrikaziSpremnike();
                                promjeniGrid("gridSpremnici");
                            }
                            else
                            {
                                MessageBox.Show("Ne možete obrisati sljedeću oznaku: '" + naziviTagovaZaBrisanje + "' jer se u spremniku nalaze stavke sa tom oznakom.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unešena zapremnina manja je od trenutnog zauzeća spremnika");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Zapremnina mora biti pozitivna");
                    }
                }
                else
                {
                    MessageBox.Show("Unesite brojčanu vrijednost u zapremninu");
                }
            }
            else
            {
                MessageBox.Show("Naziv ne može biti prazan");
            }
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

        private void SpremniciSearchEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string tekst = SpremniciSearch.Text;
                if(tekst == "")
                {
                    dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
                    dgSpremnici.Columns[4].Visibility = Visibility.Hidden;
                    promjeniHeaderSpremnici();
                }
                else
                {
                    dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnikeEnter(tekst);
                    dgSpremnici.Columns[4].Visibility = Visibility.Hidden;
                    promjeniHeaderSpremnici();
                }
            }
        }

        private void dgSpremnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSpremnici.SelectedItems.Count == 1)
            {
                PrikazSpremnici spremnik = (PrikazSpremnici)dgSpremnici.SelectedItem;
                if (spremnik != null)
                {
                    lbxTagoviSpremnika.Visibility = Visibility.Visible;
                    lbxTagoviSpremnika.ItemsSource = PrikazOznaka.dohvatiOznakeSpremnika(spremnik.idSpremnika);
                    double[] popunjenost = PrikazSpremnici.dohvatiPopunjenost(spremnik.idSpremnika);
                    double postotakZauz = popunjenost[1] / popunjenost[0];
                    if (popunjenost[0] != 0)
                    {
                        gridPopunjenost.Visibility = Visibility.Visible;
                        postotakZauzetosti.Width = new GridLength(postotakZauz, GridUnitType.Star);
                        postotakSlobodnog.Width = new GridLength(1 - postotakZauz, GridUnitType.Star);
                        postotakZauzetostiText.Content = 100 * Math.Round(postotakZauz, 4) + "%(" + popunjenost[1] + "/" + popunjenost[0] + ")";
                    }
                }
            }
            else
            {
                lbxTagoviSpremnika.Visibility = Visibility.Collapsed;
                gridPopunjenost.Visibility = Visibility.Collapsed;
            }
            
        }

        private void BtnkreirajPDFSpremnici_Click(object sender, RoutedEventArgs e)
        {
            PDFConverter.ExportPDFSpremnici();
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
                lbOznakeStavke.ItemsSource = PrikazOznaka.dohvatiOznakePripadajuStavci(stavka.idStavke);
            }

        }

        private void BtnKreirajStavku_Click(object sender, RoutedEventArgs e)
        {

            promjeniGrid("gridKreirajStavku");
            naslovLabel.Content = "Kreiraj stavku";

            cmbSpremniciKreirajStavku.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
            lbOznakeKreirajStavku.ItemsSource = PrikazOznaka.dohvatiOznake(); // tu se List<PrikazOznaka>

        }

        private void txbZauzimaKreirajStavku_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbZauzimaKreirajStavku.Text != "")
            {
                double zauzece;
                if (double.TryParse(txbZauzimaKreirajStavku.Text, out zauzece) && zauzece>0)
                {
                    if (lbOznakeKreirajStavku.SelectedItems.Count > 0)
                    {
                        List<PrikazSpremnici> filtriraniPoZauzecu;
                        filtriraniPoZauzecu = PrikazStavke.dohvatiDopusteneSpremnikeKolicine(zauzece, cmbSpremniciKreirajStavkuHidden.Items.Cast<PrikazSpremnici>().ToList());
                        cmbSpremniciKreirajStavku.ItemsSource = filtriraniPoZauzecu;
                        cmbSpremniciKreirajStavku.IsEnabled = true;
                    }
                    else
                    {
                        cmbSpremniciKreirajStavku.IsEnabled = false;
                    }
                }
                else
                {
                    cmbSpremniciKreirajStavku.IsEnabled = false;
                }
            }
            else
            {
                cmbSpremniciKreirajStavku.IsEnabled = false;
            }
        }

        private void lbOznakeKreirajStavku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbOznakeKreirajStavku.SelectedItems.Count > 0)
            {
                List<PrikazOznaka> odabraneOznake = lbOznakeKreirajStavku.SelectedItems.Cast<PrikazOznaka>().ToList();
                List<PrikazSpremnici> dopusteniSpremnici = PrikazStavke.dohvatiDopusteneSpremnikeOznake(odabraneOznake);
                cmbSpremniciKreirajStavku.ItemsSource = dopusteniSpremnici;
                cmbSpremniciKreirajStavkuHidden.ItemsSource = dopusteniSpremnici;
                if (txbZauzimaKreirajStavku.Text != "")
                {

                    double zauzece;
                    if (double.TryParse(txbZauzimaKreirajStavku.Text, out zauzece))
                    {
                        List<PrikazSpremnici> filtriraniPoZauzecu;
                        filtriraniPoZauzecu = PrikazStavke.dohvatiDopusteneSpremnikeKolicine(zauzece, cmbSpremniciKreirajStavkuHidden.Items.Cast<PrikazSpremnici>().ToList());
                        cmbSpremniciKreirajStavku.ItemsSource = filtriraniPoZauzecu;
                        cmbSpremniciKreirajStavku.IsEnabled = true;
                    }
                    else
                    {
                        cmbSpremniciKreirajStavku.IsEnabled = false;
                    }
                }
                else
                {
                    cmbSpremniciKreirajStavku.IsEnabled = false;
                }
            }
            else
            {
                cmbSpremniciKreirajStavku.IsEnabled = false;
            }
        }

        private void provjeraUnosaKreiranjeStavke()
        {

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

            List<PrikazOznaka> selektiraneOznake = new List<PrikazOznaka>();
            foreach (var item in lbOznakeKreirajStavku.SelectedItems)
            {
                PrikazOznaka oznake = (PrikazOznaka)item;
                selektiraneOznake.Add(oznake);
            }
            if (txbStavkaNoviNaziv.Text != "" && ((from c in txbStavkaNoviNaziv.Text where c != ' ' select c).Count() != 0))
            {
                if (selektiraneOznake.Count() != 0)
                {
                    double zauzima;
                    if (double.TryParse(txbZauzimaKreirajStavku.Text, out zauzima) && zauzima > 0)
                    {
                        if (cmbSpremniciKreirajStavku.SelectedItem != null)
                        {
                            double[] zapremninaSpremnika = PrikazSpremnici.dohvatiPopunjenost(selektiranSpremnik.idSpremnika);
                            if (zapremninaSpremnika[1] + zauzima <= zapremninaSpremnika[0])
                            {
                                if (PrikazOznakaStavka.provjeriStavkuOznakuUnos(selektiranSpremnik.idSpremnika, selektiraneOznake))
                                {//ako su ispravni(ako vrati true) onda nastavlja s unosom
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
                                    MessageBox.Show("Spremnik koji ste odabrali ne podržava odabrane oznake stavke.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("U odabrani spremnik ne stane ta stavka");
                                //mislim da nam ovo opce netreba vise...i ono za provjere oznaki isto?
                            }
                        }
                        else
                        {
                            MessageBox.Show("Odaberite spremnik!");
                            //se mogu odabrat 2 spremnika?
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kolicina mora biti broj i nesmije biti negativna!");
                    }
                }
                else
                {
                    MessageBox.Show("Odaberite neku oznaku!");
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
                txbIzmjeniStavkuStaroZauzeceHidden.Text= s.zauzeće.ToString();
                txbStavkaID.Text = s.id_stavka.ToString();
                txbIzmjeniStavkuSpremnikID.Text = s.spremnik_id.ToString();
                txbIzmjeniStavkuStariSpremnikID.Text= s.spremnik_id.ToString();
                txbIzmjeniStavkuStavkaIdSkriven.Text = Convert.ToString(s.id_stavka);

                int idStavke = Convert.ToInt32(txbIzmjeniStavkuStavkaIdSkriven.Text);


                List<PrikazOznaka> oznakeZaOdabranuStavku = new List<PrikazOznaka>();
                oznakeZaOdabranuStavku = PrikazOznaka.dohvatiOznakePripadajuStavci(idStavke);
                lbIzmjeniStavkuNjeneOznake.ItemsSource = oznakeZaOdabranuStavku;
                lbPocetneOznakeStavkeHidden.ItemsSource = oznakeZaOdabranuStavku;

                List<PrikazOznaka> oznakeNePripadajuStavci = new List<PrikazOznaka>();
                oznakeNePripadajuStavci = PrikazOznaka.dohvatiOznakeNePripadajuStavci(idStavke);
                lbIzmjeniStavkuOznake.ItemsSource = oznakeNePripadajuStavci;


                if (s.datum_roka.HasValue)
                {
                    var inMyString = s.datum_roka.Value.ToShortDateString();
                    dpIzmjeniStavkuNoviIstekRoka.SelectedDate = DateTime.Parse(inMyString);
                }

                dohvatiIspravneSpremnike();
            }
            else
            {
                MessageBox.Show("Niste odabrali stavku za izmjenu!");
            }

        }

        private void txbIzmjeniStavkuNovoZauzece_KeyUp(object sender, KeyEventArgs e)
        {
            if (txbIzmjeniStavkuNovoZauzece.Text != "")
            {
                double zauzece;
                if (double.TryParse(txbIzmjeniStavkuNovoZauzece.Text, out zauzece) && zauzece > 0)
                {
                    if (lbIzmjeniStavkuNjeneOznake.Items.Count > 0)
                    {
                        List<PrikazSpremnici> filtriraniPoZauzecu;
                        filtriraniPoZauzecu = PrikazStavke.dohvatiDopusteneSpremnikeKolicine(zauzece, cmbDopusteniSpremniciZaStavkuModifyHidden.Items.Cast<PrikazSpremnici>().ToList());
                        cmbDopusteniSpremniciZaStavkuModify.ItemsSource = filtriraniPoZauzecu;
                        cmbDopusteniSpremniciZaStavkuModify.IsEnabled = true;
                    }
                    else
                    {
                        cmbDopusteniSpremniciZaStavkuModify.IsEnabled = false;
                    }
                }
                else
                {
                    cmbDopusteniSpremniciZaStavkuModify.IsEnabled = false;
                }
            }
            else
            {
                cmbDopusteniSpremniciZaStavkuModify.IsEnabled = false;
            }
        }

        private void lbIzmjeniStavkuNjeneOznake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PrikazOznaka odabranaOznaka = new PrikazOznaka();
            odabranaOznaka = (PrikazOznaka)lbIzmjeniStavkuNjeneOznake.SelectedItem;
            if (odabranaOznaka != null)
            {
                int idStavke = Convert.ToInt32(txbIzmjeniStavkuStavkaIdSkriven.Text);
                PrikazOznakaStavka.obrisiStavkuOznaku(idStavke, odabranaOznaka.id_oznaka);
                osvjeziStavkineOznake();
                dohvatiIspravneSpremnike();

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
                dohvatiIspravneSpremnike();
            }
        }

        private void osvjeziStavkineOznake()
        {
            int idStavke = Convert.ToInt32(txbIzmjeniStavkuStavkaIdSkriven.Text);
            List<PrikazOznaka> oznakeZaOdabranuStavku = new List<PrikazOznaka>();
            oznakeZaOdabranuStavku=PrikazOznaka.dohvatiOznakePripadajuStavci(idStavke);
            lbIzmjeniStavkuNjeneOznake.ItemsSource = oznakeZaOdabranuStavku;

            List<PrikazOznaka> oznakeNePripadajuStavci = new List<PrikazOznaka>();
            oznakeNePripadajuStavci=PrikazOznaka.dohvatiOznakeNePripadajuStavci(idStavke);
            lbIzmjeniStavkuOznake.ItemsSource = oznakeNePripadajuStavci;
        }
        private void dohvatiIspravneSpremnike()
        {
            List<PrikazSpremnici> dopusteniSpremnici = PrikazStavke.dohvatiDopusteneSpremnikeOznake(lbIzmjeniStavkuNjeneOznake.Items.Cast<PrikazOznaka>().ToList());
            foreach(PrikazSpremnici item in dopusteniSpremnici)
            {
                if (item.idSpremnika == Convert.ToInt32(txbIzmjeniStavkuStariSpremnikID.Text))
                {
                    item.zauzece = item.zauzece - Convert.ToDouble(txbIzmjeniStavkuStaroZauzeceHidden.Text);
                }
            }
            cmbDopusteniSpremniciZaStavkuModify.ItemsSource = dopusteniSpremnici;
            cmbDopusteniSpremniciZaStavkuModifyHidden.ItemsSource = dopusteniSpremnici;
        }

        private void StavkeSearchEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string tekst = StavkeSearch.Text;
                dgStavke.ItemsSource = PrikazStavke.dohvatiStavkeEnter(tekst);
                promjeniHeaderStavke();
            }

        }

        private void StatistikaSearchEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string tekst = StatistikaSearch.Text;
                dgStatistika.ItemsSource = PrikazStatistika.dohvatiStatistike(tekst);
            }

            dgStatistika.Columns[0].Header = "Radnja";
            dgStatistika.Columns[1].Header = "Datum";
            dgStatistika.Columns[2].Header = "Kolicina";
            dgStatistika.Columns[3].Header = "Naziv stavke";
            dgStatistika.Columns[4].Header = "Naziv korisnika";
            dgStatistika.Columns[5].Header = "ID stavke";
            dgStatistika.Columns[6].Header = "Oznake";
        }

        private void btnIzmjeniStavkuOdustani_Click(object sender, RoutedEventArgs e)
        {
            PrikazOznakaStavka.obrisiSveOznakeStavke(Convert.ToInt32(txbStavkaID.Text));
            List<PrikazOznaka> stareOznake = lbPocetneOznakeStavkeHidden.Items.Cast<PrikazOznaka>().ToList();
            foreach(PrikazOznaka item in stareOznake)
            {
                PrikazOznakaStavka.dodajStavkuOznaku(Convert.ToInt32(txbStavkaID.Text), item.id_oznaka);
            }
            promjeniGrid("gridStavke");
            naslovLabel.Content = "Stavke";
            PrikaziStavke();
        }

        private void btnIzmjeniStavkuSpremi_Click(object sender, RoutedEventArgs e)
        {
            if(txbIzmjeniStavkuNoviNaziv.Text!="" && ((from c in txbIzmjeniStavkuNoviNaziv.Text where c != ' ' select c).Count() != 0))
            {
                double zauzima;
                if(double.TryParse(txbIzmjeniStavkuNovoZauzece.Text,out zauzima) && zauzima>0)
                {
                    PrikazSpremnici odabranSpremnik = (PrikazSpremnici)cmbDopusteniSpremniciZaStavkuModify.SelectedItem;
                    int idSpremnika;
                    DateTime? datum;
                    if (odabranSpremnik == null)//provjerava dal je selektiran novi spremnik,ako ne onda se sprema u stari
                    {
                        idSpremnika = int.Parse(txbIzmjeniStavkuSpremnikID.Text);

                    }
                    else
                    {
                        idSpremnika = odabranSpremnik.idSpremnika;
                    }

                    if (dpStavkaIstekRoka.SelectedDate == null)//ako je selektiran datum postavlja se novi,ako nije onda se postavlja na NULL (tu bi zapravo trebali stavit da se postavlja na staru vrijednost/ne mijenja se)
                    {
                        datum = null;
                    }
                    else
                    {
                        datum = dpIzmjeniStavkuNoviIstekRoka.SelectedDate.Value.Date;
                    }

                    if (PrikazOznakaStavka.provjeriStavkuOznakuUnos(idSpremnika, lbIzmjeniStavkuNjeneOznake.Items.Cast<PrikazOznaka>().ToList()))//svejedno se provjerava da spremnik podržava oznake,trebalo bi uvijek true bit
                    {
                        double zauzimaStaro = Convert.ToDouble(txbIzmjeniStavkuStaroZauzeceHidden.Text);
                        PrikazSpremnici.izmjeniZauzeceSpremnika(int.Parse(txbIzmjeniStavkuSpremnikID.Text), zauzimaStaro * -1); //tu se oduzima iz postojećeg
                        PrikazSpremnici.izmjeniZauzeceSpremnika(idSpremnika, zauzima); //tu se dodaje novom(koji moze bit i stari zapravo)
                        PrikazStavke.izmjeniStavku(Convert.ToInt32(txbStavkaID.Text), txbIzmjeniStavkuNoviNaziv.Text, idSpremnika, datum, zauzima, globalniKorisnikID);
                        naslovLabel.Content = "Stavke";
                        PrikaziStavke();
                        promjeniGrid("gridStavke");
                    }
                    else
                    {
                        MessageBox.Show("Trenutni spremnik ne podržava odabrane oznake");
                    }

                }
                else
                {
                    MessageBox.Show("Kolicina mora biti broj i veći od 0");
                }
            }
            else
            {
                MessageBox.Show("Naziv stavke ne može biti prazan");
            }

        }

        private void BtnkreirajPDFStavke_Click(object sender, RoutedEventArgs e)
        {
            PDFConverter.ExportPDFStavke();
        }

        private void btnkreirajPDFStavkePredIstekom_Click(object sender, RoutedEventArgs e)
        {
            var podatci = dohvatiDatotekuPodatke();
            PDFConverter.ExportPDFRokoviPredIstekom(podatci[0]);
        }

        private void btnkreirajPDFIstekleStavke_Click(object sender, RoutedEventArgs e)
        {
            PDFConverter.ExportPDFIsteceniRokovi();
        }

        private void btnInkrementirajStavku_Click(object sender, RoutedEventArgs e)
        {
            if (dgStavke.SelectedItems.Count == 1)
            {
                PrikazStavke ps = (PrikazStavke)dgStavke.SelectedItem;
                int broj;
                if (int.TryParse(txbStavkePromjeniKolicinu.Text, out broj))
                {
                    if (broj < 0) broj = broj * -1;
                    if (!PrikazStavke.promjeniKolicinuStavke(broj, ps.idStavke))
                    {
                        MessageBox.Show("Pokušavate unjeti više nego što stane u spremnik");
                    }
                }
                else
                {
                    if (!PrikazStavke.promjeniKolicinuStavke(1, ps.idStavke))
                    {
                        MessageBox.Show("Pokušavate unjeti više nego što stane u spremnik");
                    }
                }
                dgStavke.ItemsSource = PrikazStavke.dohvatiStavke();
                promjeniHeaderStavke();
            }
        }

        private void btnDekrementirajStavku_Click(object sender, RoutedEventArgs e)
        {
            if (dgStavke.SelectedItems.Count == 1)
            {
                PrikazStavke ps = (PrikazStavke)dgStavke.SelectedItem;
                int broj;
                if (int.TryParse(txbStavkePromjeniKolicinu.Text, out broj))
                {
                    if (broj > 0) broj = broj * -1;
                    if (!PrikazStavke.promjeniKolicinuStavke(broj, ps.idStavke))
                    {
                        MessageBox.Show("Pokušavate unjeti više nego što stane u spremnik");
                    }
                }
                else
                {
                    if (!PrikazStavke.promjeniKolicinuStavke(-1, ps.idStavke))
                    {
                        MessageBox.Show("Pokušavate unjeti više nego što stane u spremnik");
                    }
                }
                dgStavke.ItemsSource = PrikazStavke.dohvatiStavke();
                promjeniHeaderStavke();
            }
        }
        #endregion

        #region Oznake
        private void btnKreirajOznaku_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridKreirajOznaku");
            naslovLabel.Content = "Kreiraj oznaku";
        }
        private void btnKreirajOznakuSpremi_Click(object sender, RoutedEventArgs e)
        {
            bool oznaceno = false;
            int rezultatUnosa;
            if (rbtnKvarljiva.IsChecked == true || rbtnNeKvarljiva.IsChecked == true)
            {
                oznaceno = true;
            }
            if (txbNazivOznake.Text.Length > 0)
            {
                if (oznaceno)
                {
                    if (rbtnKvarljiva.IsChecked == true)
                    {
                        rezultatUnosa = PrikazOznaka.kreirajOznaku(txbNazivOznake.Text, "da");
                    }
                    else
                    {
                        rezultatUnosa = PrikazOznaka.kreirajOznaku(txbNazivOznake.Text, "ne");
                    }
                    if (rezultatUnosa != 1)
                    {
                        MessageBox.Show("Došlo je do pogreške kod unosa,molimo pokušajte ponovo");
                    }
                    else
                    {
                        txbNazivOznake.Text = null;
                        dgOznake.ItemsSource = PrikazOznaka.dohvatiSveOznake();
                        promjeniGrid("gridOznake");
                    }

                }
                else
                {
                    MessageBox.Show("Odaberite jedan od statusa kvarljivosti");
                }
            }
            else
            {
                MessageBox.Show("Unesite naziv nove oznake");
            }
        }
        private void btnKreirajOznakuOdustani_Click(object sender, RoutedEventArgs e)
        {
            promjeniGrid("gridOznake");
            naslovLabel.Content = "Oznake";
            PrikaziOznake();
        }
        private void btnPromjeniStatusOznake_Click(object sender, RoutedEventArgs e)
        {
            if (dgOznake.SelectedItems.Count != 0)
            {
                foreach (PrikazOznaka item in dgOznake.SelectedItems)
                {
                    string noviStatus;
                    PrikazOznaka odabranaOznaka = new PrikazOznaka();
                    odabranaOznaka = item;
                    if (odabranaOznaka.aktivna.Equals("da"))
                    {
                        noviStatus = "ne";
                    }
                    else
                    {
                        noviStatus = "da";
                    }
                    if (PrikazOznaka.promjeniStatusOznake(odabranaOznaka.id_oznaka, noviStatus) != 1)
                    {
                        MessageBox.Show("Došlo je do pogreške,molimo pokušajte ponovo");
                    }
                }
                PrikaziOznake();
            }
            else
            {
                MessageBox.Show("Odaberite oznaku kojoj želite promjeniti status");
            }
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
                    file.WriteLine(BrojDana.Text + "," + dark);
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

        private void btnkreirajPDFDnevnika_Click(object sender, RoutedEventArgs e)
        {
            PDFConverter.ExportPDFDnevnik();
        }


    }
}