using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class StudentManager : IStudentService
    {
        IStudentDal _student;

        public StudentManager(IStudentDal student)
        {
            _student = student;
        }

        public List<Student> GetList()
        {
            return _student.List();
        }

        public Student GetStudent(int studentID)
        {
           return  _student.Get(x => x.StudentID == studentID);
        }

        public Student GetStudentWihtNumber(string studentNo)
        {
            return _student.Get(x => x.StudentNo == studentNo);
        }

        public Student GetStudentWithMail(string studentMail)
        {
            return _student.Get(x => x.StudentMail == studentMail);
        }

        public void StudentAdd(Student student)
        {
            _student.Add(student);
        }

        public void StudentDelete(Student student)
        {
            _student.Delete(student);
        }

        public void StudentUpdate(Student student)
        {
            _student.Update(student);
        }

        
    }
}
