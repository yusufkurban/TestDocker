using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroKubernete.Model;

namespace MicroKubernete.Repository
{
   public interface IRepository<T> where T : BaseEntity
   {
      int Add(T item);
      void Remove(int id);
      void Update(T item);
      T FindByID(int id);
      IEnumerable<T> FindAll();
   }
}
