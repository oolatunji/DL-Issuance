﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LicenseLibrary.ModelLibrary.EntityFrameworkLib
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LicenseDBEntities : DbContext
    {
        public LicenseDBEntities()
            : base("name=LicenseDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CardAccountRequest> CardAccountRequests { get; set; }
        public virtual DbSet<Enrolment> Enrolments { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<PregeneratedCard> PregeneratedCards { get; set; }
        public virtual DbSet<RoleFunction> RoleFunctions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SmartCard> SmartCards { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
