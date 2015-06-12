using System;

namespace Test.NDatabase.Odb.Test.VO.School
{
    public class History
    {
        private DateTime date;
        private Discipline discipline;

        private int score;

        private Student student;
        private Teacher teacher;

        public History()
        {
        }

        public History(DateTime data, Discipline discipline, int score, Teacher teacher)
        {
            date = data;
            this.discipline = discipline;
            this.score = score;
            this.teacher = teacher;
        }

        public virtual DateTime GetDate()
        {
            return date;
        }

        public virtual Discipline GetDiscipline()
        {
            return discipline;
        }

        public virtual int GetScore()
        {
            return score;
        }

        public virtual void SetDate(DateTime data)
        {
            date = data;
        }

        public virtual void SetDiscipline(Discipline discipline)
        {
            this.discipline = discipline;
        }

        public virtual void SetScore(int score)
        {
            this.score = score;
        }

        public virtual Teacher GetTeacher()
        {
            return teacher;
        }

        public virtual void SetTeacher(Teacher teacher)
        {
            this.teacher = teacher;
        }

        public override string ToString()
        {
            return "disc.=" + discipline.GetName() + " | teacher=" + teacher.GetName() + " | student=" +
                   student.GetName() + " | date=" + date + " | score=" + score;
        }

        public virtual Student GetStudent()
        {
            return student;
        }

        public virtual void SetStudent(Student student)
        {
            this.student = student;
        }
    }
}
