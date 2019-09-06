using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using WpfApp1;

namespace Testovi
{
    [TestClass]
    public class WPFTestovi
    {
        [TestMethod]
        public void Test_PrikazaniSviGridovi()
        {
            Application application = Application.Launch("WpfApp1.exe");
            Assert.IsNotNull(application);

            Window mainWindow = application.GetWindow("Simply Store");
            Assert.IsNotNull(mainWindow);

            TextBox tbKorisnickoIme = mainWindow.Get<TextBox>("txbKorisnickoIme");
            tbKorisnickoIme.Text = "filip";
            TextBox tbLozinka = mainWindow.Get<TextBox>("txbLozinka");
            tbLozinka.Text = "nou";

            Button loginButton = mainWindow.Get<Button>("btnPrijava");
            Assert.IsNotNull(loginButton);
            loginButton.Click();


            Button prostorijeButton = mainWindow.Get<Button>("menuProstorije");
            Assert.IsNotNull(prostorijeButton);
            prostorijeButton.Click();

            //Listview == datagrid
            ListView datagridProstorije = mainWindow.Get<ListView>("dgProstorije"); 
            Assert.IsNotNull(datagridProstorije);


            Button spremniciButton = mainWindow.Get<Button>("menuSpremnici");
            Assert.IsNotNull(spremniciButton);
            spremniciButton.Click();

            ListView datagridSpremnici = mainWindow.Get<ListView>("dgSpremnici");
            Assert.IsNotNull(datagridSpremnici);


            Button stavkeButton = mainWindow.Get<Button>("menuStavke");
            Assert.IsNotNull(stavkeButton);
            stavkeButton.Click();

            ListView datagridStavke = mainWindow.Get<ListView>("dgStavke");
            Assert.IsNotNull(datagridStavke);


            Button menuStavkePredIstekom = mainWindow.Get<Button>("menuStavkePredIstekom");
            Assert.IsNotNull(menuStavkePredIstekom);
            menuStavkePredIstekom.Click();

            ListView dgStavkePredIstekom = mainWindow.Get<ListView>("dgStavkePredIstekom");
            Assert.IsNotNull(dgStavkePredIstekom);

            ListView dgStavkeIstekle = mainWindow.Get<ListView>("dgStavkeIstekle");
            Assert.IsNotNull(dgStavkeIstekle);
            

            Button menuStatistika = mainWindow.Get<Button>("menuStatistika");
            Assert.IsNotNull(menuStatistika);
            menuStatistika.Click();

            ListView dgStatistika = mainWindow.Get<ListView>("dgStatistika");
            Assert.IsNotNull(dgStatistika);


            Button menuOznake = mainWindow.Get<Button>("menuOznake");
            Assert.IsNotNull(menuOznake);
            menuOznake.Click();

            ListView dgOznake = mainWindow.Get<ListView>("dgOznake");
            Assert.IsNotNull(dgOznake);
            
            mainWindow.Close();
        }

        [TestMethod]
        public void Test_UspjesnoKreiranaProstorija()
        {
            Application application = Application.Launch("WpfApp1.exe");
            Assert.IsNotNull(application);

            Window mainWindow = application.GetWindow("Simply Store");
            Assert.IsNotNull(mainWindow);

            Button loginButton = mainWindow.Get<Button>("btnPrijava");
            Assert.IsNotNull(loginButton);

            TextBox tbKorisnickoIme = mainWindow.Get<TextBox>("txbKorisnickoIme");
            tbKorisnickoIme.Text = "filip";
            TextBox tbLozinka = mainWindow.Get<TextBox>("txbLozinka");
            tbLozinka.Text = "nou";

            loginButton.Click();

            Button prostorijeButton = mainWindow.Get<Button>("menuProstorije");
            Assert.IsNotNull(prostorijeButton);
            prostorijeButton.Click();

            ListView listaPrijeDodavanjaProstorije = mainWindow.Get<ListView>("dgProstorije");

            int prije = listaPrijeDodavanjaProstorije.Rows.Count;

            Button kreirajProstoriju = mainWindow.Get<Button>("btnkreirajProstoriju");
            Assert.IsNotNull(kreirajProstoriju);
            kreirajProstoriju.Click();

            TextBox tbNazivProstorije = mainWindow.Get<TextBox>("txtNazivProstorije");
            tbNazivProstorije.Text = "UnitTestProstorija";

            Button spremiKreiranuProstoriju = mainWindow.Get<Button>("btnKreirajProstoriju");
            Assert.IsNotNull(spremiKreiranuProstoriju);
            spremiKreiranuProstoriju.Click();

            System.Threading.Thread.Sleep(3000);

            ListView listaPoslijeDodavanjaProstorije = mainWindow.Get<ListView>("dgProstorije");


            int poslije = listaPoslijeDodavanjaProstorije.Rows.Count;

            Assert.AreNotEqual(prije, poslije);
            mainWindow.Close();
        }

        [TestMethod]
        public void Test_UspjesnoObrisanaProstorija()
        {
            Application application = Application.Launch("WpfApp1.exe");
            Assert.IsNotNull(application);

            Window mainWindow = application.GetWindow("Simply Store");
            Assert.IsNotNull(mainWindow);

            TextBox tbKorisnickoIme = mainWindow.Get<TextBox>("txbKorisnickoIme");
            tbKorisnickoIme.Text = "filip";
            TextBox tbLozinka = mainWindow.Get<TextBox>("txbLozinka");
            tbLozinka.Text = "nou";

            Button loginButton = mainWindow.Get<Button>("btnPrijava");
            Assert.IsNotNull(loginButton);
            loginButton.Click();

            Button prostorijeButton = mainWindow.Get<Button>("menuProstorije");
            Assert.IsNotNull(prostorijeButton);
            prostorijeButton.Click();

            ListView listaPrijeBrisanjaProstorije = mainWindow.Get<ListView>("dgProstorije");
            int prije = listaPrijeBrisanjaProstorije.Rows.Count;

            Button obrisiProstoriju = mainWindow.Get<Button>("btnObrisiProstoriju");
            Assert.IsNotNull(obrisiProstoriju);

            int brojRedova = listaPrijeBrisanjaProstorije.Rows.Count;
            listaPrijeBrisanjaProstorije.Rows[brojRedova-1].Click();
            obrisiProstoriju.Click();

            ListView listaPoslijeBrisanjaProstorije = mainWindow.Get<ListView>("dgProstorije");
            int poslije = listaPoslijeBrisanjaProstorije.Rows.Count;

            Assert.AreNotEqual(prije, poslije);
            mainWindow.Close();
        }
    }
}