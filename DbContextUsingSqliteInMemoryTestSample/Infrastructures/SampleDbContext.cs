using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbContextUsingSqliteInMemoryTestSample.Infrastructures
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {

        }

        public DbSet<SampleModel> SampleModels { get; set; }
    }
}
