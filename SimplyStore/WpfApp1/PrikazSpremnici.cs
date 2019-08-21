using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public static List<PrikazSpremnici> dohvatiSpremnike()
        {
            List<PrikazSpremnici> sviSpremnici = new List<PrikazSpremnici>();

            using (var db = new SSDB())
            {
                var query = (from sp in db.spremnik
                             join pros in db.prostorija on sp.prostorija_id equals pros.id_prostorija
                             where sp.zapremnina>0
                             select new PrikazSpremnici
                             {
                                 idSpremnika = sp.id_spremnik,
                                 nazivSpremnika = sp.naziv_spremnika,
                                 datumKreiranja = sp.datum_kreiranja,
                                 zapremnina = sp.zapremnina,
                                 opis = sp.opis,
                                 nazivProstorije = pros.naziv_prostorije,
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
                             where s.naziv_spremnika.ToLower().Contains(search) && s.zapremnina > 0 && s.zapremnina > 0
                             select new PrikazSpremnici
                             {
                                 idSpremnika = s.id_spremnik,
                                 nazivSpremnika = s.naziv_spremnika,
                                 datumKreiranja = s.datum_kreiranja,
                                 zapremnina = s.zapremnina,
                                 opis = s.opis,
                                 nazivProstorije = pros.naziv_prostorije,
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
                             where pros.id_prostorija == nazivProstorije.idProstorije && sp.zapremnina > 0
                             select new PrikazSpremnici
                             {
                                 idSpremnika = sp.id_spremnik,
                                 nazivSpremnika = sp.naziv_spremnika,
                                 datumKreiranja = sp.datum_kreiranja,
                                 zapremnina = sp.zapremnina,
                                 opis = sp.opis,
                                 nazivProstorije = pros.naziv_prostorije,
                             }).ToList();
                sviSpremnici = query;
            }
            return sviSpremnici;

        }

        public static List<int> kreirajSpremnik(string naziv, double zapremnina, string opis, int idProstorije,int idKorisnika, int brojUnosa = 1)
        {
            List<int> idNovogSpremnika = new List<int>();
            for (int i = 0; i < brojUnosa; i++)
            {
                spremnik noviSpremnik = new spremnik
                {
                    naziv_spremnika = naziv + " (" + (i + 1) + ")",
                    datum_kreiranja = DateTime.Now,
                    zapremnina = zapremnina,
                    opis = opis,
                    prostorija_id = idProstorije,
                    korisnik_id = idKorisnika
                };

                using (var db = new SSDB())
                {
                    spremnik a = db.spremnik.Add(noviSpremnik);
                    db.SaveChanges();
                    idNovogSpremnika.Add(a.id_spremnik);
                    
                }
            }
            return idNovogSpremnika;
        }

        public static int kreirajSpremnikOznaka(int idSpremnik,int idOznaka)
        {
            int rezultat;
            string upit = "INSERT INTO spremnik_oznaka(spremnik_id,oznaka_id) VALUES(" + idSpremnik + "," + idOznaka + ")";
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit, connection);
                int insert = command.ExecuteNonQuery();
                rezultat = insert;
                connection.Close();
            }
            return rezultat;
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

        public static List<PrikazOznaka> provjeraTagovaSpremnikaIStavke(int idSpremnika)//prije UPDATE provjerava dal u spremniku postoje stavke sa tagom koji želimo obrisati,ako da ne dopušta
        {
            List<PrikazOznaka> oznake = new List<PrikazOznaka>();
            string upit = "SELECT DISTINCT oznaka.* FROM oznaka,stavka_oznaka,stavka WHERE oznaka.id_oznaka=stavka_oznaka.oznaka_id AND stavka.id_stavka=stavka_oznaka.stavka_id AND stavka.zauzeće>0 AND stavka.spremnik_id="+idSpremnika;
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PrikazOznaka a = new PrikazOznaka();
                        a.id_oznaka = reader.GetInt32(0);
                        a.naziv = reader.GetString(1);
                        a.kvarljivost = reader.GetString(2);
                        oznake.Add(a);
                    }
                }

                reader.Close();
                connection.Close();
               
            }
            return oznake;
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

        public static void obrisiSpremnik(int idSpremnika, int korisnikID)
        {
            using (var db = new SSDB())
            {
                var query = (from sprem in db.spremnik where sprem.id_spremnik == idSpremnika select sprem).First();
                db.spremnik.Attach(query);
                query.zapremnina = 0;
                db.SaveChanges();

                foreach(var item in db.stavka)
                {
                    if (item.spremnik_id == idSpremnika)
                    {
                        dnevnik noviDnevnik = new dnevnik
                        {
                            radnja = "bris. Spremnik",
                            datum = DateTime.Now,
                            korisnik_id = korisnikID,
                            stavka_id = item.id_stavka,
                            kolicina = item.zauzeće
                            
                        };
                        db.dnevnik.Add(noviDnevnik);
                        db.stavka.Attach(item);
                        item.zauzeće = 0;
                    }
                }
                db.SaveChanges();
            }

        }

        public static void obrisiOznakuSpremnika(int idSpremnika,List<PrikazOznaka> oznakeZaBrisanje)
        {
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach(PrikazOznaka item in oznakeZaBrisanje)
                {
                    string upit = "DELETE FROM spremnik_oznaka WHERE spremnik_id="+idSpremnika+" AND oznaka_id=" + item.id_oznaka;
                    SqlCommand command = new SqlCommand(upit, connection);
                    command.ExecuteNonQuery();
                    
                }
                connection.Close();
            }
            }

        public override string ToString()
        {
            return nazivSpremnika;
        }

        public static double[] dohvatiPopunjenost(int idSpremnika)
        {
            double[] popunjenost = new double[2];
            string upit1 = "SELECT zapremnina FROM spremnik WHERE id_spremnik=" + idSpremnika + "";
            string upit2 = "SELECT SUM(zauzeće) FROM stavka WHERE spremnik_id=" + idSpremnika + "";
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command1 = new SqlCommand(upit1, connection);
                SqlCommand command2 = new SqlCommand(upit2, connection);
                try
                {
                    popunjenost[0] = (double)command1.ExecuteScalar(); //sprema zapremninu spremnika na poziciju 0
                    popunjenost[1] = (double)command2.ExecuteScalar(); //sprema ukupno zauzeće spremnika na poziciju 1
                }
                catch (Exception ex)
                {
                }
                connection.Close();
            }
            return popunjenost;
        }

       
    }
}
