﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SSDB : DbContext
    {
        public SSDB()
            : base("name=SSDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<dnevnik> dnevnik { get; set; }
        public DbSet<korisnik> korisnik { get; set; }
        public DbSet<oznaka> oznaka { get; set; }
        public DbSet<prostorija> prostorija { get; set; }
        public DbSet<spremnik> spremnik { get; set; }
        public DbSet<stavka> stavka { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
