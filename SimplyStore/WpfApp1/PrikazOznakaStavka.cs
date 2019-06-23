using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazOznakaStavka
    {

        public int idStavka { get; set; }
        public int idOznaka { get; set; }
        public string nazivOznake { get; set; }

        public static List<PrikazOznakaStavka> dohvatiOznakeStavke(int idStavke)
        {
            List<PrikazOznakaStavka> sveOznakeZaStavku = new List<PrikazOznakaStavka>();
            using (var db = new SSDB())
            {
                var querry = (from oznaka in db.oznaka
                              where oznaka.stavka.Any(s => s.id_stavka == idStavke)
                              select new PrikazOznakaStavka
                              {
                                  idOznaka = oznaka.id_oznaka,
                                  idStavka = idStavke,
                                  nazivOznake = oznaka.naziv
                              }).ToList();
                sveOznakeZaStavku = querry;
            }


            return sveOznakeZaStavku;
        }

        public static int dodajStavkuOznaku(int idStavke, int idOznake)
        {

            string upit = "INSERT INTO stavka_oznaka VALUES(" + idStavke + "," + idOznake + ")";
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(upit, connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return 0;
                }
                connection.Close();
            }
            return 1;
        }

        public static void obrisiStavkuOznaku(int idStavke, int idOznake)
        {
            string upit = "DELETE FROM stavka_oznaka WHERE stavka_id=" + idStavke + "AND oznaka_id=" + idOznake + "";
            string connectionString = @"Data Source=31.147.204.119\PISERVER,1433; Initial Catalog=19023_DB; User=19023_User; Password='z#X1iD;M'";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(upit, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public override string ToString()
        {
            return nazivOznake;
        }

    }
}
