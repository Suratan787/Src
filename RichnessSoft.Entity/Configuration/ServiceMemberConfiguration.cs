using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichnessSoft.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichnessSoft.Entity.Configuration
{
    internal class ServiceMemberConfiguration : IEntityTypeConfiguration<ServiceMember>
    {
        public void Configure(EntityTypeBuilder<ServiceMember> builder)
        {
            builder.ToTable("servicemember");
            builder.HasKey(m => m.id);
            builder.Property(e => e.stdprice).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice1).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice2).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice3).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice4).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice5).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice6).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice7).HasColumnType("decimal(18,2)");
            builder.Property(e => e.memberprice8).HasColumnType("decimal(18,2)");
            builder.Property(e => e.seq).HasColumnType("varchar(4)");
            builder.Property(e => e.discount).HasColumnType("varchar(255)");

            builder.HasOne(p => p.Company).WithMany(p => p.ServiceMembers).HasForeignKey(e => e.companyid).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Customer).WithMany(p => p.ServiceMembers).HasForeignKey(e => e.customerid).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Service).WithMany(p => p.ServiceMembers).HasForeignKey(e => e.serviceid).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
