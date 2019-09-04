using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazOznaka 
    {

        public int id_oznaka { get; set; }
        public string naziv { get; set; }
        public string kvarljivost { get; set; }
        public string aktivna { get; set; }
 

        public static List<PrikazOznaka> dohvatiOznake() {

            List<PrikazOznaka> sveOznake = new List<PrikazOznaka>();
            using (var db = new SSDB())
            {

                var query = (from oznaka in db.oznaka
                             where oznaka.aktivna.Equals("da")
                             select new PrikazOznaka
                             {

                                 id_oznaka = oznaka.id_oznaka,
                                 naziv = oznaka.naziv,
                                 kvarljivost = oznaka.kvarljivost,
                                 aktivna=oznaka.aktivna

                             }).ToList();

                sveOznake = query;

            }
            return sveOznake;
        }

        public static List<PrikazOznaka> dohvatiSveOznake()
        {

            List<PrikazOznaka> sveOznake = new List<PrikazOznaka>();
            using (var db = new SSDB())
            {

                var query = (from oznaka in db.oznaka
                             select new PrikazOznaka
                             {

                                 id_oznaka = oznaka.id_oznaka,
                                 naziv = oznaka.naziv,
                                 kvarljivost = oznaka.kvarljivost,
                                 aktivna = oznaka.aktivna

                             }).ToList();

                sveOznake = query;

            }
            return sveOznake;
        }

        public override string ToString()
        {
            return naziv;
        }

        public static List<PrikazOznaka> dohvatiOznakeSpremnika(int idSpremnika)
        {
            List<PrikazOznaka> oznake = new List<PrikazOznaka>();
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            string upit = "SELECT oznaka.* FROM oznaka,spremnik_oznaka WHERE oznaka.id_oznaka=spremnik_oznaka.oznaka_id AND spremnik_oznaka.spremnik_id=" + idSpremnika;
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

        public static List<PrikazOznaka> dohvatiOznakeNePripadajuSpremniku(int idSpremnika)//dohvaca oznake koje ne pripadaju odabranom spremniku
        {
            List<PrikazOznaka> oznake = new List<PrikazOznaka>();
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            string upit = "SELECT oznaka.* FROM oznaka WHERE oznaka.aktivna='da' EXCEPT SELECT oznaka.* FROM oznaka,spremnik_oznaka WHERE oznaka.id_oznaka=spremnik_oznaka.oznaka_id AND spremnik_oznaka.spremnik_id=" + idSpremnika;
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

        public static int kreirajOznaku(string nazivOznake,string kvarljivost)
        {
            int rezultat;
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            string upit = "INSERT INTO oznaka(naziv, kvarljivost) VALUES('"+nazivOznake+"','"+kvarljivost+"')";
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

    }
}
