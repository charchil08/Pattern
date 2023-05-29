using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pattern.Entity.DataModels;

namespace Pattern.Entity.Data
{
    public partial class PatternContext : DbContext
    {
        public PatternContext()
        {
        }

        public PatternContext(DbContextOptions<PatternContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Skill> Skills { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=PCI141\\SQL2017;Initial Catalog=Pattern;User ID=sa;Password=tatva123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("skill");

                entity.HasIndex(e => e.Name, "UQ__skill__72E12F1BB4334596")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
