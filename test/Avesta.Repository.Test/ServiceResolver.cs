﻿using Avesta.Data.Context;
using Avesta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avesta.Repository.Test
{
    public class ServiceResolver<TId, TEntity, TContext> : RepositoryResolver<TId, TEntity, TContext>
        where TId : class
        where TEntity : BaseEntity<TId>
        where TContext : AvestaDbContext
    {

    }
}
