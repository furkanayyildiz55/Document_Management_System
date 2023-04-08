using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class Repository
    {
        
        //public void DocumentSignaturesWithAdminID(int  AdminID )
        //{
        //    using (var context = new Context())
        //    {
        //        //var query = "SELECT * FROM Customers WHERE CustomerId = @customerId";
        //        var query = "SELECT * FROM Students";

        //       // var customerIdParameter = new SqlParameter("@customerId", desiredCustomerId);
        //        var customers = context.Students.SqlQuery(query).ToList();

                

        //        var custom=1;
        //    }


        public object DocumentSignaturesWithAdminID(int AdminID)
        {
            using (var context = new Context())
            {
                var result = from ds in context.DocumentSignatures
                             join dts in context.DocumentTypeSignatures
                             on ds.DocumentTypeSignatureID equals dts.DocumentTypeSignatureID
                             where dts.AdminID == AdminID

                             join dt in context.DocumentTypes
                             on dts.DocumentTypeID equals dt.DocumentTypeID

                             join d in context.Documents
                             on ds.DocumentID equals d.DocumentID

                             join s in context.Students
                             on d.StudentID equals s.StudentID
                             

                             select new
                             {
                                 DocumentName = dt.DocumentTypeName,
                                 DocumentCreateDate = d.DocumentCreateDate,
                                 StudentFullName = s.StudentName + s.StudentSurname,
                                 StudentProgram = s.StudentProgram == null ? "Program Yok" : s.StudentProgram,
                                 DocumentSignatureID = ds.DocumentSignatureID
                             };


                var list = result.ToList(); 
                return list;
            }

        }




    }
}
