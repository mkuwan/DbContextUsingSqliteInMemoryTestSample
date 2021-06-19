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
        /// �R���X�g���N�^
        /// </summary>
        public UnitTest1()
        {
            // SqliteInMemory�ɐݒ�
            DbSetting.DataBaseName = "SqliteInMemory";

            // DB���̓e�X�g���ƂɕύX����Ƌ������Ȃ��Ă��݂܂�
            DbSetting.SqliteInMemoryConnectionString = $"DataSource=UnitTest1.db;mode=memory;";

            // DI�͎g�p�����ɒ��ڎ�������Ăяo���Ă��܂�
            dbContextOptionsFactory = new DbContextOptionsFactory();

            //�����f�[�^�쐬
            Seed();
        }


        /// <summary>
        /// �����f�[�^�쐬
        /// using(DbContext...�����������Ă����v
        /// </summary>
        [TestMethod("�����f�[�^�쐬")]
        private void Seed()
        {
            using (SampleDbContext sampleDbContext = new SampleDbContext(dbContextOptionsFactory.Options))
            {
                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "���͂悤"
                });

                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "����ɂ���"
                });

                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "���悤�Ȃ�"
                });



                sampleDbContext.SaveChanges();
            }

            using (SampleDbContext sampleDbContext = new SampleDbContext(dbContextOptionsFactory.Options))
            {
                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "���₷�݂Ȃ���"
                });

                sampleDbContext.SampleModels.Add(new SampleModel()
                {
                    Message = "���������܂�"
                });

                sampleDbContext.SaveChanges();
            }

            using(SampleDbContext sampleDbContext = new SampleDbContext(dbContextOptionsFactory.Options))
            {
                var count = sampleDbContext.SampleModels.Count();
                // �ŏ��ɓo�^����Ă����b�Z�[�W��5���
                Assert.IsTrue(count == 5);

                // ��Ŏg�p���邽�߁A"���͂悤"�̎�L�[���擾���Ă����܂�
                testId = sampleDbContext.SampleModels.Where(x => x.Message == "���͂悤").Select(x => x.SampleModelId).FirstOrDefault();
            }
        }

        [TestMethod("DbAccess�̃e�X�g")]
        public void DbAccessTest()
        {
            DbAccess dbAccess = new DbAccess(dbContextOptionsFactory);

            var count = dbAccess.CountMessage();

            // �ŏ��ɓo�^����Ă��郁�b�Z�[�W��5���
            Assert.IsTrue(count == 5);


            // �ǉ�
            // �����̃��b�Z�[�W������̂Ő��͕ύX�Ȃ��͂�
            dbAccess.AddMessage("���͂悤");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 5);

            // �V�������b�Z�[�W��ǉ�
            dbAccess.AddMessage("������������");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 6);

            // �폜
            // ���݂��Ȃ����b�Z�[�W�͍폜�ł������̕ύX�Ȃ��̂͂�
            dbAccess.DeleteMessage("���͂悤�������܂�");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 6);

            // �ύX
            // ���͂悤 => ���͂悤�������܂�
            dbAccess.UpdateMessage(testId, "���͂悤�������܂�");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 6);

            // �폜
            // ����ǂ�"���͂悤�������܂�"�͑��݂���̂ō폜�ł���
            dbAccess.DeleteMessage("���͂悤�������܂�");
            count = dbAccess.CountMessage();
            Assert.IsTrue(count == 5);

        }
    }
}
