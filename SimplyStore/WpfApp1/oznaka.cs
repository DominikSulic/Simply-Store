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
    
    public partial class oznaka
    {
        public oznaka()
        {
            this.spremnik = new HashSet<spremnik>();
            this.stavka = new HashSet<stavka>();
        }
    
        public int id_oznaka { get; set; }
        public string naziv { get; set; }
        public string kvarljivost { get; set; }
        public string aktivna { get; set; }
    
        public virtual ICollection<spremnik> spremnik { get; set; }
        public virtual ICollection<stavka> stavka { get; set; }
    }
}
