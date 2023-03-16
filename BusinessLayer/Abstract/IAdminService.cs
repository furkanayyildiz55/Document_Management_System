using System.Collections.Generic;
using EntityLayer.Concrete;


namespace BusinessLayer.Abstract
{
    public interface IAdminService
    {
        List<Admin> GetList();
        List<Admin> GetListTopLevelAdmin();
        void AdminAdd(Admin admin);
        void AdminDelete(Admin admin);
        void AdminUpdate(Admin admin);
        Admin GetAdmin(int admin);
    }
}
