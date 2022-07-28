﻿using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Mapping
{
    public class EmployeeMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(e => e.Address).HasMaxLength(60);

            builder.Property(e => e.BirthDate).HasColumnType("datetime");

            builder.Property(e => e.City).HasMaxLength(15);

            builder.Property(e => e.Country).HasMaxLength(15);

            builder.Property(e => e.Extension).HasMaxLength(4);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.HireDate).HasColumnType("datetime");

            builder.Property(e => e.HomePhone).HasMaxLength(24);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Notes).HasColumnType("ntext");

            builder.Property(e => e.Photo).HasColumnType("image");

            builder.Property(e => e.PhotoPath).HasMaxLength(255);

            builder.Property(e => e.PostalCode).HasMaxLength(10);

            builder.Property(e => e.Region).HasMaxLength(15);

            builder.Property(e => e.Title).HasMaxLength(30);

            builder.Property(e => e.TitleOfCourtesy).HasMaxLength(25);

            builder.HasOne(d => d.Manager)
                    .WithMany(p => p.DirectReports)
                    .HasForeignKey(d => d.ReportsTo);
        }
    }
}