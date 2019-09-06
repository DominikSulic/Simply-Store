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
                             where p.aktivna == "da"
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

        public static List<PrikazProstorije> dohvatiProstorijeEnter(string tekst)
        {
            List<PrikazProstorije> prostorije = new List<PrikazProstorije>();
            using (var db = new SSDB())
            {
                var query = (from p in db.prostorija
                             where p.naziv_prostorije.ToLower().Contains(tekst.ToLower())
                             select new PrikazProstorije
                             {
                                 idProstorije = p.id_prostorija,
                                 nazivProstorije = p.naziv_prostorije,
                                 datumKreiranja = p.datum_kreiranja,
                                 opis = p.opis,
                                 posebneNapomene = p.posebne_napomene
                             }).ToList();
                prostorije = query;
            }
            return prostorije;
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

        public static void kreirajProstoriju(string naziv, string opis, string napomene, int brojProstorija, int idKorisnika)
        {
            int broj = brojProstorija;

            if (broj == 1)
            {
                prostorija prostorija = new prostorija
                {
                    naziv_prostorije = naziv,
                    datum_kreiranja = DateTime.Now,
                    opis = opis,
                    posebne_napomene = napomene,
                    aktivna = "da",
                    korisnik_id = idKorisnika
                };

                using (var db = new SSDB())
                {
                    db.prostorija.Add(prostorija);
                    db.SaveChanges();
                }
            }
            else
            {
                for (int i = 1; i <= broj; i++)
                {
                    prostorija prostorija = new prostorija
                    {
                        naziv_prostorije = naziv + " (" + i + ")",
                        datum_kreiranja = DateTime.Now,
                        opis = opis,
                        posebne_napomene = napomene,
                        aktivna = "da",
                        korisnik_id = idKorisnika
                    };

                    using (var db = new SSDB())
                    {
                        db.prostorija.Add(prostorija);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void obrisiProstoriju(int idProstorije, int idKorisnik)
        {
            List<PrikazSpremnici> listaSpremnika = new List<PrikazSpremnici>();
            List<PrikazStavke> listaStavki = new List<PrikazStavke>();

            using (var db = new SSDB())
            {
                var query = (from p in db.prostorija where p.id_prostorija == idProstorije select p).First();
                query.aktivna = "ne";

                listaSpremnika = PrikazSpremnici.dohvatiSpremnike(query.id_prostorija);
                foreach (PrikazSpremnici ps in listaSpremnika)
                {
                    var query2 = (from s in db.spremnik where s.id_spremnik == ps.idSpremnika select s).First();
                    query2.zapremnina = 0;
                    listaStavki = PrikazStavke.dohvatiStavke(query2.id_spremnik);
                    foreach (PrikazStavke ps2 in listaStavki)
                    {
                        double staroZauzece = PrikazStavke.dohvatiStaroZauzece(ps2.idStavke);
                        double novaKolicina = 0 - staroZauzece;

                        dnevnik noviDnevnik = new dnevnik
                        {
                            radnja = "ažuriranje",
                            datum = DateTime.Now,
                            kolicina = novaKolicina,
                            korisnik_id = idKorisnik,
                            stavka_id = ps2.idStavke
                        };

                        var query3 = (from stv in db.stavka where stv.id_stavka == ps2.idStavke select stv).First();
                        query3.zauzeće = 0;
                        db.dnevnik.Add(noviDnevnik);
                        db.SaveChanges();
                    }
                }

                db.SaveChanges();
            }
        }

        public prostorija dohvatiProstoriju(string staraProstorija)
        {
            prostorija prostorija = new prostorija();
            using (var db = new SSDB())
            {
                var query = (from p in db.prostorija where p.naziv_prostorije == nazivProstorije select p).First();
                prostorija = query;
            }
            return prostorija;
        }

        public static void izmjeniProstoriju(int id, string noviNaziv, string noviOpis, string noveNapomene)
        {
            using (var db = new SSDB())
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
