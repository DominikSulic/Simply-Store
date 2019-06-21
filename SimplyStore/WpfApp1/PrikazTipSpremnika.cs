using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class PrikazTipSpremnika
    {
        public int idTipSpremnika { get; set; }
        public string nazivTipSpremnika { get; set; }

        public static List<PrikazTipSpremnika> dohvatiTipSpremnika()
        {
            List<PrikazTipSpremnika> sviTipoviSpremnika =new List<PrikazTipSpremnika>();
            using (var db = new SSDB())
            {
                var query = (from tip in db.tip_spremnika

                             select new PrikazTipSpremnika
                             {
                                 idTipSpremnika = tip.id_tip,
                                 nazivTipSpremnika= tip.naziv
                             }).ToList();
                sviTipoviSpremnika = query;
            }
             return sviTipoviSpremnika;
        }

        public override string ToString()
        {
            return nazivTipSpremnika;
        }
    }

    
}
