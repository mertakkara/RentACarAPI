﻿using Microsoft.EntityFrameworkCore;
using RentACarAPI.Domain.Entities;
using RentACarAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = RentACarAPI.Domain.Entities.File;

namespace RentACarAPI.Persistence.Contexts
{
    public class RentACarAPIDbContext : DbContext
    {
        public RentACarAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<CarImageFile> CarImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker Entityler üzerinden yapılan değişiklerin ya da yeni eklenen verinin yakalanmasını sağlayan propertydir.Update operasyonlarında Track edilen verileri yakalayıp elde etmemizi sağlar
            var data = ChangeTracker.Entries<BaseEntity>();
            foreach (var item in data)
            {
                _ = item.State switch
                {
                    EntityState.Added => item.Entity.CreatedDateTime = DateTime.UtcNow,
                    EntityState.Modified => item.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
