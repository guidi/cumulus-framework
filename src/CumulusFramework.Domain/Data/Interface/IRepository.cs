﻿using System.Collections.Generic;

namespace CumulusFramework.Domain.Data.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IList<TEntity> GetAll();
        TEntity GetById(int id);
        TEntity Save(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);
        void Evict(TEntity entity);
    }
}
