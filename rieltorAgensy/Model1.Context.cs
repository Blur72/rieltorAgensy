﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace rieltorAgensy
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RieltorskoeAgentsvoEntities2 : DbContext
    {
        public RieltorskoeAgentsvoEntities2()
            : base("name=RieltorskoeAgentsvoEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Deals> Deals { get; set; }
        public virtual DbSet<DealStatuses> DealStatuses { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Propertiezzz> Propertiezzz { get; set; }
        public virtual DbSet<PropertyFeatures> PropertyFeatures { get; set; }
        public virtual DbSet<PropertyHistory> PropertyHistory { get; set; }
        public virtual DbSet<PropertyImages> PropertyImages { get; set; }
        public virtual DbSet<PropertyStatuses> PropertyStatuses { get; set; }
        public virtual DbSet<PropertyTypes> PropertyTypes { get; set; }
        public virtual DbSet<Realtors> Realtors { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}