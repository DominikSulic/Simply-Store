using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazStatistika
    {
        public string radnja { get; set; }
        public DateTime datum { get; set; }
        public double? kolicina { get; set; }
        public string nazivStavke { get; set; }
        public string nazivKorisnika { get; set; }
        public int idStavke { get; set; }
        public string oznake { get; set; }

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
                                 radnja = d.radnja,
                                 datum = d.datum,
                                 kolicina = d.kolicina,
                                 nazivStavke = s.naziv,
                                 nazivKorisnika = k.korisnicko_ime,
                                 idStavke = s.id_stavka
                             }).ToList();

                sveStatistike = query;
            }

            foreach (var item in sveStatistike)
            {
                string upit = "SELECT oznaka.naziv FROM stavka JOIN stavka_oznaka ON stavka.id_stavka = stavka_oznaka.stavka_id JOIN oznaka ON " +
                "stavka_oznaka.oznaka_id = oznaka.id_oznaka WHERE stavka.id_stavka = " + item.idStavke;
                string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(upit, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        item.oznake = item.oznake + " " + reader.GetString(0);
                    }
                    reader.Close();
                }
            }
            return sveStatistike;
        }

        public static List<PrikazStatistika> dohvatiStatistike(string tekst)
        {
            List<PrikazStatistika> sveStatistike = new List<PrikazStatistika>();

            using (var db = new SSDB())
            {
                var query = (from d in db.dnevnik
                             join s in db.stavka on d.stavka_id equals s.id_stavka
                             join k in db.korisnik on d.korisnik_id equals k.id_korisnik
                             where s.naziv.Contains(tekst)
                             select new PrikazStatistika
                             {
                                 radnja = d.radnja,
                                 datum = d.datum,
                                 kolicina = d.kolicina,
                                 nazivStavke = s.naziv,
                                 nazivKorisnika = k.korisnicko_ime,
                                 idStavke = s.id_stavka
                             }).ToList();

                sveStatistike = query;
            }

            foreach (var item in sveStatistike)
            {
                string upit = "SELECT oznaka.naziv FROM stavka JOIN stavka_oznaka ON stavka.id_stavka = stavka_oznaka.stavka_id JOIN oznaka ON " +
                "stavka_oznaka.oznaka_id = oznaka.id_oznaka WHERE stavka.id_stavka = " + item.idStavke;
                string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(upit, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        item.oznake = item.oznake + " " + reader.GetString(0);
                    }
                    reader.Close();
                }
            }
            return sveStatistike;
        }
    }
}