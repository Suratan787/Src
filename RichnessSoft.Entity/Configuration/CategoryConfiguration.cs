using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichnessSoft.Entity.Model;

namespace RichnessSoft.Entity.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");
            builder.HasKey(m => m.id);
            builder.Property(e => e.code).HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.name1).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.name2).HasColumnType("varchar(150)");
            builder.Property(e => e.active).HasColumnType("varchar(1)").HasDefaultValue("Y").HasComment("active");

            builder.HasOne(p => p.Company).WithMany(p => p.Categorys).HasForeignKey(e => e.companyid).OnDelete(DeleteBehavior.Restrict);
        }
    }
}