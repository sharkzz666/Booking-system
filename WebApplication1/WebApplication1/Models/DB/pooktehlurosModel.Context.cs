﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class pooktehlurosEntities : DbContext
    {
        public pooktehlurosEntities()
            : base("name=pooktehlurosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Ref_ProductType> Ref_ProductType { get; set; }
        public virtual DbSet<Ref_Role> Ref_Role { get; set; }
        public virtual DbSet<Ref_Status> Ref_Status { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}