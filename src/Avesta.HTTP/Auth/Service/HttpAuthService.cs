﻿using Avesta.Data.IdentityCore.Model;
using Avesta.Data.Entity.Model;
using Avesta.HTTP.JWT.Service;
using Avesta.Share.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Security.Claims;
using Avesta.Repository.IdentityCore;

namespace Avesta.HTTP.Auth.Service
{
    public interface IHttpAuthService<TAvestaUser>
    {
        Task<TAvestaUser> GetCurrentLoggedUserByCookieAuth();
        Task<TAvestaUser?> GetCurrentLoggedUserByJWTAuth();
        Task<TAvestaUser?> GetCurrentLoggedUserByJWTAuth(string navigationPropertyPath);
        Task<TAvestaUser> GetCurrentLoggedUserFromContext();
        Task<string?> GetBearerTokenFromContext();
    }

    public class HttpAuthService<TId, TAvestaUser, TRole> : IHttpAuthService<TAvestaUser>
        where TId : class, IEquatable<TId>
        where TAvestaUser : AvestaIdentityUser<TId>
        where TRole : IdentityRole
    {
        readonly IIdentityRepository<TId, TAvestaUser, TRole> _identityRepository;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IJWTAuthenticationService _jWTAuthenticationService;
        public HttpAuthService(IHttpContextAccessor httpContextAccessor
            , IIdentityRepository<TId, TAvestaUser, TRole> identityRepository
            , IJWTAuthenticationService jWTAuthenticationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityRepository = identityRepository;
            _jWTAuthenticationService = jWTAuthenticationService;
        }



        /// <summary>
        /// If user login with session or cookie authentication schema 
        /// Then you can get user from http context with this method
        /// </summary>
        /// <returns>Avesta User</returns>
        public async Task<TAvestaUser> GetCurrentLoggedUserByCookieAuth()
        {
            var claimPrinciple = _httpContextAccessor.HttpContext.User;
            var user = await _identityRepository.GetUser(claimPrinciple);
            return user;
        }



        /// <summary>
        /// If user login with jwt authentication schema 
        /// Then you can get user from http context with this method
        /// </summary>
        /// <returns>Avesta User</returns>
        public async Task<TAvestaUser?> GetCurrentLoggedUserByJWTAuth()
        {
            var token = await GetBearerTokenFromContext();
            if (string.IsNullOrEmpty(token))
                return default(TAvestaUser);
            var email = await _jWTAuthenticationService.GetClaimFromToken(token, ClaimTypes.Email);
            var user = await _identityRepository.GetUserByEmail(email);
            return user;
        }

        public async Task<TAvestaUser?> GetCurrentLoggedUserByJWTAuth(string navigationPropertyPath)
        {
            var token = await GetBearerTokenFromContext();
            if (token == null)
                return default(TAvestaUser);
            var email = await _jWTAuthenticationService.GetClaimFromToken(token, ClaimTypes.Email);
            var user = await _identityRepository.GetUserByEmail(email, navigationPropertyPath);
            return user;
        }



        public async Task<TAvestaUser> GetCurrentLoggedUserFromContext()
        {
            throw new NotImplementedException();
        }


        public async Task<string?> GetBearerTokenFromContext()
        {
            var token = await _httpContextAccessor.HttpContext.GetHeaderValue("Authorization", prefix: "Bearer ");
            return token;
        }


    }
}
