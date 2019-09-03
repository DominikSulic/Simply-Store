using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazStavke
    {
        public int idStavke { get; set; }
        public string nazivStavke { get; set; }
        public DateTime datumKreiranja { get; set; }
        public DateTime? datumRoka { get; set; }
        public double zauzece { get; set; }
        public string nazivSpremnika { get; set; }
        public string nazivProstorije { get; set; }

        public static List<PrikazStavke> dohvatiStavke()
        {
            List<PrikazStavke> sveStavke = new List<PrikazStavke>();

            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where s.zauzeće > 0
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 nazivStavke = s.naziv,
                                 datumKreiranja = s.datum_kreiranja,
                                 datumRoka = s.datum_roka,
                                 zauzece = s.zauzeće,
                                 nazivSpremnika = d.naziv_spremnika,
                                 nazivProstorije = p.naziv_prostorije,

                             }).ToList();
                sveStavke = query;
            }

            return sveStavke;
        }

        public static List<PrikazStavke> dohvatiStavkeN(string tekst)
        {
            List<PrikazStavke> stavke = new List<PrikazStavke>();
            string search = tekst.ToLower();
            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where s.naziv.ToLower().Contains(search) && s.zauzeće > 0
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 nazivStavke = s.naziv,
                                 datumKreiranja = s.datum_kreiranja,
                                 datumRoka = s.datum_roka,
                                 zauzece = s.zauzeće,
                                 nazivSpremnika = d.naziv_spremnika,
                                 nazivProstorije = p.naziv_prostorije

                             }).ToList();
                stavke = query;
            }
            return stavke;

        }

        public static List<PrikazStavke> dohvatiStavke(string nazivSpremnika)
        {
            List<PrikazStavke> sveStavke = new List<PrikazStavke>();

            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where d.naziv_spremnika == nazivSpremnika && s.zauzeće > 0
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 nazivStavke = s.naziv,
                                 datumKreiranja = s.datum_kreiranja,
                                 datumRoka = s.datum_roka,
                                 zauzece = s.zauzeće,
                                 nazivSpremnika = d.naziv_spremnika,
                                 nazivProstorije = p.naziv_prostorije


                             }).ToList();
                sveStavke = query;
            }

            return sveStavke;
        }

        public static stavka dohvatiStavku(int idStavke)
        {

            stavka stavka = new stavka();
            using (var db = new SSDB())
            {

                var query = (from stv in db.stavka where stv.id_stavka == idStavke select stv).First();
                stavka = query;

            }

            return stavka;

        }

        public static void kreirajStavku(string nazivStavke, int idSpremnika, List<PrikazOznaka> listaSelektiranihOznaka, DateTime? datumIsteka, double zauzima, int korisnikID)
        {

            ICollection<oznaka> oznake = new List<oznaka>();

            foreach (var item in listaSelektiranihOznaka)
            {
                oznaka oznaka = new oznaka
                {
                    id_oznaka = item.id_oznaka,
                    naziv = item.naziv,
                    kvarljivost = item.kvarljivost
                };
                oznake.Add(oznaka);
            }

            stavka novaStavka = new stavka
            {
                naziv = nazivStavke,
                datum_kreiranja = DateTime.Now,
                datum_roka = datumIsteka,
                zauzeće = zauzima,
                spremnik_id = idSpremnika
            };

            dnevnik noviDnevnik = new dnevnik
            {
                radnja = "kreiranje",
                datum = DateTime.Now,
                kolicina = zauzima,
                korisnik_id = korisnikID
            };

            using (var db = new SSDB())
            {
                novaStavka.oznaka = new List<oznaka>();
                foreach (var item in oznake)
                {
                    db.oznaka.Attach(item);
                    novaStavka.oznaka.Add(item);
                }
                db.stavka.Add(novaStavka);
                noviDnevnik.stavka_id = novaStavka.id_stavka;
                db.dnevnik.Add(noviDnevnik);
                db.SaveChanges();
            }

        }

        public static void izmjeniStavku(int id, string noviNaziv, int idSpremnika, DateTime? datumIsteka, double zauzima, int korisnikID)
        {
            double staroZauzece = dohvatiStaroZauzece(id);
            double novaKolicina = zauzima - staroZauzece;

            dnevnik noviDnevnik = new dnevnik
            {
                radnja = "ažuriranje",
                datum = DateTime.Now,
                kolicina = novaKolicina,
                korisnik_id = korisnikID,
                stavka_id = id
            };

            using (var db = new SSDB())
            {
                var query = (from stv in db.stavka where stv.id_stavka == id select stv).First();
                query.naziv = noviNaziv;
                query.spremnik_id = idSpremnika;
                query.datum_roka = datumIsteka;
                query.zauzeće = zauzima;
                db.dnevnik.Add(noviDnevnik);
                db.SaveChanges();
            }
        }

        public static void obrisiStavku(int idStavke, int korisnikID)
        {
            dnevnik noviDnevnik = new dnevnik
            {
                radnja = "brisanje",
                datum = DateTime.Now,
                korisnik_id = korisnikID,
                stavka_id = idStavke
            };

            using (var db = new SSDB())
            {
                var query = (from stavka in db.stavka where stavka.id_stavka == idStavke select stavka).First();
                db.stavka.Attach(query);
                noviDnevnik.kolicina = query.zauzeće;
                query.zauzeće = 0;
                db.dnevnik.Add(noviDnevnik);
                db.SaveChanges();
            }
        }

        public static List<PrikazStavke> dohvatiStavkePredIstekom(int brojDana)
        {
            List<PrikazStavke> sveStavke = new List<PrikazStavke>();
            DateTime? now = DateTime.Now;


            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where System.Data.Objects.SqlClient.SqlFunctions.DateDiff("dd", now, s.datum_roka) < brojDana
                             && now < s.datum_roka && s.zauzeće > 0
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 nazivStavke = s.naziv,
                                 datumKreiranja = s.datum_kreiranja,
                                 datumRoka = s.datum_roka,
                                 zauzece = s.zauzeće,
                                 nazivSpremnika = d.naziv_spremnika,
                                 nazivProstorije = p.naziv_prostorije

                             }).ToList();
                sveStavke = query;
            }

            return sveStavke;
        }

        public static List<PrikazStavke> stavkeIstecenogRoka()
        {
            List<PrikazStavke> lista = new List<PrikazStavke>();

            DateTime? now = DateTime.Now;

            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where now > s.datum_roka && s.zauzeće > 0
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 nazivStavke = s.naziv,
                                 datumKreiranja = s.datum_kreiranja,
                                 datumRoka = s.datum_roka,
                                 zauzece = s.zauzeće,
                                 nazivSpremnika = d.naziv_spremnika,
                                 nazivProstorije = p.naziv_prostorije

                             }).ToList();
                lista = query;
            }

            return lista;
        }

        public static double dohvatiStaroZauzece(int stavkaId)
        {
            double zauzece;
            using (var db = new SSDB())
            {
                var query = (from s in db.stavka where s.id_stavka == stavkaId select s.zauzeće).First();
                zauzece = Convert.ToDouble(query);
            }
            return zauzece;
        }

        public static List<PrikazStavke> dohvatiStavke(int idSpremnika)
        {
            List<PrikazStavke> sveStavke = new List<PrikazStavke>();

            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             where s.zauzeće > 0 && s.spremnik_id == idSpremnika
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 zauzece = s.zauzeće,
                             }).ToList();
                sveStavke = query;
            }

            return sveStavke;
        }
    }
}
