using Avesta.Share.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Avesta.Repository.EntityRepository
{





    public interface IRepository<TEntity>
    {
        #region get entity
        Task<TEntity> GetByIdAsync(int id, bool track = true, bool exceptionRaseIfNotExist = false);
        Task<TEntity> GetByIdAsync(object key, bool track = true, bool exceptionRaseIfNotExist = false);
        Task<TEntity> GetEntity(Expression<Func<TEntity, bool>> predicate, bool exceptionRaseIfNotExist);
        Task<TEntity> GetEntity(string navigationPropertyPath, Expression<Func<TEntity, bool>> predicate, bool exceptionRaseIfNotExist);
        Task<TEntity> First(bool exceptionRaseIfNotExist);
        #endregion

        #region get entities
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool track = true);
        Task<IEnumerable<TEntity>> GetAllAsync(bool track = false);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take);
        Task<IEnumerable<TEntity>> GetAllIncludeAllChildren(bool track = false);
        Task<IEnumerable<TEntity>> GetAllIncludeAllChildren(int skip, int take, bool track = false);
        Task<TEntity> GetIncludeAllChildren(string id, bool track = false);
        Task<IEnumerable<TEntity>> GetAllByInclude(string navigationPropertyPath);
        Task<IEnumerable<TEntity>> GetAllByInclude(string navigationPropertyPath, int skip, int take);
        Task<IEnumerable<TEntity>> GetAllAsync<TKey>(string navigationPropertyPath = null, Func<TEntity, TKey> descendingOrder = null);
        #endregion



        #region availability
        Task CheckAvailability(Expression<Func<TEntity, bool>> any);
        #endregion


        #region insert or update
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task InsertRange(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateOrInsert(TEntity entity);
        Task ClearAllTEntityInDbThenAddRange(IEnumerable<TEntity> insertEntities);
        Task ClearAllTEntityInDbThenAddRange(IEnumerable<TEntity> removeEntities, IEnumerable<TEntity> insertEntities);
        Task ReCreate(Expression<Func<TEntity, bool>> deleteCondition, IEnumerable<TEntity> insertEntities);
        Task ReCreate(string navigationPropertyPath, Expression<Func<TEntity, bool>> deleteCondition, IEnumerable<TEntity> insertEntities);
        #endregion


        #region entity state
        Task DetachEntity(TEntity entity);
        #endregion

        #region delete
        Task DeleteAsync(TEntity entity);
        Task DeleteAsyncById(object id);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteWithAllChildren(string id);
        Task SoftDelete(string id, bool exceptionRaseIfNotExist);
        #endregion



        #region where
        Task<IEnumerable<TEntity>> WhereByInclude(string navigationPropertyPath, Expression<Func<TEntity, bool>> search);
        Task<IEnumerable<TEntity>> WhereByInclude(string navigationPropertyPath, string dynamicQuery, int skip, int take);
        Task<IEnumerable<TResult>> DynamicQuery<TResult>(string navigationPropertyPath, string where, string select, int skip, int take)
            where TResult : class;
        Task<IEnumerable<TResult>> DynamicQuery<TResult>(string navigationPropertyPath, string where, string select, string orderBy, int? takeFromLast)
            where TResult : class;
        Task<IEnumerable<TEntity>> WhereByInclude(string navigationPropertyPath, string dynamicQuery);
        Task<IEnumerable<TEntity>> GetAllByPropertyInfo(PropertyInformation info);
        #endregion


        #region single
        Task<TEntity> SingleOrDefaultAsyncByInclude(string navigationPropertyPath, Expression<Func<TEntity, bool>> search);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> search, bool exceptionIfNotExist = false);
        Task<TEntity> SingleAsyncByInclude(string navigationPropertyPath, Expression<Func<TEntity, bool>> search);
        #endregion


        #region include
        Task<IQueryable<TEntity>> Include(string navigationPropertyPath);
        Task<IQueryable<TEntity>> Query(bool eager = false);
        #endregion

        #region any
        Task<bool> AnyByInclude(string navigationPropertyPath, Expression<Func<TEntity, bool>> any);
        Task<bool> Any(Expression<Func<TEntity, bool>> any);
        #endregion

        Task<int> Count();
        Task<int> Count(string navigationPropertyPath, string where);


        #region propeties
        public IQueryable<TEntity> Table
        {
            get;
        }
        #endregion
    }
}
