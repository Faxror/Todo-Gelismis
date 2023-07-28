using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcces
{
    public class DBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-I45D279;Initial Catalog=SonDatabaseV3;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentTime = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    // Yeni nesne ekleniyor, createddate ve modifieddate alanlarına şu anki tarihi atayın
                    if (entry.Entity is BaseEntity baseEntity)
                    {
                        baseEntity.createddate = currentTime;
                        baseEntity.modifieddate = currentTime;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Varolan nesne güncelleniyor, modifieddate alanına şu anki tarihi atayın
                    if (entry.Entity is BaseEntity baseEntity)
                    {
                        baseEntity.modifieddate = currentTime;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Test> Tessts { get; set; }
        public DbSet<Categorys> Categorys { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Durum> Durums { get; set; }
    }
}
