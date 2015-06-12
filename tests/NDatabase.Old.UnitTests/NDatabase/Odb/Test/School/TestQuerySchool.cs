using System;
using NDatabase;
using NDatabase.Api;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.School;

namespace Test.NDatabase.Odb.Test.School
{
    [TestFixture]
    public class TestQuerySchool : ODBTest
    {
        // possiveis consultas
        // Listar todos os alunos de determinado professor
        // Listar alunos com nota abaixo de x
        // Listar disciplinas que um professor ministrou no semestre

        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DeleteBase("t-school.ndb");
            var odb = Open("t-school.ndb");
            var query = odb.Query<Student>();
            var students = query.Execute<Student>(true);
            var numStudents = students.Count;
            var computerScience = new Course("Computer Science");
            var teacher = new Teacher("Jeremias", "Java");
            var dw1 = new Discipline("Des. Web 1", 3);
            var @is = new Discipline("Intranet/Seguran√ßa", 4);
            var std1 = new Student(20, computerScience, new DateTime(), "1cs", "Brenna");
            var h1 = new History(new DateTime(), dw1, 0, teacher);
            var h2 = new History(new DateTime(), @is, 0, teacher);
            std1.AddHistory(h1);
            std1.AddHistory(h2);
            odb.Store(std1);
            odb.Commit();
            odb.Close();
            odb = Open("t-school.ndb");
            var query1 = odb.Query<Student>();
            students = query1.Execute<Student>(true);
            odb.Close();
            AssertEquals(numStudents + 1, students.Count);
        }

        #endregion

        /// <exception cref="System.Exception"></exception>
        public override void TearDown()
        {
            DeleteBase("t-school.ndb");
        }

        [Test]
        public virtual void Test12()
        {
            IOdb odb = null;
            try
            {
                odb = Open("t-school.ndb");
                var ci = ((global::NDatabase.Odb)odb).GetStorageEngine().GetSession().GetMetaModel().GetClassInfo(typeof(Student), true);

                AssertFalse(ci.HasCyclicReference());
            }
            finally
            {
                if (odb != null)
                    odb.Close();
            }
        }
    }
}
