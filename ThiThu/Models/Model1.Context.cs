﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThiThu.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DatabaseBloggingContextEntities : DbContext
    {
        public DatabaseBloggingContextEntities()
            : base("name=DatabaseBloggingContextEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BlogSet> BlogSets { get; set; }
        public virtual DbSet<PostSet> PostSets { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
