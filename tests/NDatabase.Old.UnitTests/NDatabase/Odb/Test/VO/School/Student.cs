using System;
using System.Collections;

namespace Test.NDatabase.Odb.Test.VO.School
{
    public class Student
    {
        private int age;

        private Course course;

        private DateTime firstDate;
        private string id;

        private IList listHistory;
        private string name;

        public Student(int age, Course course, DateTime date, string id, string name)
        {
            this.age = age;
            this.course = course;
            firstDate = date;
            this.id = id;
            this.name = name;
            listHistory = new ArrayList();
        }

        public virtual int GetAge()
        {
            return age;
        }

        public virtual Course GetCourse()
        {
            return course;
        }

        public virtual DateTime GetFirstDate()
        {
            return firstDate;
        }

        public virtual string GetId()
        {
            return id;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetAge(int age)
        {
            this.age = age;
        }

        public virtual void SetCourse(Course course)
        {
            this.course = course;
        }

        public virtual void SetFirstDate(DateTime firstDate)
        {
            this.firstDate = firstDate;
        }

        public virtual void SetId(string id)
        {
            this.id = id;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual IList GetListHistory()
        {
            return listHistory;
        }

        public virtual void SetListHistory(IList listHistory)
        {
            this.listHistory = listHistory;
        }

        public virtual void AddHistory(History history)
        {
            history.SetStudent(this);
            listHistory.Add(history);
        }

        public override string ToString()
        {
            return "id=" + id + " | name=" + name + " | age= " + age + " | date=" + firstDate + " | course=" +
                   course.GetName() + " | history=" + listHistory;
        }
    }
}
