using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazSpremnici
    {
        public int idSpremnika { get; set; }
        public string nazivSpremnika { get; set; }
        public DateTime datumKreiranja { get; set; }
        public double zapremnina { get; set; }
        public string opis { get; set; }
        public string nazivProstorije { get; set; }
        public string nazivTipa { get; set; }

        public static List<PrikazSpremnici> dohvatiSpremnike()
        {
            List<PrikazSpremnici> sviSpremnici = new List<PrikazSpremnici>();

            using (var db = new SSDB())
            {
                var query = (from sp in db.spremnik
                             join pros in db.prostorija on sp.prostorija_id equals pros.id_prostorija
                             join tip in db.tip_spremnika on sp.tip_id equals tip.id_tip
                             select new PrikazSpremnici
                             {
                                 idSpremnika = sp.id_spremnik,
                                 nazivSpremnika = sp.naziv_spremnika,
                                 datumKreiranja = sp.datum_kreiranja,
                                 zapremnina = sp.zapremnina,
                                 opis = sp.opis,
                                 nazivProstorije = pros.naziv_prostorije,
                                 nazivTipa = tip.naziv
                             }).ToList();
                sviSpremnici = query;
            }
            return sviSpremnici;

        }

        public static List<PrikazSpremnici> dohvatiSpremnikeN(string tekst)
        {
            List<PrikazSpremnici> spremnici = new List<PrikazSpremnici>();
            string search = tekst.ToLower();
            using (var db = new SSDB())
            {
                var query = (from s in db.spremnik
                             join pros in db.prostorija on s.prostorija_id equals pros.id_prostorija
                             join tip in db.tip_spremnika on s.tip_id equals tip.id_tip
                             where s.naziv_spremnika.ToLower().Contains(search)
                             select new PrikazSpremnici
                             {
                                 idSpremnika = s.id_spremnik,
                                 nazivSpremnika = s.naziv_spremnika,
                                 datumKreiranja = s.datum_kreiranja,
                                 zapremnina = s.zapremnina,
                                 opis = s.opis,
                                 nazivProstorije = pros.naziv_prostorije,
                                 nazivTipa = tip.naziv
                             }).ToList();
                spremnici = query;
            }
            return spremnici;

        }

        public static List<PrikazSpremnici> dohvatiSpremnike(PrikazProstorije nazivProstorije)
        {
            List<PrikazSpremnici> sviSpremnici = new List<PrikazSpremnici>();

            using (var db = new SSDB())
            {
                var query = (from sp in db.spremnik
                             join pros in db.prostorija on sp.prostorija_id equals pros.id_prostorija
                             join tip in db.tip_spremnika on sp.tip_id equals tip.id_tip
                             where pros.id_prostorija == nazivProstorije.idProstorije
                             select new PrikazSpremnici
                             {
                                 idSpremnika = sp.id_spremnik,
                                 nazivSpremnika = sp.naziv_spremnika,
                                 datumKreiranja = sp.datum_kreiranja,
                                 zapremnina = sp.zapremnina,
                                 opis = sp.opis,
                                 nazivProstorije = pros.naziv_prostorije,
                                 nazivTipa = tip.naziv
                             }).ToList();
                sviSpremnici = query;
            }
            return sviSpremnici;

        }

        public static List<string> dohvatiNaziveSpremnika()
        {
            List<string> naziviSpremnika = new List<string>();

            using (var db = new SSDB())
            {
                var query = (from s in db.spremnik select s.naziv_spremnika).ToList();
                naziviSpremnika = query;
            }

            return naziviSpremnika;
        }

        public static void kreirajSpremnik(string naziv, double zapremnina, string opis, int idProstorije, int idTipa, int brojUnosa = 1)
        {
            for (int i = 0; i < brojUnosa; i++)
            {
                spremnik noviSpremnik = new spremnik
                {
                    naziv_spremnika = naziv + " ("+(i+1)+")",
                    datum_kreiranja = DateTime.Now,
                    zapremnina = zapremnina,
                    opis = opis,
                    prostorija_id = idProstorije,
                    tip_id = idTipa
                };

                using (var db = new SSDB())
                {
                    db.spremnik.Add(noviSpremnik);
                    db.SaveChanges();
                }
            }
            
        }

        public static void izmjeniSpremnik(int id, string noviNaziv, double zapremnina,string noviOpis, int idProstorije)
        {
            using (var db = new SSDB())
            {
                var query = (from sprem in db.spremnik where sprem.id_spremnik == id select sprem).First();
                query.naziv_spremnika = noviNaziv;
                query.opis = noviOpis;
                query.prostorija_id = idProstorije;
                query.zapremnina = zapremnina;
                db.SaveChanges();
            }
        }

        public static spremnik dohvatiSpremnik(int idSpremnika)
        {
            spremnik spremnik = new spremnik();
            using (var db = new SSDB())
            {
                var query = (from sprem in db.spremnik where sprem.id_spremnik == idSpremnika select sprem).First();
                spremnik = query;
            }
            return spremnik;
        }

        public static void obrisiSpremnik(int idSpremnika)
        {
            using (var db = new SSDB())
            {
                var query = (from sprem in db.spremnik where sprem.id_spremnik == idSpremnika select sprem).First();
                db.spremnik.Attach(query);
                db.spremnik.Remove(query);
                db.SaveChanges();
            }
        }

        public override string ToString()
        {
            return nazivSpremnika;
        }
    }
}
