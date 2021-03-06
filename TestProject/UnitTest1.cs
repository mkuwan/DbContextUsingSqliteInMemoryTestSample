using DbContextUsingSqliteInMemoryTestSample.Infrastructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        DbContextOptionsFactory dbContextOptionsFactory;
        int testId = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnitTest1()
        {
            // SqliteInMemoryに設定
            DbSetting.DataBaseName = "SqliteInMemory";

            // DB名はテストごとに変更すると競合しなくてすみます
            DbSetting.SqliteInMemoryConnectionString = $"DataSource=UnitTest1.db;mode=memory;";

            // DIは使用せずに直接実装から呼び出しています
            dbContextOptionsFactory = new DbContextOptionsFactory();

            //初期データ作成
            Seed();
        }


        /// <summary>
        /// 初期データ作成
        /// using(DbContext...が複数あっても大丈夫
        /// </summary>
        [TestMethod("初期データ作成")]
        private void Seed()
        {
            using (SampleDbContext sampleDbContext = new SampleDbContext(dbContextOptionsFactory.Options))
            {
                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "おはよう"
                });

                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "こんにちは"
                });

                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "さようなら"
                });



                sampleDbContext.SaveChanges();
            }

            using (SampleDbContext sampleDbContext = new SampleDbContext(dbContextOptionsFactory.Options))
            {
                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "おやすみなさい"
                });

                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "いただきます"
                });

                sampleDbContext.SaveChanges();
            }

            using(SampleDbContext sampleDbContext = new SampleDbContext(dbContextOptionsFactory.Options))
            {
                var count = sampleDbContext.SampleModels.Count();
                // 最初に登録されてたメッセージは5種類
                Assert.IsTrue(count == 5);

                // 後で使用するため、"おはよう"の主キーを取得しておきます
                testId = sampleDbContext.SampleModels.Where(x => x.Message == "おはよう").Select(x => x.SampleModelId).FirstOrDefault();
            }
        }

        [TestMethod("DbAccessのテスト")]
        public void DbAccessTest()
        {
            DbAccess dbAccess = new DbAccess(dbContextOptionsFactory);

            var count = dbAccess.CountMessage();

            // 最初に登録されているメッセージは5種類
            Assert.IsTrue(count == 5);


            // 追加
            // 既存のメッセージがあるので数は変更ないはず
            dbAccess.AddMessage("おはよう");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 5);

            // 新しいメッセージを追加
            dbAccess.AddMessage("ごちそうさま");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 6);

            // 削除
            // 存在しないメッセージは削除できず数の変更なしのはず
            dbAccess.DeleteMessage("おはようございます");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 6);

            // 変更
            // おはよう => おはようございます
            dbAccess.UpdateMessage(testId, "おはようございます");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 6);

            // 削除
            // こんどは"おはようございます"は存在するので削除できる
            dbAccess.DeleteMessage("おはようございます");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 5);

        }
    }
}
