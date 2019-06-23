﻿using System;
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
            cmbTipSpreminka.ItemsSource = PrikazTipSpremnika.dohvatiTipSpremnika();


        }

        private void btnKreirajSpremnikSpremi_Click(object sender, RoutedEventArgs e)
        {
            PrikazTipSpremnika selektiranTipSpremnika = new PrikazTipSpremnika();
            selektiranTipSpremnika = (PrikazTipSpremnika)cmbTipSpreminka.SelectedItem;

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
                            if (selektiranTipSpremnika != null)
                            {
                                if (txbBrojSpremnikaUnos.Text != "")
                                {
                                    if (int.TryParse(txbBrojSpremnikaUnos.Text, out brojUnosa))
                                    {
                                        if (brojUnosa > 0)
                                        {
                                            PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije, selektiranTipSpremnika.idTipSpremnika,brojUnosa);
                                            txbNoviSpremnikNaziv.Clear();
                                            txbNoviSpremnikOpis.Clear();
                                            txbNoviSpremnikZapremnina.Clear();
                                            cmbTipSpreminka.SelectedItem = null;
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
                                    PrikazSpremnici.kreirajSpremnik(txbNoviSpremnikNaziv.Text, zapremnina, txbNoviSpremnikOpis.Text, selektiranaProstorija.idProstorije, selektiranTipSpremnika.idTipSpremnika);
                                    txbNoviSpremnikNaziv.Clear();
                                    txbNoviSpremnikOpis.Clear();
                                    txbNoviSpremnikZapremnina.Clear();
                                    cmbTipSpreminka.SelectedItem = null;
                                    cmbProstorijeKreiranjeSpremnika.SelectedItem = null;
                                    naslovLabel.Content = "Spremnici";
                                    PrikaziSpremnike();
                                    promjeniGrid("gridSpremnici");
                                }
                                
                                
                            }
                            else
                            {
                                MessageBox.Show("Morate odabrati tip spremnika");
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
            PrikazSpremnici.izmjeniSpremnik(Convert.ToInt32(txbSpremnikID.Text), txbSpremnikNoviNaziv.Text, Convert.ToDouble(txbSpremnikNovaZapremnina.Text), txbSpremnikNoviOpis.Text, odabranaProstorija.idProstorije);
            txbSpremnikNoviNaziv.Clear();
            txbSpremnikNoviOpis.Clear();
            txbSpremnikNovaZapremnina.Clear();
            txbSpremnikID.Clear();
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();
            promjeniGrid("gridSpremnici");
        }

<<<<<<< HEAD
        private void menuIzlaz_Click(object sender, RoutedEventArgs e)
=======
        private void btnKreirajSpremnikOdustani_Click(object sender, RoutedEventArgs e)
        {

            promjeniGrid("gridSpremnici");
            naslovLabel.Content = "Spremnici";
            PrikaziSpremnike();

        }

        private void SpremniciSearch_TextChanged(object sender, RoutedEventArgs e)
>>>>>>> Stavke
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

            List<PrikazOznaka> selektiraneOznake = new List<PrikazOznaka>();
            foreach (var item in lbOznakeKreirajStavku.SelectedItems)
            {
                PrikazOznaka oznake = (PrikazOznaka)item;
                selektiraneOznake.Add(oznake);
            } 

            int zauzima = Convert.ToInt32(txbZauzimaKreirajStavku.Text);
            
            PrikazStavke.kreirajStavku(txbStavkaNoviNaziv.Text, selektiranSpremnik.idSpremnika, selektiraneOznake, dpStavkaIstekRoka.SelectedDate.Value.Date, zauzima);

            txbStavkaNoviNaziv.Clear();
            txbZauzimaKreirajStavku.Clear();
            dpStavkaIstekRoka.SelectedDate = null;
            cmbSpremniciKreirajStavku.SelectedItem = null;
            lbOznakeKreirajStavku.SelectedItem = null;
            naslovLabel.Content = "Stavke";
            PrikaziStavke();
            promjeniGrid("gridStavke");
            

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
<<<<<<< HEAD

        private void btnPrikaziSveSpremnike_Click(object sender, RoutedEventArgs e)
        {
            cmbProstorije.ItemsSource = PrikazProstorije.dohvatiProstorije();
            cmbProstorije.SelectedIndex = -1;
            dgSpremnici.ItemsSource = PrikazSpremnici.dohvatiSpremnike();
        }
    }
=======
>>>>>>> Stavke

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

            int zauzima = Convert.ToInt32(txbIzmjeniStavkuNovoZauzece.Text);
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
            

            PrikazStavke.izmjeniStavku(Convert.ToInt32(txbStavkaID.Text), txbIzmjeniStavkuNoviNaziv.Text, idSpremnika, dpIzmjeniStavkuNoviIstekRoka.SelectedDate.Value.Date, zauzima);

            naslovLabel.Content = "Stavke";
            PrikaziStavke();
            promjeniGrid("gridStavke");

        }

        #endregion


    }

}
