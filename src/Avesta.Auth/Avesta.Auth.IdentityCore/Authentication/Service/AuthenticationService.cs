﻿using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Avesta.Auth.IdentityCore.Authentication.ViewModel;
using Avesta.Exceptions.Identity;
using Avesta.Constant;
using Avesta.Share.Extensions;
using Avesta.Share.Model.Identity;
using Avesta.HTTP.JWT.Model;
using Avesta.HTTP.JWT.Service;
using Avesta.Data.IdentityCore.Model;
using Avesta.Repository.IdentityCore;

namespace Avesta.Auth.IdentityCore.Authentication.Service
{
    public interface IAuthenticationService<TId, TAvestaUser>
        where TId : class, IEquatable<TId>
        where TAvestaUser : AvestaIdentityUser<TId>
    {
        Task<IdentityReturnTemplate> Login<TLoginUserModel>(TLoginUserModel model) where TLoginUserModel : LoginModelBase<TId>;
        Task<IdentityReturnTemplate> Register<IRegisterUserViewModel>(IRegisterUserViewModel model) where IRegisterUserViewModel : RegisterUserViewModel<TId>;
        Task<IdentityReturnTemplate> ResetPasswordByPhoneNumber(ResetPasswordViewModel viewModel);
        Task<IdentityReturnTemplate> ResetPasswordByEmail(ResetPasswordViewModel viewModel);
        Task<JWTAvestaUser<TId>?> SignInWithJWT<TLoginUserModel>(TLoginUserModel model) where TLoginUserModel : LoginModelBase<TId>;
        Task<JWTAvestaUser<TId>?> SignInWithJWT<TLoginUserModel>(TLoginUserModel model, params Claim[] claims) where TLoginUserModel : LoginModelBase<TId>;
        Task<TAvestaUser> GetAuthenticatedUser<TLoginModel>(TLoginModel model) where TLoginModel : LoginModelBase<TId>;
        Task<JWTTokenIdentityResult> RefreshJWTTokens(JWTTokenIdentityResult model);
        Task<string> GenerateResetPasswordTokenByEmail(string email);
        Task<JWTAvestaUser<TId>?> GetUserByToken(string token);
        Task LogOut();

    }




    public class AuthenticationService<TId, TAvestaUser, TRole> : IAuthenticationService<TId,TAvestaUser>
        where TId : class, IEquatable<TId>
        where TAvestaUser : AvestaIdentityUser<TId>
        where TRole : IdentityRole
    {
        readonly IIdentityRepository<TId,TAvestaUser, TRole> _identityRepository;
        readonly IJWTAuthenticationService _jWTAuthenticationService;
        readonly IConfiguration _configuration;
        readonly IMapper _mapper;
        public AuthenticationService(
            IIdentityRepository<TId, TAvestaUser, TRole> identityRepository
            , IJWTAuthenticationService jWTAuthenticationService
            , IConfiguration configuration
            , IMapper mapper)
        {
            _jWTAuthenticationService = jWTAuthenticationService;
            _configuration = configuration;
            _identityRepository = identityRepository;
            _mapper = mapper;
            _identityRepository = identityRepository;
        }


        public async Task<JWTAvestaUser<TId>?> GetUserByToken(string token)
        {
            var email = await _jWTAuthenticationService.GetClaimFromToken(token, ClaimTypes.Email);
            var user = await _identityRepository.GetUserByEmail(email);
            var data = user.Convert<JWTAvestaUser<TId>>()?.SetToken(token);
            return data;
        }


        public async Task<IdentityReturnTemplate> ResetPasswordByPhoneNumber(ResetPasswordViewModel viewModel)
        {
            var user = await _identityRepository.GetUser(u => u.PhoneNumber == viewModel.UserPhonenumber, exceptionRaiseIfNotExist: true);
            var result = await _identityRepository.ResetUserPassword(user, viewModel.ResetPasswordToken, viewModel.Password);
            return new IdentityReturnTemplate
            {
                Errors = result.Errors.Select(e => e.Description).ToArray(),
                Succeed = result.Succeeded
            };
        }

        public async Task<IdentityReturnTemplate> ResetPasswordByEmail(ResetPasswordViewModel viewModel)
        {
            var user = await _identityRepository.GetUser(u => u.Email == viewModel.Email, exceptionRaiseIfNotExist: true);
            var result = await _identityRepository.ResetUserPassword(user, viewModel.ResetPasswordToken, viewModel.Password);
            return new IdentityReturnTemplate
            {
                Errors = result.Errors.Select(e => e.Description).ToArray(),
                Succeed = result.Succeeded
            };
        }

        public async Task<string> GenerateResetPasswordTokenByPhonenumber(string phoneNumber)
        {
            var user = await _identityRepository.GetUser(u => u.PhoneNumber == phoneNumber, exceptionRaiseIfNotExist: true);
            var token = await _identityRepository.GenerateResetPasswordToken(user);
            return token;
        }



        public async Task<string> GenerateResetPasswordTokenByEmail(string email)
        {
            var user = await _identityRepository.GetUserByEmail(email);
            var token = await _identityRepository.GenerateResetPasswordToken(user);
            return token;
        }



        public async Task<IdentityReturnTemplate> Register<IRegisterUserViewModel>(IRegisterUserViewModel model)
            where IRegisterUserViewModel : RegisterUserViewModel<TId>
        {
            var user = _mapper.Map<TAvestaUser>(model);
            var result = await _identityRepository.RegisterNewUser(user, model.RepeatPassword);
            return new IdentityReturnTemplate
            {
                Errors = result.Errors?.Select(e => e.Description).ToArray(),
                Succeed = result.Succeeded
            };
        }

        public async Task<IdentityReturnTemplate> Login<TLoginModel>(TLoginModel model) where TLoginModel : LoginModelBase<TId>
        {
            model.ID = await _identityRepository.GetUserIDByEmail(model.Email);
            var result = await _identityRepository.SignIn<LoginModelBase<TId>>(model, isPersistent: model.RememberMe);
            if (!result.Succeed)
                throw new CanNotFoundAnyUserWithThisUsernameAndPassword($"status of signin result : {result.Succeed}");
            return new IdentityReturnTemplate
            {
                Errors = result.Errors,
                Succeed = result.Succeed
            };
        }

        public async Task<TAvestaUser> GetAuthenticatedUser<TLoginModel>(TLoginModel model) where TLoginModel : LoginModelBase<TId>
        {
            var user = await _identityRepository.GetUserByEmail(model.Email);
            if (user == null)
                throw new UserNotFoundException($"user with email or id : {model.Email} not found !");
            model.ID = user.Id;
            var result = await _identityRepository.SignIn<LoginModelBase<TId>>(model, isPersistent: model.RememberMe);
            if (!result.Succeed)
                throw new CanNotFoundAnyUserWithThisUsernameAndPassword($"status of signin result : {result.Succeed}");

            return user;

        }








        public async Task<JWTAvestaUser<TId>?> SignInWithJWT<TLoginUserModel>(TLoginUserModel model) where TLoginUserModel : LoginModelBase<TId>
        {
            var user = await GetAuthenticatedUser(model);
            if (user == null)
                throw new IdentityException(msg: "", code: ExceptionConstant.IdentityException);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email)
            };
            var result = await _jWTAuthenticationService.GenerateToken(claims);
            user.RefreshToken = result.RefreshToken;
            await _identityRepository.UpdateUser(user);

            var data = user.Convert<JWTAvestaUser<TId>>()?.SetToken(result.Token);
            return data;

        }

        public async Task<JWTAvestaUser<TId>?> SignInWithJWT<TLoginUserModel>(TLoginUserModel model, params Claim[] claims) where TLoginUserModel : LoginModelBase<TId>
        {
            var user = await GetAuthenticatedUser(model);
            if (user == null)
                throw new IdentityException(msg: "", code: ExceptionConstant.IdentityException);

            var localClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email)
            };
            localClaims.AddRange(claims);

            var result = await _jWTAuthenticationService.GenerateToken(localClaims);
            user.RefreshToken = result.RefreshToken;
            await _identityRepository.UpdateUser(user);

            var data = user.Convert<JWTAvestaUser<TId>>()?.SetToken(result.Token);
            return data;

        }



        public async Task<JWTTokenIdentityResult> RefreshJWTTokens(JWTTokenIdentityResult model)
        {
            var result = await _jWTAuthenticationService.ReinitialIdentityJWTTokens(model);
            return result;
        }

        public async Task<IdentityReturnTemplate> RegisterNewUser<TRegisterUserModel>(TRegisterUserModel model) where TRegisterUserModel : RegisterModelBase<TId>
        {
            //var result = await _identityRepository.RegisterUser(model, Share.Constant.Role.NormalUser);
            var result = await _identityRepository.RegisterUser(model);
            return new IdentityReturnTemplate
            {
                Errors = result.Errors,
                Succeed = result.Succeed
            };
        }

        public async Task LogOut()
        {
            await _identityRepository.SignOut();
        }



    }




}
