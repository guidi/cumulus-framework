using CumulusFramework.Domain.Data.Interface;
using CumulusFramework.Infra.Data.UnitOfWork;
using NHibernate;
using System;
using System.Collections.Generic;

namespace CumulusFramework.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly NHibernateUnitOfWork _unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = (NHibernateUnitOfWork)unitOfWork;
        }

        protected ISession Session
        {
            get
            {
                return _unitOfWork.Session;
            }
        }

        public void Delete(TEntity entity)
        {            
            try
            {
                Session.Delete(entity);
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var entity = Session.Load<TEntity>(id);
                Session.Evict(entity);
                Session.Delete(entity);
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        public void Evict(TEntity entity)
        {
            Session.Evict(entity);
        }

        public IList<TEntity> GetAll()
        {
            try
            {
                return Session.CreateCriteria(typeof(TEntity)).List<TEntity>();
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        public TEntity GetById(int id)
        {
            try
            {
                var entity =  Session.Get<TEntity>(id);
                return entity;
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }            
        }

        public TEntity Save(TEntity entity)
        {
            try
            {
                Session.SaveOrUpdate(entity);                             
                return entity;
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }

        public TEntity Update(TEntity entity)
        {
            try
            {
                Session.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                String log = ex.Message;
                throw;
            }
        }
    }
}
