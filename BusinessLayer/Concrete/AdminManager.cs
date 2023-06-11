using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class AdminManager : IAdminService
    {

        IAdminDal _admin;


        public AdminManager(IAdminDal admin)
        {
            _admin = admin;
        }

        public void AdminAdd(Admin admin)
        {
            _admin.Add(admin);
        }

        public void AdminDelete(Admin admin)
        {
            _admin.Delete(admin);
        }

        public void AdminUpdate(Admin admin)
        {
            _admin.Update(admin);
        }

        public List<Admin> GetList()
        {
            return _admin.List();
        }

        public Admin GetAdmin(int AdminID)
        {
            return _admin.Get(x => x.AdminID == AdminID);
        }

        public Admin GetAdminWithMail(string AdminMail)
        {
            return _admin.Get(x => x.AdminMail == AdminMail);
        }

        public List<Admin> GetListTopLevelAdmin()
        {
            return _admin.List(x => x.AdminAuthorization == true && x.AdminStatus==true);
        }

        //public List<Admin> AdminCheckRecord( )
        //{
        //    return _admin.List(x => x.AdminAuthorization == true);
        //}

    }
}
