using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EfEntityFramework
{
    public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<List<T>> GetList(Expression<Func<T,bool>> filter=null);
        Task<T> Add(T entity);
        Task<T> Delete(T entity);
        Task<T> HardDelete(T entity);
        Task<T> Update(T entity);
    }
}
