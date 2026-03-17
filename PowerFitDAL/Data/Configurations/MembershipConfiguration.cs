using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerFitDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Data.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasKey(X => new { X.MemberId, X.PlanId, X.StartDate });

            builder.Ignore(X => X.Id);
        }
    }
}
