﻿using Avesta.Data.Entity.Model;
using Avesta.Services.Entity.Availability;
using Avesta.Services.Entity.Create;
using Avesta.Services.Entity.Delete;
using Avesta.Services.Entity.Graph;
using Avesta.Services.Entity.Read;
using Avesta.Services.Entity.Update;
using Avesta.Share.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Avesta.Services.Entity
{

    public interface IEntityService<TEntity, TModel> : IReadEntityService<string, TEntity, TModel>
    , IDeleteEntityService<string, TEntity, TModel>
    , IUpdateEntityService<string, TEntity, TModel>
    , ICreateEntityService<string, TEntity, TModel>
    , IAvailabilityService<string, TEntity, TModel>
    where TEntity : BaseEntity<string>
    where TModel : BaseModel<string>
    {
    }

    public interface IEntityService<TEntity, TModel, TCreateModel, TEditModel> : IEntityService<string, TEntity, TModel>
    , IUpdateEntityService<string, TEntity, TModel, TEditModel>
    , ICreateEntityService<string, TEntity, TModel, TCreateModel>
    where TEntity : BaseEntity<string>
    where TModel : BaseModel<string>
    where TCreateModel : TModel
    where TEditModel : TModel
    { }



    public interface IEntityService<TId, TEntity, TModel> : IReadEntityService<TId, TEntity, TModel>
        , IDeleteEntityService<TId, TEntity, TModel>
        , IUpdateEntityService<TId, TEntity, TModel>
        , ICreateEntityService<TId, TEntity, TModel>
        , IAvailabilityService<TId, TEntity, TModel>
        where TId : class
        where TEntity : BaseEntity<TId>
        where TModel : BaseModel<TId>
    {
    }


    public interface IEntityService<TId, TEntity, TModel, TCreateModel, TEditModel> : IEntityService<TId, TEntity, TModel>
        , IUpdateEntityService<TId, TEntity, TModel, TEditModel>
        , ICreateEntityService<TId, TEntity, TModel, TCreateModel>
        where TId : class
        where TEntity : BaseEntity<TId>
        where TModel : BaseModel<TId>
        where TCreateModel : TModel
        where TEditModel : TModel
    { }
}
