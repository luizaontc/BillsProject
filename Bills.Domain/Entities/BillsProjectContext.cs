using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bills.Domain.Entities;

public partial class BillsProjectContext : DbContext
{
    public BillsProjectContext()
    {
    }

    public BillsProjectContext(DbContextOptions<BillsProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<InstallmentBill> InstallmentBills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=BillsProject;User Id=login;Password=login;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.BillsName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("billsName");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.Installments).HasColumnName("installments");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Bills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Bills_UserId");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("Currency");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameBr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name_br");
            entity.Property(e => e.NameEn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name_en");
            entity.Property(e => e.Symbol)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<InstallmentBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Installmentbills_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.BillsId).HasColumnName("billsId");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.InstallmentNumber).HasColumnName("installment_number");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Bills).WithMany(p => p.InstallmentBills)
                .HasForeignKey(d => d.BillsId)
                .HasConstraintName("FK_Installmentbills_BillsId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Currency).HasColumnName("currency");
            entity.Property(e => e.Document)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("document");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.PasswordSalt).HasMaxLength(128);
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.CurrencyNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Currency)
                .HasConstraintName("FK_Users_Currency");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
