using System;
using System.Collections.Generic;
using MSPR_bloc_4_customers.Models;
using Microsoft.EntityFrameworkCore;

namespace MSPR_bloc_4_customers.Data;

public partial class ClientDBContext : DbContext
{
    public ClientDBContext()
    {
    }

    public ClientDBContext(DbContextOptions<ClientDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            optionsBuilder.UseSqlServer("Data Source=sqlserver;Initial Catalog=MSPR4_CLIENT;User ID=sa;Password=1StrongPwd!!;Encrypt=False");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Clients__6EC2B6C0C48EC7D6");

            entity.Property(e => e.IdClient)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_client");
            entity.Property(e => e.CodePostal)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code_postal");
            entity.Property(e => e.Entreprise)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("entreprise");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.Ville)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ville");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
