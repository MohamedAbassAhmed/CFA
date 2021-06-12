using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CFAEntities;
using ViewEntities;

namespace CFA.Areas.Common
{
    public class CFADBContext : IdentityDbContext<IdentityUser>
    {
        public CFADBContext(DbContextOptions<CFADBContext> options)
             : base(options)
        {
        }
        public CFADBContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FishBoat>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_FishBoat");
            });
            modelBuilder.Entity<LoadDelivery>(entity =>
                        {
                            entity.HasKey(e => e.Id)
                            .IsClustered(false)
                            .HasName("PK_LoadDelivery");
                        });
            modelBuilder.Entity<FishType>(entity =>
                        {
                            entity.HasKey(e => e.Id)
                            .IsClustered(false)
                            .HasName("PK_FishType");
                        });
            modelBuilder.Entity<CFAAgent>(entity =>
                        {
                            entity.HasKey(e => e.Id)
                            .IsClustered(false)
                            .HasName("PK_CFAAgent");
                        });
            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_PurchaseOrder");
            });
            modelBuilder.Entity<PurchaseOrderStatus>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_PurchaseOrderStatus");
            });
            modelBuilder.Entity<PurchaseOrderDetails>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_PurchaseOrderDetails");
            });
            modelBuilder.Entity<ExceptionLog>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_ExceptionLog");
            });
            modelBuilder.Entity<DatabaseLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                .IsClustered(false)
                .HasName("PK_DatabaseLog");
            });
            modelBuilder.Entity<LoadDeliveryStock>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_LoadDeliveryStock");
            });
            modelBuilder.Entity<PurchaseOrderSupply>(entity =>
            {
                entity.HasKey(e => e.Id)
                .IsClustered(false)
                .HasName("PK_PurchaseOrderSupply");
            });

            #region Views
            modelBuilder.Entity<ViewLoadDelivery>().HasNoKey().ToView("ViewLoadDeliveries");
            modelBuilder.Entity<ViewPurchaseOrder>().HasNoKey().ToView("ViewPurchaseOrders");
            modelBuilder.Entity<ViewLoadDeliveryStock>().HasNoKey().ToView("ViewLoadDeliveryStocks");
            modelBuilder.Entity<ViewFishTypeStock>().HasNoKey().ToView("ViewFishTypeStocks");
            #endregion

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Constants.ConnectionStringTest);
        }
        public virtual DbSet<FishType> FishTypes { get; set; }
        public virtual DbSet<LoadDelivery> LoadDeliveries { get; set; }
        public virtual DbSet<FishBoat> FishBoats { get; set; }
        public virtual DbSet<CFAAgent> CFAAgents { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }
        public virtual DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public virtual DbSet<DatabaseLog> DatabaseLogs { get; set; }
        public virtual DbSet<LoadDeliveryStock> LoadDeliveryStocks { get; set; }
        public virtual DbSet<PurchaseOrderSupply> PurchaseOrderSupplies { get; set; }

        public virtual DbSet<ViewLoadDelivery> ViewLoadDeliveries { get; set; }
        public virtual DbSet<ViewLoadDeliveryStock> ViewLoadDeliveryStocks { get; set; }
        public virtual DbSet<ViewPurchaseOrder> ViewPurchaseOrders { get; set; }
        public virtual DbSet<ViewFishTypeStock> ViewFishTypeStocks { get; set; }
        

    }
}
