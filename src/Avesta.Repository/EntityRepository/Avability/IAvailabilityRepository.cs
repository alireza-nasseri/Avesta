﻿using Avesta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Avesta.Repository.EntityRepository.Avability
{
    public interface IAvailabilityRepository<TEntity, TId>
        where TId : class
        where TEntity : BaseEntity<TId>
    {
        Task CheckAvailability(Expression<Func<TEntity, bool>> any, string navigationPropertyPath = null);
        Task<bool> Any(Expression<Func<TEntity, bool>> any, string navigationPropertyPath = null);
    }

}
