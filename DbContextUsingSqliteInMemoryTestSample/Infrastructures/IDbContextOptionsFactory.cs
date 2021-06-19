using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbContextUsingSqliteInMemoryTestSample.Infrastructures
{
    public interface IDbContextOptionsFactory
    {
        DbContextOptions<SampleDbContext> Options { get; }
    }
}
