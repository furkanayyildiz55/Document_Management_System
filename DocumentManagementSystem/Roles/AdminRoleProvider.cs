using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DocumentManagementSystem.Roles
{
    public class AdminRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        AdminManager adminManager = new AdminManager(new EfAdminDal());

        //Authentication tarafında cookie oluşuturulurken tanımlanan adminID buraya geliyor
        public override string[] GetRolesForUser(string adminID)
        {
            try
            {
                if (adminID.Contains("student"))
                {
                    return new string[] { };
                }
                else
                {
                    //Giriş yapan adminin role bilgisi sorgulanıyor
                    Admin admin = adminManager.GetAdmin(int.Parse(adminID));
                    return new string[] { admin.AdminAuthorization == true ? "1" : "0" };
                }
            }
            catch (Exception )
            {
                throw;
            }



        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }



        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}