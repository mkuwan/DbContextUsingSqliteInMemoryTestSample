using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbContextUsingSqliteInMemoryTestSample.Infrastructures
{
    public class DbAccess
    {

        private readonly IDbContextOptionsFactory _dbContextOptionsFactory;

        public DbAccess(IDbContextOptionsFactory dbContextOptionsFactory)
        {
            _dbContextOptionsFactory = dbContextOptionsFactory;
        }

        /// <summary>
        /// 登録されているメッセージの数
        /// </summary>
        /// <returns></returns>
        public int CountMessage()
        {
            using (SampleDbContext sampleDbContext = new SampleDbContext(_dbContextOptionsFactory.Options))
            {
                return sampleDbContext.SampleModels.Count();
            }
        }

        /// <summary>
        /// 新規メッセージを登録します
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            using (SampleDbContext sampleDbContext = new SampleDbContext(_dbContextOptionsFactory.Options))
            {
                var model = sampleDbContext.SampleModels.Where(x => x.Message == message).FirstOrDefault();

                if(model == null)
                {
                    sampleDbContext.SampleModels.Add(new SampleModel()
                    {
                        Message = message
                    });
                    sampleDbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// メッセージの削除
        /// </summary>
        /// <param name="message"></param>
        public void DeleteMessage(string message)
        {
            using (SampleDbContext sampleDbContext = new SampleDbContext(_dbContextOptionsFactory.Options))
            {
                var model = sampleDbContext.SampleModels.Where(x => x.Message == message).FirstOrDefault();

                if (model != null)
                {
                    sampleDbContext.SampleModels.Remove(model);
                    sampleDbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// メッセージに変更
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void UpdateMessage(int id, string message)
        {
            using (SampleDbContext sampleDbContext = new SampleDbContext(_dbContextOptionsFactory.Options))
            {
                var model = sampleDbContext.SampleModels.Where(x => x.SampleModelId == id).FirstOrDefault();

                if(model != null)
                {
                    model.Message = message;
                    sampleDbContext.SaveChanges();
                }
            }
        }
    }
}
