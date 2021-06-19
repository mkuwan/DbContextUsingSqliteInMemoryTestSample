using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace DbContextUsingSqliteInMemoryTestSample.Infrastructures
{
    public class DbContextOptionsFactory : IDbContextOptionsFactory
    {
        private DbConnection _dbConnection;

        public DbContextOptions<SampleDbContext> Options { get; private set; }

        public DbContextOptionsFactory()
        {
            SetDbContextOptions();
        }

        private void SetDbContextOptions()
        {
            switch (DbSetting.DataBaseName)
            {
                case "SQLServer":
                    {
                        var option = new DbContextOptionsBuilder<SampleDbContext>();
                        Options = option.UseSqlServer(DbSetting.SQLServerConnectionString).Options;
                        break;
                    }

                case "SqliteInMemory":
                    {

                        if (_dbConnection == null)
                        {
                            _dbConnection = new SqliteConnection(DbSetting.SqliteInMemoryConnectionString);
                            _dbConnection.Open();
                            var option = new DbContextOptionsBuilder<SampleDbContext>();

                            // InMemoryではトランザクション(スコープ)を使うとエラーになってしまうので回避コード
                            option.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));

                            Options = option.UseSqlite(_dbConnection).Options;

                            using (var context = new SampleDbContext(Options))
                            {
                                context.Database.EnsureDeleted();
                                context.Database.EnsureCreated();
                            }
                        }
                        break;
                    }

            }
        }

    }
}
