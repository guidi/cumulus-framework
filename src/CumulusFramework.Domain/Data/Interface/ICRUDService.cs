using CumulusFramework.Domain.ValueObject;
using System.Collections.Generic;

namespace CumulusFramework.Domain.Data.Interface
{
    public interface ICRUDService<TEntity> where TEntity : class
    {
        List<MessageVO> Messages { get; set; }
        void Save(TEntity entity);
        void Delete(TEntity entity);
    }
}
