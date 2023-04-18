using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    //Entity Layer da yer alan her sınıf için CRUD işlemi yapılmalı. 
    //Generiklik için kurulan bu sisemde ilk önce CRUD işlemi yapacak olan sınıfa yön vermesi açısından interface yazıyoruz
    //Ardından her sınıf interfacedeki methodlarına göre işlem yapıyor

    //IRepository sınıfının amacı da birden fazla interface sınıfını tek noktadan yönetmek
    //Böylekikle kod tekarının önüne geçilmiş olacak
    public interface IRepository<T>
    {
        List<T> List();
        int Add(T p);
        void Delete(T p);
        void Update(T p);
        T Get(Expression<Func<T, bool>> filter);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);

        List<T> List(Expression<Func<T, bool>> filter);

    }
}
