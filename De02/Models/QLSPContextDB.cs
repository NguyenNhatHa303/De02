using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace De02.Models
{
    public partial class QLSPContextDB : DbContext
    {
        public QLSPContextDB()
            : base("name=QLSPContextDB")
        {
        }

        public virtual DbSet<LoaiSP> LoaiSPs { get; set; }
        public virtual DbSet<Sanpham> Sanphams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiSP>()
                .Property(e => e.MaLoai)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LoaiSP>()
                .HasMany(e => e.Sanphams)
                .WithOptional(e => e.LoaiSP)
                .HasForeignKey(e => e.MaLoai);

            modelBuilder.Entity<LoaiSP>()
                .HasMany(e => e.Sanphams1)
                .WithOptional(e => e.LoaiSP1)
                .HasForeignKey(e => e.MaLoai);

            modelBuilder.Entity<Sanpham>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Sanpham>()
                .Property(e => e.MaLoai)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
