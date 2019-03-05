using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace coliks.Models
{
    public partial class ColiksContext : DbContext
    {
        public ColiksContext()
        {
        }

        public ColiksContext(DbContextOptions<ColiksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Contracts> Contracts { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Durations> Durations { get; set; }
        public virtual DbSet<Geartypes> Geartypes { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Purchases> Purchases { get; set; }
        public virtual DbSet<Renteditems> Renteditems { get; set; }
        public virtual DbSet<Rentprices> Rentprices { get; set; }
        public virtual DbSet<Staffs> Staffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=ASP;User=SA;Password=Passw0rd!;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.ToTable("cities");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contracts>(entity =>
            {
                entity.ToTable("contracts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Effectivereturn)
                    .HasColumnName("effectivereturn")
                    .HasColumnType("datetime");

                entity.Property(e => e.Goget).HasColumnName("goget");

                entity.Property(e => e.HelpStaffId).HasColumnName("help_staff_id");

                entity.Property(e => e.Insurance).HasColumnName("insurance");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("text");

                entity.Property(e => e.Paidon)
                    .HasColumnName("paidon")
                    .HasColumnType("datetime");

                entity.Property(e => e.Plannedreturn)
                    .HasColumnName("plannedreturn")
                    .HasColumnType("datetime");

                entity.Property(e => e.Takenon)
                    .HasColumnName("takenon")
                    .HasColumnType("datetime");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.TuneStaffId).HasColumnName("tune_staff_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contract_customer");

                entity.HasOne(d => d.HelpStaff)
                    .WithMany(p => p.ContractsHelpStaff)
                    .HasForeignKey(d => d.HelpStaffId)
                    .HasConstraintName("contract_help");

                entity.HasOne(d => d.TuneStaff)
                    .WithMany(p => p.ContractsTuneStaff)
                    .HasForeignKey(d => d.TuneStaffId)
                    .HasConstraintName("contract_tune");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.ToTable("customers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("customer_city");
            });

            modelBuilder.Entity<Durations>(entity =>
            {
                entity.ToTable("durations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasColumnName("details")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Geartypes>(entity =>
            {
                entity.ToTable("geartypes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.ToTable("items");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Brand)
                    .HasColumnName("brand")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Itemnb)
                    .IsRequired()
                    .HasColumnName("itemnb")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Returned)
                    .HasColumnName("returned")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Serialnumber)
                    .HasColumnName("serialnumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("item_category");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("logs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("text");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Purchases>(entity =>
            {
                entity.ToTable("purchases");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("purchase_customer");
            });

            modelBuilder.Entity<Renteditems>(entity =>
            {
                entity.ToTable("renteditems");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasDefaultValueSql("('1')");

                entity.Property(e => e.ContractId).HasColumnName("contract_id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DurationId).HasColumnName("duration_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Linenb)
                    .HasColumnName("linenb")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Partialreturn).HasColumnName("partialreturn");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasDefaultValueSql("('0')");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Renteditems)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("renteditem_category");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Renteditems)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("renteditem_contract");

                entity.HasOne(d => d.Duration)
                    .WithMany(p => p.Renteditems)
                    .HasForeignKey(d => d.DurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("renteditem_duration");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Renteditems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("renteditem_item");
            });

            modelBuilder.Entity<Rentprices>(entity =>
            {
                entity.ToTable("rentprices");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.DurationId).HasColumnName("duration_id");

                entity.Property(e => e.GeartypeId).HasColumnName("geartype_id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasDefaultValueSql("('0')");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Rentprices)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rentprice_category");

                entity.HasOne(d => d.Duration)
                    .WithMany(p => p.Rentprices)
                    .HasForeignKey(d => d.DurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rentprice_duration");

                entity.HasOne(d => d.Geartype)
                    .WithMany(p => p.Rentprices)
                    .HasForeignKey(d => d.GeartypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rentprice_type");
            });

            modelBuilder.Entity<Staffs>(entity =>
            {
                entity.ToTable("staffs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
