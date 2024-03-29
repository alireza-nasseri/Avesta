﻿using AutoMapper;
using Avesta.Data.Entity.Model;
using Avesta.Share.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Avesta.Services.Entity.Delete
{
    public interface IDeleteEntityService<TId, TEntity, TModel>
        where TId : class
        where TEntity : BaseEntity<TId>
        where TModel : BaseModel<TId>
    {
        Task Delete(TModel model, bool exceptionRaiseIfNotExist = false);
        Task Delete(TId id, bool exceptionRaiseIfNotExist = false);
        Task Delete(Expression<Func<TEntity, bool>> single, bool exceptionRaiseIfNotExist = false);
        Task DeleteRange(IEnumerable<TModel> models);
        Task DeleteRange(Expression<Func<TEntity, bool>> where);
        Task SoftDelete(TId id, bool exceptionRaiseIfNotExist = false);
    }




 


}
