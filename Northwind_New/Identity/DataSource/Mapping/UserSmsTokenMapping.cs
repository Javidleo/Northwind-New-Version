using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DataSource.Mapping
{
    public class UserSmsTokenMapping : IEntityTypeConfiguration<UserSmsToken>
    {
        public void Configure(EntityTypeBuilder<UserSmsToken> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                  .ValueGeneratedOnAdd();
            builder.HasOne(s => s.User)
                  .WithMany(u => u.UserSmsTokens)
                  .HasForeignKey(s => s.UserId);
        }
    }
}
