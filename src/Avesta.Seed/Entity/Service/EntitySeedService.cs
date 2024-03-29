﻿
using Avesta.Data.Entity.Context;
using Avesta.Data.Entity.Model;
using Avesta.Exceptions;
using Avesta.Repository.EntityRepository;
using Avesta.Seed.Extension;
using Avesta.Seed.Model;
using Avesta.Share.Extensions;

namespace Avesta.Seed.Entity.Service
{




    public class EntitySeedService<TId, TEntity, TAvestaDbContext> : IEntitySeedService<TId, TEntity, TAvestaDbContext>
        where TId : class
        where TEntity : BaseEntity<TId>
        where TAvestaDbContext : AvestaDbContext
    {
        readonly IEntityRepository<TEntity, TId> _entityRepository;
        readonly ISeedDataGenerator _dataGenerator;
        public EntitySeedService(IEntityRepository<TEntity, TId> entityRepository, ISeedDataGenerator dataGenerator)
        {
            _entityRepository = entityRepository;
            _dataGenerator = dataGenerator;
        }

        public async Task<SeedResultModel> SeedEntity(int number = 100)
        {
            await Task.CompletedTask;

            var result = new SeedResultModel();
            var currentInstance = default(TEntity);

            number.For(Try.Catch(async () =>
            {
                currentInstance = SeedExtension.CreateInstance<TEntity>(_dataGenerator);
                await _entityRepository.Insert(currentInstance);
            }, (e) =>
            {
                result.Successful = false;
                result.Message += $"instance with following information did not apply to the database with error message of '${e.Message}' \n instance info : \n ${currentInstance?.ToString()}";
            }));

            return result;
        }



    }


}
