using System;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Download;

namespace Test.NDatabase.Odb.Test.Other
{
    [TestFixture]
    public class TestDownloadManager : ODBTest
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DeleteBase("download.ndb");
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteBase("download.ndb");
        }

        #endregion

        public virtual void NewDownload(string name, string email, string downloadType, string fileName)
        {
            IOdb odb = null;
            User user = null;
            try
            {
                odb = Open("download.ndb");
                var query = odb.Query<User>();
                query.Descend("email").Constrain((object) email).Equal();
                var users = query.Execute<User>();
                if (users.Count != 0)
                {
                    user = users.GetFirst();
                    user.SetLastDownload(new DateTime());
                    user.SetNbDownloads(user.GetNbDownloads() + 1);
                    odb.Store(user);
                }
                else
                {
                    user = new User();
                    user.SetName(name);
                    user.SetEmail(email);
                    user.SetLastDownload(new DateTime());
                    user.SetNbDownloads(1);
                    odb.Store(user);
                }
                var download = new Download();
                download.SetFileName(fileName);
                download.SetType(downloadType);
                download.SetUser(user);
                download.SetWhen(new DateTime());
                odb.Store(download);
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }

        public static void Main2(string[] args)
        {
            var td = new TestDownloadManager();

            for (var i = 0; i < 2000; i++)
            {
                td.SetUp();
                td.Test1();
                td.TearDown();
                td.Test1();
            }
        }

        [Test]
        public virtual void Test1()
        {
            var tdm = new TestDownloadManager();
            tdm.NewDownload("olivier", "user@ndatabase.net", "knowledger", "knowledger1.1");
            tdm.NewDownload("olivier", "user@ndatabase.net", "knowledger", "knowledger1.1");
            var odb = Open("download.ndb");
            AssertEquals(2, odb.Query<Download>().Count());
            AssertEquals(1, odb.Query<User>().Count());
            odb.Close();
        }

        [Test]
        public virtual void Test2()
        {
            var tdm = new TestDownloadManager();
            var size = 100;
            for (var i = 0; i < size; i++)
            {
                tdm.NewDownload("olivier", "user@ndatabase.net", "knowledger", "knowledger1.1");
                tdm.NewDownload("olivier", "user@ndatabase.net", "knowledger", "knowledger1.1");
            }
            var odb = Open("download.ndb");
            AssertEquals(size * 2, odb.Query<Download>().Count());
            AssertEquals(1, odb.Query<User>().Count());
            odb.Close();
        }
    }
}
