using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Energy2.AutoGen
{
    public partial class SettlementCenter : DbContext
    {
        public SettlementCenter()
        {
        }

        public SettlementCenter(DbContextOptions<SettlementCenter> options)
            : base(options)
        {
        }

        public virtual DbSet<Tariff> Tariffs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlite("Filename=SettlementCenter.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tariff>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

