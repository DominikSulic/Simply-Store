using System;
using System.Collections.Generic;
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
        public float zauzece { get; set; }
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

        public static List<PrikazStavke> dohvatiStavkeN(string tekst)
        {
            List<PrikazStavke> stavke = new List<PrikazStavke>();
            string search = tekst.ToLower();
            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where s.naziv.ToLower().Contains(search)
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
                             where d.naziv_spremnika == nazivSpremnika
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
    }
}
