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
    public class UserJWTTokenMapping : IEntityTypeConfiguration<UserJWTToken>
    {
        public void Configure(EntityTypeBuilder<UserJWTToken> builder)
        {
            builder.HasKey(e => e.jwt_Id);
            builder.Property(e => e.jwt_Id)
                  .ValueGeneratedOnAdd();
        }
    }
}
