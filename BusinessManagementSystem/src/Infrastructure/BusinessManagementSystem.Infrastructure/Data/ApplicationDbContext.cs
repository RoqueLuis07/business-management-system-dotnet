using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Domain.Entities;

namespace BusinessManagementSystem.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core DbContext para A Y R Servicio T�cnico
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Entidades principales
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Equipment> Equipment { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<WorkOrder> WorkOrders { get; set; } = null!;
        public DbSet<PartCatalogItem> PartCatalogItems { get; set; } = null!;
        public DbSet<WarrantyClaim> WarrantyClaims { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // CONFIGURACI�N DE CLIENT
            // ============================================
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(255);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.Observations)
                    .HasMaxLength(1000);

                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW()");

                // �ndices
                entity.HasIndex(e => e.Phone)
                    .IsUnique()
                    .HasName("idx_client_phone");

                entity.HasIndex(e => e.Email)
                    .HasName("idx_client_email");

                entity.HasIndex(e => e.FullName)
                    .HasName("idx_client_fullname");
            });

            // ============================================
            // CONFIGURACI�N DE EQUIPMENT
            // ============================================
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW()");

                // �ndices
                entity.HasIndex(e => e.Type)
                    .HasName("idx_equipment_type");

                entity.HasIndex(e => e.SerialNumber)
                    .HasName("idx_equipment_serial");
            });

            // ============================================
            // CONFIGURACI�N DE USER
            // ============================================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(500);

                entity.Property(e => e.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW()");

                // �ndices
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasName("idx_user_email");

                entity.HasIndex(e => e.Role)
                    .HasName("idx_user_role");

                entity.HasIndex(e => e.IsActive)
                    .HasName("idx_user_active");
            });

            // ============================================
            // CONFIGURACI�N DE WORK ORDER
            // ============================================
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.WorkOrderNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestedWorkDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.QuoteRejectionReason)
                    .HasMaxLength(500);

                entity.Property(e => e.CancellationReason)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.DeliveredAtLocal)
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.QuoteRejectedAtUtc)
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.CancelledAtUtc)
                    .HasColumnType("timestamp with time zone");

                // Relaciones
                // map foreign key properties explicitly to avoid shadow properties
                entity.Property(e => e.ClientId)
                    .IsRequired();

                entity.Property(e => e.EquipmentId)
                    .IsRequired();

                entity.HasOne(e => e.Client)
                    .WithMany()
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_workorder_client");

                entity.HasOne(e => e.Equipment)
                    .WithMany()
                    .HasForeignKey(e => e.EquipmentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_workorder_equipment");

                // �ndices
                entity.HasIndex(e => e.WorkOrderNumber)
                    .IsUnique()
                    .HasName("idx_workorder_number");

                entity.HasIndex(e => e.Status)
                    .HasName("idx_workorder_status");

                entity.HasIndex(e => e.CreatedAtUtc)
                    .HasName("idx_workorder_createdat");

                entity.HasIndex("ClientId")
                    .HasName("idx_workorder_clientid");

                entity.HasIndex(e => e.AssignedMechanicUserId)
                    .HasName("idx_workorder_mechanicid");
            });

            // ============================================
            // CONFIGURACI�N DE PART CATALOG ITEM
            // ============================================
            modelBuilder.Entity<PartCatalogItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.DefaultUnitPrice)
                    .HasPrecision(15, 2);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW()");

                // �ndices
                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasName("idx_partcatalog_name");

                entity.HasIndex(e => e.IsActive)
                    .HasName("idx_partcatalog_active");
            });

            // ============================================
            // CONFIGURACI�N DE WARRANTY CLAIM
            // ============================================
            modelBuilder.Entity<WarrantyClaim>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("NOW()");

                // Relaciones
                entity.Property<Guid>("OriginalWorkOrderId")
                    .IsRequired();

                entity.Property<Guid>("ClaimWorkOrderId")
                    .IsRequired();

                entity.HasOne<WorkOrder>()
                    .WithMany(w => w.WarrantyClaims)
                    .HasForeignKey("OriginalWorkOrderId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_warrantyclaim_original");

                entity.HasOne<WorkOrder>()
                    .WithMany()
                    .HasForeignKey("ClaimWorkOrderId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_warrantyclaim_claim");

                // �ndices
                entity.HasIndex("OriginalWorkOrderId")
                    .HasName("idx_warrantyclaim_original");

                entity.HasIndex("ClaimWorkOrderId")
                    .HasName("idx_warrantyclaim_claim");
            });

            // ============================================
            // CONFIGURACI�N DE WORK ORDER ACCESSORIES (OWNED TYPE)
            // ============================================
            modelBuilder.Entity<WorkOrder>()
                .OwnsMany(w => w.Accessories, a =>
                {
                    a.ToTable("WorkOrderAccessories");
                    a.WithOwner().HasForeignKey("WorkOrderId");
                    a.HasKey("Id");
                    a.Property(x => x.Name).HasMaxLength(255).IsRequired();
                    a.Property(x => x.Condition).HasMaxLength(255);
                    a.HasIndex("WorkOrderId").HasDatabaseName("idx_accessory_workorderid");
                });

            // ============================================
            // CONFIGURACI�N DE WORK ORDER PARTS (OWNED TYPE)
            // ============================================
            modelBuilder.Entity<WorkOrder>()
                .OwnsMany(w => w.Parts, p =>
                {
                    p.ToTable("WorkOrderParts");
                    p.WithOwner().HasForeignKey("WorkOrderId");
                    p.HasKey("Id");
                    p.Property(x => x.PartName).HasMaxLength(255).IsRequired();
                    p.Property(x => x.UnitPrice).HasPrecision(15, 2);
                    p.HasIndex("WorkOrderId").HasDatabaseName("idx_part_workorderid");
                });

            // ============================================
            // CONFIGURACI�N DE WORK ORDER DIAGNOSIS (OWNED TYPE)
            // ============================================
            modelBuilder.Entity<WorkOrder>()
                .OwnsOne(w => w.Diagnosis, d =>
                {
                    d.ToTable("WorkOrderDiagnosis");
                    d.WithOwner().HasForeignKey("WorkOrderId");
                    d.Property(x => x.Findings).HasMaxLength(1000).IsRequired();
                    d.Property(x => x.RecommendedWork).HasMaxLength(1000).IsRequired();
                    d.Property(x => x.Notes).HasMaxLength(1000);
                    d.Property(x => x.CreatedAtUtc).HasColumnType("timestamp with time zone");
                });

            // ============================================
            // CONFIGURACI�N DE WORK ORDER QUOTE (OWNED TYPE)
            // ============================================
            modelBuilder.Entity<WorkOrder>()
                .OwnsOne(w => w.Quote, q =>
                {
                    q.ToTable("WorkOrderQuote");
                    q.WithOwner().HasForeignKey("WorkOrderId");
                    q.Property(x => x.LaborCost).HasPrecision(15, 2);
                    q.Property(x => x.PartsTotal).HasPrecision(15, 2);
                    q.Property(x => x.Notes).HasMaxLength(1000);
                    q.Property(x => x.CreatedAtUtc).HasColumnType("timestamp with time zone");
                });

            // ============================================
            // CONFIGURACI�N DE WORK ORDER SERVICE REPORT (OWNED TYPE)
            // ============================================
            modelBuilder.Entity<WorkOrder>()
                .OwnsOne(w => w.ServiceReport, sr =>
                {
                    sr.ToTable("WorkOrderServiceReport");
                    sr.WithOwner().HasForeignKey("WorkOrderId");
                    sr.Property(x => x.WorkPerformed).HasMaxLength(2000).IsRequired();
                    sr.Property(x => x.Recommendations).HasMaxLength(1000);
                    sr.Property(x => x.Notes).HasMaxLength(1000);
                    sr.Property(x => x.CreatedAtUtc).HasColumnType("timestamp with time zone");
                });
        }
    }
}
