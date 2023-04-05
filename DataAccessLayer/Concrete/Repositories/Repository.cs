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
        
        public void DocumentSignaturesWithAdminID(int  AdminID )
        {
            using (var context = new Context())
            {
                //var query = "SELECT * FROM Customers WHERE CustomerId = @customerId";
                var query = "SELECT * FROM Students";

               // var customerIdParameter = new SqlParameter("@customerId", desiredCustomerId);
                var customers = context.Students.SqlQuery(query).ToList();

                var custom=1;
            }

        }



    }
}
