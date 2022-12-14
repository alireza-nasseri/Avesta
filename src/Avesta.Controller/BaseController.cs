using Avesta.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Avesta.Storage.Constant;
using System;

namespace Avesta.Controller
{



    public interface IBaseController
    {

        Task<IActionResult> Search(string keyword);
        Task<IActionResult> Paginate(int? page = null, string keyword = null);
    }
    public abstract class BaseController<T> : AvestaController
        where T : class
    {
        readonly IBaseService<T> _baseService;
        public BaseController(
            IBaseService<T> baseService)
        {
            _baseService = baseService;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual async Task<IActionResult> Paginate(int? page = null, string viewName = null, string keyword = null)
        {
            var model = await _baseService.Paginate(page, searchKeyWord: keyword);
            var entities = model.Entities;
            TempData["EntityCount"] = model.Total;
            return View(viewName, entities);
        }

    }


    public abstract class BaseAPIController<T> : AvestaController
        where T : class
    {
        readonly IBaseService<T> _baseService;
        public BaseAPIController(
            IBaseService<T> baseService)
        {
            _baseService = baseService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route(BaseController.Paginate)]
        public virtual async Task<IActionResult> Paginate(int? page = null, int perPage = Pagination.PerPage, string? keyword = null)
        {
            var result = await _baseService.Paginate(page, perPage: perPage, searchKeyWord: keyword);
            return Ok(result);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [Route(BaseController.PaginateNavigationChildren)]
        public virtual async Task<IActionResult> PaginateNavigationChildren(int? page = null, string? navigation = null, bool? navigationAll = null
            , int perPage = Pagination.PerPage
            , string? keyword = null
            , string? dynamicQuery = null
            , DateTime? startDate = null
            , DateTime? endDate = null)
        {
            var result = await _baseService.PaginateNavigationChildren(page, navigation: navigation, navigateAll: navigationAll
                , perPage: perPage
                , searchKeyWord: keyword
                , dynamicQuery: dynamicQuery
                , startDate: startDate
                , endDate: endDate);
            return Ok(result);
        }

    }



    public abstract class BaseAPIController<T, TViewModel> : BaseAPIController<T>
        where T : class
        where TViewModel : class
    {
        readonly IBaseService<T, TViewModel> _baseService;
        public BaseAPIController(
            IBaseService<T, TViewModel> baseService) : base(baseService)
        {
            _baseService = baseService;
        }



        [ApiExplorerSettings(IgnoreApi = true)]
        [Route(BaseController.PaginateAsViewModel)]
        public virtual async Task<IActionResult> PaginateAsViewModel(int? page = null, int perPage = Pagination.PerPage, string? keyword = null
            , DateTime? startDate = null
            , DateTime? endDate = null)
        {
            var result = await _baseService.PaginateAsViewModel(page, perPage: perPage, searchKeyWord: keyword, startDate: startDate, endDate: endDate);
            return Ok(result);
        }
    }




}
