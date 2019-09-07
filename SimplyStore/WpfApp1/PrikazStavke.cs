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

                             }).OrderBy(s => s.datumRoka).ToList();
                sveStavke = query;
            }

            return sveStavke;
        }

        public static List<PrikazStavke> dohvatiStavkeEnter(string tekst)
        {
            List<PrikazStavke> stavke = new List<PrikazStavke>();
            using (var db = new SSDB())
            {
                var query = (from s in db.stavka
                             join d in db.spremnik on s.spremnik_id equals d.id_spremnik
                             join p in db.prostorija on d.prostorija_id equals p.id_prostorija
                             where s.naziv.ToLower().Contains(tekst.ToLower()) && s.zauzeće > 0
                             select new PrikazStavke
                             {
                                 idStavke = s.id_stavka,
                                 nazivStavke = s.naziv,
                                 datumKreiranja = s.datum_kreiranja,
                                 datumRoka = s.datum_roka,
                                 zauzece = s.zauzeće,
                                 nazivSpremnika = d.naziv_spremnika,
                                 nazivProstorije = p.naziv_prostorije
                             }).OrderBy(s => s.datumRoka).ToList();
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
                             }).OrderBy(s => s.datumRoka).ToList();
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

            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            string upit = "UPDATE spremnik SET zauzeće=zauzeće+" + zauzima + " WHERE id_spremnik=" + idSpremnika;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit, connection);
                command.ExecuteNonQuery();
                connection.Close();
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

            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            string upit = "UPDATE spremnik SET zauzeće=zauzeće+" + novaKolicina + " WHERE id_spremnik=" + idSpremnika;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit, connection);
                command.ExecuteNonQuery();
                connection.Close();
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
            double zauzeceStavke;
            int? idSpremnika;
            using (var db = new SSDB())
            {
                var query = (from stavka in db.stavka where stavka.id_stavka == idStavke select stavka).First();
                db.stavka.Attach(query);
                noviDnevnik.kolicina = query.zauzeće;
                zauzeceStavke = query.zauzeće;
                idSpremnika = query.spremnik_id;
                query.zauzeće = 0;
                db.dnevnik.Add(noviDnevnik);
                db.SaveChanges();
            }
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            string upit = "UPDATE spremnik SET zauzeće=zauzeće-" + zauzeceStavke + " WHERE id_spremnik=" + idSpremnika;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit, connection);
                command.ExecuteNonQuery();
                connection.Close();
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

                             }).OrderBy(s => s.datumRoka).ToList();
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

                             }).OrderBy(s => s.datumRoka).ToList();
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

        public static bool promjeniKolicinuStavke(int promjeniKolicinu, int stavkaID)
        {
            int? idSpremnika;
            double promjena;
            using (var db = new SSDB())
            {
                var query = (from s in db.stavka where s.id_stavka == stavkaID select s).First();
                idSpremnika = query.spremnik_id;
                var query2 = (from spre in db.spremnik where spre.id_spremnik == idSpremnika select spre).First();
                if (query2.zauzeće + promjeniKolicinu > query2.zapremnina)
                {
                    return false;
                }
                else
                {
                    if (query.zauzeće + promjeniKolicinu < 0)
                    {
                        promjena = ((query.zauzeće + promjeniKolicinu) * -1) + promjeniKolicinu;
                        query.zauzeće += promjena;
                    }
                    else
                    {
                        query.zauzeće += promjeniKolicinu;
                        promjena = promjeniKolicinu;
                    }
                    query2.zauzeće += promjena;
                    
                }
               
                db.SaveChanges();
                return true;
            }
           
        }

        public static List<PrikazSpremnici> dohvatiDopusteneSpremnikeOznake(List<PrikazOznaka> odabraneOznake)
        {
            List<PrikazSpremnici> dopusteniSpremnici = new List<PrikazSpremnici>();
            string upit1 = "SELECT spremnik_id,COUNT(oznaka_id),zauzeće,zapremnina,naziv_spremnika FROM spremnik JOIN spremnik_oznaka ON id_spremnik = spremnik_id JOIN oznaka ON oznaka_id = id_oznaka WHERE zapremnina> 0 AND (";
            foreach(PrikazOznaka item in odabraneOznake)
            {
                upit1 = upit1 + "oznaka_id=" + item.id_oznaka + " OR ";
            }
            upit1 = upit1 + " 1!=1) GROUP BY spremnik_id,zauzeće,zapremnina,naziv_spremnika HAVING count(oznaka_id)=" + odabraneOznake.Count();
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit1, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        dopusteniSpremnici.Add(new PrikazSpremnici
                        {
                            idSpremnika = reader.GetInt32(0),
                            nazivSpremnika=reader.GetString(4),
                            zapremnina=reader.GetDouble(3),
                            zauzece=reader.GetDouble(2)
                        });
                    }   
                }
                reader.Close();
                connection.Close();
            }
            return dopusteniSpremnici;
        }

        public static List<PrikazSpremnici> dohvatiDopusteneSpremnikeKolicine(double unesenoZauzece, List<PrikazSpremnici> filtriraniSpremnici)
        {
            List<PrikazSpremnici> filtriraniSpremnici2 = new List<PrikazSpremnici>();
            foreach(PrikazSpremnici item in filtriraniSpremnici)
            {
                if (unesenoZauzece + item.zauzece <= item.zapremnina)
                {
                    filtriraniSpremnici2.Add(item);
                }
            }
            return filtriraniSpremnici2;
        }

        

    }
}