using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazProstorije
    {
        public int idProstorije { get; set; }    
        public string nazivProstorije { get; set; }
        public DateTime datumKreiranja { get; set; }
        public string opis { get; set; }
        public string posebneNapomene { get; set; }

        public static List<PrikazProstorije> dohvatiProstorije()
        {
            List<PrikazProstorije> sveProstorije = new List<PrikazProstorije>();

            using (var db = new SSDB())
            {
                var query = (from p in db.prostorija
                             select new PrikazProstorije
                             {
                                 idProstorije = p.id_prostorija,
                                 nazivProstorije = p.naziv_prostorije,
                                 datumKreiranja = p.datum_kreiranja,
                                 opis = p.opis,
                                 posebneNapomene = p.posebne_napomene
                             }).ToList();
                sveProstorije = query;
            }
            return sveProstorije;

        }

        public static List<string> dohvatiNaziveProstorija()
        {
            List<string> sviNazivi = new List<string>();
            using (var db = new SSDB())
            {
                var query = (from p in db.prostorija
                             select p.naziv_prostorije
                             ).ToList();
                sviNazivi = query;
            }
            return sviNazivi;
        }

        public static void kreirajProstoriju(string naziv, string opis, string napomene) 
        {
            prostorija prostorija = new prostorija
            {
                naziv_prostorije = naziv,
                datum_kreiranja = DateTime.Now,
                opis = opis,
                posebne_napomene = napomene
            };

            using(var db = new SSDB())
            {
                db.prostorija.Add(prostorija);
                db.SaveChanges();
            }
        }

        public void obrisiProstoriju(string nazivProstorije)
        {
            using (var db = new SSDB())
            {
                var query = (from p in db.prostorija where p.naziv_prostorije == nazivProstorije select p).First();
                db.prostorija.Attach(query);
                db.prostorija.Remove(query);
                db.SaveChanges();
            }
        }

        public prostorija dohvatiProstoriju(string staraProstorija)
        {
            prostorija prostorija = new prostorija();
            using(var db = new SSDB())
            {
                var query = (from p in db.prostorija where p.naziv_prostorije == nazivProstorije select p).First();
                prostorija = query;
            }
            return prostorija;
        }

        public static void izmjeniProstoriju(int id, string noviNaziv, string noviOpis, string noveNapomene)
        {
            using(var db = new SSDB())
            {
                var query = (from p in db.prostorija where p.id_prostorija == id select p).First();
                query.naziv_prostorije = noviNaziv;
                query.opis = noviOpis;
                query.posebne_napomene = noveNapomene;
                db.SaveChanges();
            }
        }

        public override string ToString()
        {
            return nazivProstorije;
        }
    }
}
