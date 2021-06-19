using System;
using System.Collections.Generic;
using System.Text;

namespace DbContextUsingSqliteInMemoryTestSample.Infrastructures
{
    /// <summary>
    /// 接続するデータベース種別
    /// </summary>
    public static class DbSetting
    {
        public static string DataBaseName { get; set; } = "SQLServer";

        public static string SQLServerConnectionString { get; set; }

        public static string SqliteInMemoryConnectionString { get; set; }


    }
}
