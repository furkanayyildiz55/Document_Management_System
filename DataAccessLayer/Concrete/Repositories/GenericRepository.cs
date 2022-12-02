using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
   //Generic repository bütün veritabanı tabloları için CRUD işlemlerini yapan clastır
   //Generic yapısı sayesinde gelen sınıfın anlar ve veritabanında o sınıfa göre işlem yapar
   //<T> parametresi işlem yapılacak sınıfı belirtir

   //IRepository<T> ise implemente edilecek generic interface dir. T değeri sadece class olabilir...


    public class GenericRepository<T> : IRepository<T> where T : class
    {
        Context context =new Context();
        DbSet<T> _object;

        public GenericRepository()
        {
            //context.Set<T>(); ile _object nesnesinin belirli olmayan T türünü belirliyoruz.
            _object = context.Set<T>();
        }

        public void Delete(T p)
        {
            _object.Remove(p);
            context.SaveChanges();
        }

        public void Add(T p)
        {
            _object.Add(p);
            context.SaveChanges();
        }

        public void Update(T p)
        {
            context.SaveChanges();
        }

        public List<T> List()
        {
            return _object.ToList();

        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }



    }
}
