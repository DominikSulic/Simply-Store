//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class spremnik
    {
        public spremnik()
        {
            this.stavka = new HashSet<stavka>();
        }
    
        public int id_spremnik { get; set; }
        public string naziv_spremnika { get; set; }
        public System.DateTime datum_kreiranja { get; set; }
        public double zapremnina { get; set; }
        public string opis { get; set; }
        public Nullable<int> prostorija_id { get; set; }
        public Nullable<int> tip_id { get; set; }
    
        public virtual prostorija prostorija { get; set; }
        public virtual tip_spremnika tip_spremnika { get; set; }
        public virtual ICollection<stavka> stavka { get; set; }
    }
}