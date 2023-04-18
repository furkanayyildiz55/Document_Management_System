using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;  
using System.Linq.Expressions;


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

        public int Add(T p)
        {
            var addedEntity = context.Entry(p);
            addedEntity.State = EntityState.Added;

            context.SaveChanges();

            var type = typeof(T);
            if(type == typeof(DocumentType))
            {
                DocumentType documentType = (DocumentType)Convert.ChangeType( p , typeof(DocumentType) );
                return documentType.DocumentTypeID;
            }
            else if (type == typeof(Document))
            {
                Document document = (Document)Convert.ChangeType( p , typeof(Document) );
                return document.DocumentID;
            }
            return -1;
            
        }

        public void Update(T p)
        {
            var updatedEntity = context.Entry(p);
            updatedEntity.State = EntityState.Modified;
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

        public T Get(Expression<Func<T, bool>> filter)  //TODO:  bu  performan kaybı yaşatıyormuş  <Func<T, bool>> filter bu tarz bişey varmış dene !
        {
            return _object.SingleOrDefault(filter); //Dizde veya listede sadece bir değer döndürek için kullanılan EF linq methodu
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return _object.FirstOrDefault(filter);
        }

        









    }
}
