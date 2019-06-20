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

        public static List<PrikazSpremnici> dohvatiSpremnike(string nazivProstorije)
        {
            List<PrikazSpremnici> sviSpremnici = new List<PrikazSpremnici>();

            using (var db = new SSDB())
            {
                var query = (from sp in db.spremnik
                             join pros in db.prostorija on sp.prostorija_id equals pros.id_prostorija
                             join tip in db.tip_spremnika on sp.tip_id equals tip.id_tip
                             where pros.naziv_prostorije == nazivProstorije
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
    }
}
