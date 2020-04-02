using CumulusFramework.Domain.Data.Interface;
using CumulusFramework.Domain.Entity;
using CumulusFramework.Infra.Data.Context;
using NHibernate;
using System;

namespace CumulusFramework.Infra.Data.UnitOfWork
{
    public abstract class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly DBEnvironment _environment;

        private ISession _context;
        private ISessionFactory _factory;
        private ITransaction _transaction;

        protected NHibernateUnitOfWork(DBEnvironment environment)
        {
            _environment = environment;
        }

        public ISession Session
        {
            get
            {
                if (_factory == null)
                {
                    _factory = DataContextFactory.GetContextForConnection(_environment);
                }

                if (_context == null || !_context.IsOpen)
                {
                    _context = _factory.OpenSession();
                }
                return _context;
            }
        }

        public void BeginTransaction()
        {
            if (_context != null && _context.IsOpen)
            {
                _transaction = _context.BeginTransaction();
            }
            else
            {
                _transaction = Session.BeginTransaction();
            }
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch (Exception)
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                //Se ocorrer rollback descarta a sessão, necessário para não ocorrer o erro null id in entry (don't flush the Session after an exception occurs)
                //e conforme documentação em http://nhibernate.info/doc/nhibernate-reference/manipulatingdata.html
                if (_transaction != null && _transaction.IsActive)
                {
                    _transaction.Rollback();
                    Session.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
