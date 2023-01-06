using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AerolineasU4.Models
{
    public partial class sistem21_aerolineau4Context : DbContext
    {
        public sistem21_aerolineau4Context()
        {
        }

        public sistem21_aerolineau4Context(DbContextOptions<sistem21_aerolineau4Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Aerolineau4> Aerolineau4 { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Aerolineau4>(entity =>
            {
                entity.HasKey(e => e.IdAerolineaU4)
                    .HasName("PRIMARY");

                entity.ToTable("aerolineau4");

                entity.Property(e => e.IdAerolineaU4)
                    .HasColumnType("int(11)")
                    .HasColumnName("idAerolineaU4");

                entity.Property(e => e.Destino).HasMaxLength(45);

                entity.Property(e => e.Estado).HasMaxLength(45);

                entity.Property(e => e.Hora).HasColumnType("datetime");

                entity.Property(e => e.Puerta).HasMaxLength(45);

                entity.Property(e => e.Vuelo).HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
