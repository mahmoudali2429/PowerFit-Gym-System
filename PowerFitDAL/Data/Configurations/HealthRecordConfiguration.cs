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
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                   .HasKey(X => X.Id); // Not Needed [By Convention]

            builder.HasOne<Member>()
                   .WithOne(X => X.HealthRecord)
                   .HasForeignKey<HealthRecord>(X => X.Id);
            builder.Ignore(X => X.CreatedAt);
            builder.Ignore(X => X.UpdatedAt);
        }
    }
}
