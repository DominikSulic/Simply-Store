﻿using System;
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

        public static stavka dohvatiStavku(int idStavke) {

            stavka stavka = new stavka();
            using (var db = new SSDB())
            {

                var query = (from stv in db.stavka where stv.id_stavka == idStavke select stv).First();
                stavka = query;

            }

            return stavka;

        }

        public static void kreirajStavku(string nazivStavke, int idSpremnika, List<PrikazOznaka> listaSelektiranihOznaka, DateTime datumIsteka, int zauzima) {
            stavka novaStavka = new stavka
            {
                
                naziv = nazivStavke,
                datum_kreiranja = DateTime.Now,
                datum_roka = datumIsteka,
                zauzeće = zauzima,
                spremnik_id = idSpremnika
                //oznaka = listaSelektiranihOznaka // dođe do greške, jer se ne može List<PrkazOznaka> convertati di ICollection<oznaka> koji je definiran u stavka.cs



            };

            
            
        }

        public static void obrisiStavku(int idStavke) {

            using (var db = new SSDB())
            {
                var query = (from stavka in db.stavka where stavka.id_stavka == idStavke select stavka).First();
                db.stavka.Attach(query);
                db.stavka.Remove(query);
                db.SaveChanges();
            }
        }

    }
    
}
