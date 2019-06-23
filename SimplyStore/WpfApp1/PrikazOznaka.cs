using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazOznaka 
    {

        public int id_oznaka { get; set; }
        public string naziv { get; set; }
        public byte kvarljivost { get; set; }
 

        public static List<PrikazOznaka> dohvatiOznake() {

            List<PrikazOznaka> sveOznake = new List<PrikazOznaka>();
            using (var db = new SSDB())
            {

                var query = (from oznaka in db.oznaka

                             select new PrikazOznaka
                             {

                                 id_oznaka = oznaka.id_oznaka,
                                 naziv = oznaka.naziv,
                                 kvarljivost = oznaka.kvarljivost

                             }).ToList();

                sveOznake = query;

            }
            return sveOznake;
        }


        public override string ToString()
        {
            return naziv;
        }


    }
}
