﻿using AutoMapper;
using Avesta.Share.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Avesta.Repository.EntityRepository;
using Avesta.Storage.Constant;
using Avesta.Data.Model;
using Avesta.Share.Model;
using Avesta.Share.Model.Controller;

namespace Avesta.Services
{

    public class EntityService<TModel, TViewModel>
        where TModel : BaseEntity
        where TViewModel : BaseModel
    {
        readonly IRepository<TModel> _repository;
        readonly IMapper _mapper;
        public EntityService(IRepository<TModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TModel>> GetAllEntitiesWithAllChildren()
        {
            var result = await _repository.GetAllIncludeAllChildren();
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<TModel>> GetAllEntitiesWithAllChildren(int skip, int take)
        {
            var result = await _repository.GetAllIncludeAllChildren(skip, take);
            return result.OrderByDescending(i => i.CreatedDate);
        }

        public virtual async Task<IEnumerable<TModel>> GetAllEntitiesWithSpecificChildren(string navigationPropertyPath)
        {
            var result = await _repository.GetAllByInclude(navigationPropertyPath: navigationPropertyPath);
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<TModel>> GetAllEntitiesWithSpecificChildren(string navigationPropertyPath, int skip, int take)
        {
            var result = await _repository.GetAllByInclude(navigationPropertyPath: navigationPropertyPath, skip, take);
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<TModel> GetEntityWithSpecificChildren(string id, string navigationPropertyPath)
        {
            var result = await _repository.SingleOrDefaultAsyncByInclude(navigationPropertyPath: navigationPropertyPath, e => e.ID == id);
            return result;
        }


        public virtual async Task<TModel> GetEntityWithAllChildren(string id)
        {
            var result = await _repository.GetIncludeAllChildren(id);
            return result;
        }
        public virtual async Task Detach(TModel model)
        {
            await _repository.DetachEntity(model);
        }

        public virtual async Task<IEnumerable<TModel>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<TModel>> GetAll(int skip, int take)
        {
            var result = await _repository.GetAllAsync(skip, take);
            return result;
        }

        public virtual async Task<IEnumerable<TModel>> GetAll(string navigationPropertyPath)
        {
            var result = await _repository.GetAllByInclude(navigationPropertyPath);
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<TModel>> GetAll(string navigationPropertyPath, Expression<Func<TModel, bool>> exp)
        {
            var result = await _repository.WhereByInclude(navigationPropertyPath, exp);
            return result.OrderByDescending(i => i.CreatedDate);
        }


        public virtual async Task<IEnumerable<TViewModel>> WhereAsViewModel(string navigationPropertyPath, Expression<Func<TModel, bool>> exp)
        {
            var entites = await _repository.WhereByInclude(navigationPropertyPath, exp);
            var result = entites.Select(e => _mapper.Map<TViewModel>(e)).ToList();
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<GViewModel>> WhereAsViewModel<GViewModel>(string navigationPropertyPath, Expression<Func<TModel, bool>> exp)
        {
            var entites = await _repository.WhereByInclude(navigationPropertyPath, exp);
            var result = entites.Select(e => _mapper.Map<GViewModel>(e)).ToList();
            return result;
        }
        public virtual async Task<IEnumerable<TViewModel>> WhereAsViewModel(Expression<Func<TModel, bool>> exp)
        {
            var entites = await _repository.GetAllAsync(exp);
            var result = entites.Select(e => _mapper.Map<TViewModel>(e)).ToList();
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<TViewModel>> GetAllAsViewModel()
        {
            var entites = await _repository.GetAllAsync();
            var result = entites.Select(e => _mapper.Map<TViewModel>(e)).ToList();
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<IEnumerable<TViewModel>> GetAllAsViewModel(int skip, int take)
        {
            var entites = await _repository.GetAllAsync(skip, take);
            var result = entites.Select(e => _mapper.Map<TViewModel>(e)).ToList();
            return result.OrderByDescending(i => i.CreatedDate);
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsViewModel(Expression<Func<TModel, bool>> exp)
        {
            var entites = await _repository.GetAllAsync(exp);
            var result = entites.Select(e => _mapper.Map<TViewModel>(e)).ToList();
            return result.OrderByDescending(i => i.CreatedDate);
        }

        public virtual async Task Create(TViewModel viewModel)
        {
            var entity = _mapper.Map<TModel>(viewModel);
            await _repository.InsertAsync(entity);
        }


        public virtual async Task CreateRange(IEnumerable<TViewModel> viewModels)
        {
            var entities = viewModels.Select(item => _mapper.Map<TModel>(item)).ToList();
            await _repository.InsertRange(entities);
        }


        public virtual async Task<IEnumerable<TViewModel>> GetAllByPropertyInfo(PropertyInformation info)
        {
            var entities = await _repository.GetAllByPropertyInfo(info);
            var result = entities.Select(e => _mapper.Map<TViewModel>(e)).ToList();
            return result;
        }

        public virtual async Task Create(TModel model)
        {
            await _repository.InsertAsync(model);
        }



        public virtual async Task<IEnumerable<TModel>> GetAll(Expression<Func<TModel, bool>> exp)
        {
            var result = await _repository.GetAllAsync(exp);
            return result.OrderByDescending(i => i.CreatedDate);
        }
        public virtual async Task<TModel> Get(Expression<Func<TModel, bool>> exp, bool exceptionRaseIfNotExist)
        {
            var result = await _repository.GetEntity(exp, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
            return result;
        }
        public virtual async Task<TModel> Get(string navigationProperties, Expression<Func<TModel, bool>> exp, bool exceptionRaseIfNotExist)
        {
            var result = await _repository.GetEntity(navigationProperties, exp, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
            return result;
        }
        public virtual async Task<TModel> Get(string id, bool exceptionRaseIfNotExist)
        {
            var result = await _repository.GetByIdAsync(id, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
            return result;
        }
        public virtual async Task<TViewModel> GetAsViewModel(string id, bool exceptionRaseIfNotExist)
        {
            var entity = await _repository.GetByIdAsync(id, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
            var model = _mapper.Map<TViewModel>(entity);
            return model;
        }
        public virtual async Task<GViewModel> GetAsViewModel<GViewModel>(string id, bool exceptionRaseIfNotExist) where GViewModel : BaseModel
        {
            var tViewModel = await GetAsViewModel(id, exceptionRaseIfNotExist);
            var model = _mapper.Map<GViewModel>(tViewModel);
            return model;
        }
        public virtual async Task<TViewModel> GetAsViewModel(string navigationProperties, Expression<Func<TModel, bool>> exp, bool exceptionRaseIfNotExist)
        {
            var entity = await _repository.GetEntity(navigationProperties, exp, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
            var model = _mapper.Map<TViewModel>(entity);
            return model;
        }



        public virtual async Task DeleteById(string navigationProperties, string id)
        {
            var entity = await Get(navigationProperties, e => e.ID == id, exceptionRaseIfNotExist: true);
            await Delete(entity);
        }
        public virtual async Task DeleteById(string id)
        {
            await _repository.DeleteWithAllChildren(id);
        }
        public virtual async Task SoftDeleteById(string id, bool exceptionRaseIfNotExist)
        {
            await _repository.SoftDelete(id, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
        }
        public virtual async Task Delete(TModel model)
        {
            await _repository.DeleteAsync(model);
        }


        public virtual async Task Update(TViewModel viewModel)
        {
            var entity = _mapper.Map<TModel>(viewModel);
            await Update(entity);
        }
        public virtual async Task Update(TModel model)
        {
            await _repository.UpdateAsync(model);
        }
        public virtual async Task Update<SourceViewModel>(SourceViewModel sourceViewModel)
        {
            var viewModel = _mapper.Map<TViewModel>(sourceViewModel);
            await Update(viewModel);
        }

        public virtual async Task<IEnumerable<TModel>> Search(string keywords)
        {
            var all = await GetAll();
            var result = await all.Search(keywords);
            return result;
        }


        public virtual async Task<IEnumerable<TModel>> SearchByIncludeNavigationPath(string keywords, string? navigation = null, bool? navigateAll = null)
        {
            IEnumerable<TModel> all = null;
            if (!string.IsNullOrEmpty(navigation))
            {
                all = await GetAllEntitiesWithSpecificChildren(navigation);
            }
            if (navigateAll.HasValue && navigateAll.Value)
            {
                all = await GetAllEntitiesWithAllChildren();
            }
            var result = await all.Search(keywords);
            return result.OrderByDescending(i => i.CreatedDate);
        }

        public virtual async Task<IEnumerable<TViewModel>> SearchAsViewModel(string keywords)
        {
            var all = await GetAllAsViewModel();
            var result = await all.Search(keywords);
            return result.OrderByDescending(i => i.CreatedDate);
        }



        public virtual async Task<int> Count()
        {
            var result = await _repository.Count();
            return result;
        }


    }




    public class EntityService<TModel, TViewModel, TEditViewModel, TCreateViewModel> : EntityService<TModel, TViewModel>
      , ICrudService<TModel, TViewModel, TEditViewModel, TCreateViewModel>
      where TModel : BaseEntity
      where TViewModel : BaseModel
      where TEditViewModel : TViewModel
      where TCreateViewModel : TViewModel
    {
        readonly IMapper _mapper;
        public EntityService(IRepository<TModel> repository
            , IMapper mapper) : base(repository, mapper)
        {
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TModel>> GetLastN(int n)
        {
            var entities = await GetAllEntities();
            var result = entities.TakeLast(n).ToList();
            return result;
        }

        public virtual async Task<IEnumerable<TModel>> Where(Expression<Func<TModel, bool>> where)
        {
            var result = await base.GetAll(exp: where);
            return result;
        }

        public virtual async Task<IEnumerable<TModel>> Where(string navigationProperties, Expression<Func<TModel, bool>> where)
        {
            var result = await base.GetAll(navigationProperties, exp: where);
            return result;
        }




        public virtual async Task<IEnumerable<TModel>> GetLastN<TKey>(int n, Expression<Func<TModel, TKey>> orderBy) where TKey : class
        {
            var entities = await GetAllEntities();
            var result = entities.AsQueryable().OrderBy(orderBy).TakeLast(n).ToList();
            return result;
        }

        public virtual async Task<IEnumerable<TModel>> GetLastN(int n, Expression<Func<TModel, bool>> filter)
        {
            var entities = await GetAllEntities();
            var result = entities.AsQueryable().Where(filter).TakeLast(n).ToList();
            return result;
        }


        public virtual async Task<TModel> GetEntity(string id, bool exceptionRaseIfNotExist)
        {
            return await base.Get(id, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
        }

        public virtual async Task<TModel> GetEntity(string navigationProperties, string id, bool exceptionRaseIfNotExist)
        {
            var result = await base.Get(navigationProperties, i => i.ID == id, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
            return result;
        }


        public virtual async Task CreateNew(TCreateViewModel viewModel)
        {
            await base.Create(viewModel);
        }


        public virtual async Task CreateNew(TModel model)
        {
            await base.Create(model);
        }
        public virtual async Task Delete(string id)
        {
            await base.DeleteById(id);
        }
        public virtual async Task SoftDelete(string id, bool exceptionRaseIfNotExist = true)
        {
            await base.SoftDeleteById(id, exceptionRaseIfNotExist: exceptionRaseIfNotExist);
        }



        public virtual async Task EditEntity(TEditViewModel viewModel)
        {
            await base.Update(viewModel);
        }
        public virtual async Task EditEntity(TModel model)
        {
            await base.Update(model);
        }

        public virtual async Task<IEnumerable<TModel>> GetAllEntities()
        {
            var result = await base.GetAll();
            return result.OrderByDescending(e => e.CreatedDate).ToList();
        }

        public virtual async Task<IEnumerable<TModel>> GetAllEntities(int skip, int take)
        {
            var result = await base.GetAll(skip, take);
            return result.OrderByDescending(e => e.CreatedDate).ToList();
        }

        public virtual async Task<IEnumerable<TModel>> GetAllEntitiesIncludeAllChildren()
        {
            var result = await base.GetAllEntitiesWithAllChildren();
            return result;
        }


        public virtual async Task<IEnumerable<TViewModel>> GetAllEntitiesAsViewModel()
        {
            var result = await base.GetAllAsViewModel();
            return result.ToList();
        }


        public virtual async Task<IEnumerable<TModel>> GetAllEntities(string navigationProperties)
        {
            var result = await base.GetAll(navigationProperties);
            return result.OrderByDescending(e => e.CreatedDate).ToList();
        }

        public virtual async Task<TEditViewModel> GetEntityAsViewModel(string id, bool exceptionRaseIfNotExist)
        {
            var result = await base.GetAsViewModel<TEditViewModel>(id, exceptionRaseIfNotExist);
            return result;
        }

        public virtual async Task<PaginationModel<TModel>> Paginate(int page, int perPage = Pagination.PerPage, string searchKeyWord = null)
        {
            IEnumerable<TModel> resultSearchByCustome = default;
            IEnumerable<TModel> resultSearchByDefault = default;
            IEnumerable<TModel> total = null;

            var skip = (page - 1) * perPage;
            var take = perPage;

            var all = await GetAllEntities(skip, take);
            var count = await Count();
            if (!string.IsNullOrEmpty(searchKeyWord))
            {
                resultSearchByCustome = await Search(searchKeyWord);
                resultSearchByDefault = await all.Search(keyword: searchKeyWord);
                total = List.Merge(resultSearchByCustome, resultSearchByDefault);
                count = total.Distinct().Count();
                total = total.Distinct().Skip(skip).Take(take).ToList();
            }


            var result = new PaginationModel<TModel>
            {
                Entities = (total ?? all),
                Total = count
            };


            return result;
        }


        public virtual async Task<PaginationModel<TViewModel>> PaginateAsViewModel(int page, int perPage = Pagination.PerPage, string searchKeyWord = null)
        {
            IEnumerable<TViewModel> resultSearchByCustome = default;
            IEnumerable<TViewModel> resultSearchByDefault = default;
            IEnumerable<TViewModel> total = null;

            var skip = (page - 1) * perPage;
            var take = perPage;

            var all = await GetAllAsViewModel(skip, take);
            var count = await Count();
            if (!string.IsNullOrEmpty(searchKeyWord))
            {
                resultSearchByCustome = await SearchAsViewModel(searchKeyWord);
                resultSearchByDefault = await all.Search(keyword: searchKeyWord);
                total = List.Merge(resultSearchByCustome, resultSearchByDefault);
                count = total.Distinct().Count();
                total = total.Distinct().Skip(skip).Take(take).ToList();
            }


            var result = new PaginationModel<TViewModel>
            {
                Entities = (total ?? all),
                Total = count
            };


            return result;

        }


        public virtual async Task<PaginationModel<TModel>> PaginateNavigationChildren(int page
            , string? navigation = null
            , bool? navigateAll = null
            , int perPage = Pagination.PerPage
            , string? searchKeyword = null)
        {
            IEnumerable<TModel> resultSearchByCustome = default;
            IEnumerable<TModel> resultSearchByDefault = default;
            IEnumerable<TModel> total = null;
            IEnumerable<TModel> all = null;

            var skip = (page - 1) * perPage;
            var take = perPage;

            if (!string.IsNullOrEmpty(navigation))
            {
                all = await GetAllEntitiesWithSpecificChildren(navigation, skip, take);
            }
            if (navigateAll.HasValue && navigateAll.Value)
            {
                all = await GetAllEntitiesWithAllChildren(skip, take);
            }

            var count = await Count();
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                resultSearchByCustome = await SearchByIncludeNavigationPath(searchKeyword, navigation: navigation, navigateAll: navigateAll);
                resultSearchByDefault = await all.Search(keyword: searchKeyword);
                total = List.Merge(resultSearchByCustome, resultSearchByDefault);
                count = total.Distinct().Count();
                total = total.Distinct().Skip(skip).Take(take).ToList();
            }


            var result = new PaginationModel<TModel>
            {
                Entities = (total ?? all),
                Total = count
            };


            return result;
        }


    }






}
