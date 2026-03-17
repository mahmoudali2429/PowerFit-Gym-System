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
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity BETWEEN 1 AND 25");
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(X => X.Category)
                   .WithMany(X => X.Sessions)
                   .HasForeignKey(X => X.CategoryId);

            builder.HasOne(X => X.Trainer)
                   .WithMany(X => X.TrainerSessions)
                   .HasForeignKey(X => X.TrainerId);
        } 
    }
}
