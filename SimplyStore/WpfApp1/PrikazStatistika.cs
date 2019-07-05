using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazStatistika
    {
        public int idZapisa { get; set; }
        public string radnja { get; set; }
        public DateTime datum { get; set; }
        public double? kolicina { get; set; }
        public string nazivStavke { get; set; }
        public string nazivKorisnika { get; set; }

        public static List<PrikazStatistika> dohvatiStatistike()
        {
            List<PrikazStatistika> sveStatistike = new List<PrikazStatistika>();
            using (var db = new SSDB())
            {
                var query = (from d in db.dnevnik
                             join s in db.stavka on d.stavka_id equals s.id_stavka
                             join k in db.korisnik on d.korisnik_id equals k.id_korisnik
                             select new PrikazStatistika
                             {
                                 idZapisa = d.id_dnevnik,
                                 radnja = d.radnja,
                                 datum= d.datum,
                                 kolicina = d.kolicina,
                                 nazivStavke = s.naziv,
                                 nazivKorisnika = k.korisnicko_ime

                             }).ToList();
                sveStatistike = query;
            }

            return sveStatistike;
        }

        public static List<PrikazStatistika> dohvatiStatistike(string tekst)
        {
            List<PrikazStatistika> lista = new List<PrikazStatistika>();
            int broj;

            DateTime? now = DateTime.Now;

            if(int.TryParse(tekst, out broj))
            {
                using(var db = new SSDB())
                {
                    var query = (from d in db.dnevnik
                                 join s in db.stavka on d.stavka_id equals s.id_stavka
                                 join k in db.korisnik on d.korisnik_id equals k.id_korisnik
                                 where System.Data.Objects.SqlClient.SqlFunctions.DateDiff("dd", now, d.datum) < broj
                                 select new PrikazStatistika
                                 {
                                     idZapisa = d.id_dnevnik,
                                     radnja = d.radnja,
                                     datum = d.datum,
                                     kolicina = d.kolicina,
                                     nazivStavke = s.naziv,
                                     nazivKorisnika = k.korisnicko_ime
                                 }).ToList();
                    lista = query;
                }
            }
            else
            {
                using (var db = new SSDB())
                {
                    tekst = tekst.ToLower();
                    var query = (from d in db.dnevnik
                                 join s in db.stavka on d.stavka_id equals s.id_stavka
                                 join k in db.korisnik on d.korisnik_id equals k.id_korisnik
                                 where s.naziv.Contains(tekst) || k.korisnicko_ime.Contains(tekst)
                                 select new PrikazStatistika
                                 {
                                     idZapisa = d.id_dnevnik,
                                     radnja = d.radnja,
                                     datum = d.datum,
                                     kolicina = d.kolicina,
                                     nazivStavke = s.naziv,
                                     nazivKorisnika = k.korisnicko_ime
                                 }).ToList();
                    lista = query;
                }
            }

            return lista;
        }
    }
}
