namespace CumulusFramework.Domain.Data.Interface
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
