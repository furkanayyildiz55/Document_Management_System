using System.Collections.Generic;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IStudentService
    {
        List<Student> GetList();
        void StudentAdd(Student student);
        void StudentDelete(Student student);
        void StudentUpdate(Student student);
        Student GetStudent(int student);
        Student GetStudentWihtNumber(string studentNo);

    }
}
