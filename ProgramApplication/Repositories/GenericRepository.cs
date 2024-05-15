using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ProgramApplication.Repositories
{
    public interface IGenericRepository<T>
        where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        bool Save();
    }

    ///<remark/>
    public interface IDbContext
    {

        //
        // Summary:
        //     Gets a System.Data.Entity.Infrastructure.DbEntityEntry object for the given
        //     entity providing access to information about the entity and the ability to
        //     perform actions on the entity.
        //
        //// Parameters:
        ////   entity:
        ////     The entity.
        ////
        //// Returns:
        ////     An entry for the entity.
        ///<remark/>
        EntityEntry Entry(object entity);
        //
        // Summary:
        //     Gets a System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> object for
        //     the given entity providing access to information about the entity and the
        //     ability to perform actions on the entity.
        //
        // Parameters:
        //   entity:
        //     The entity.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     An entry for the entity.
        ///<remark/>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        [EditorBrowsable(EditorBrowsableState.Never)]
        //
        // Summary:
        //     Validates tracked entities and returns a Collection of System.Data.Entity.Validation.DbEntityValidationResult
        //     containing validation results.
        //
        // Returns:
        //     Collection of validation results for invalid entities. The collection is
        //     never null and must not contain null values or results for valid entities.
        //
        // Remarks:
        //     1. This method calls DetectChanges() to determine states of the tracked entities
        //     unless DBContextConfiguration.AutoDetectChangesEnabled is set to false. 
        //     2. By default only Added on Modified entities are validated. The user is
        //     able to change this behavior by overriding ShouldValidateEntity method.
        // IEnumerable<DbEntityValidationResult> GetValidationErrors();

        //
        // Summary:
        //     Saves all changes made in this context to the underlying database.
        //
        // Returns:
        //     The number of objects written to the underlying database.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     Thrown if the context has been disposed.
        //int SaveChanges();

        //
        // Summary:
        //     Saves all changes made in this context to the underlying database.
        //Exceptions:
        //   System.InvalidOperationException:
        ///<remark/>

        //     Thrown if the context has been disposed.
        int SaveChanges();

        //
        // Summary:
        //     Returns a DbSet instance for access to entities of the given type in the
        //     context, the ObjectStateManager, and the underlying store.
        //
        // Type parameters:
        //   TEntity:
        //     The type entity for which a set should be returned.
        //
        // Returns:
        //     A set for the given entity type.
        //
        // Remarks:
        //     See the DbSet class for more details.
        ///<remark/>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

    public abstract class GenericRepository<T> : IGenericRepository<T>
      where T : class
    {
        private readonly IDbContext entities;
        public GenericRepository(IDbContext ntities)
        {
            entities = ntities;
        }

        public virtual IQueryable<T> Query()
        {
            try
            {
                IQueryable<T> query = entities.Set<T>();
                return query;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            try
            {
                IQueryable<T> query = entities.Set<T>().Where(predicate);
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public virtual void Add(T entity)
        {

            entities.Set<T>().Add(entity);

        }

        public void Delete(T entity)
        {
            entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual bool Save()
        {
            return (entities.SaveChanges() > 0);
        }

    }

}
