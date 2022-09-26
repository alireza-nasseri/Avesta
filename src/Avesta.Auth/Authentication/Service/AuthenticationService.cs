﻿using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AutoMapper;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Avesta.Avesta.Model.Identity;
using Avesta.Model.Identity;
using Avesta.Auth.JWT.Model;
using Avesta.Data.Model;
using Avesta.Repository.Identity;
using Avesta.Auth.JWT.Service;
using Avesta.Auth.Authentication.ViewModel;
using Avesta.Exceptions.Identity;
using Avesta.Storage.Constant;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Avesta.Auth.Authentication.Service
{
    public interface IAuthenticationService<TAvestaUser>
        where TAvestaUser : AvestaUser
    {
        Task<IdentityReturnTemplate> LoginUser<TLoginUserModel>(TLoginUserModel model) where TLoginUserModel : LoginModelBase;
        Task LogOut();
        Task<IdentityReturnTemplate> RegisterNewUser(RegisterUserViewModel model);
        Task<IdentityReturnTemplate> ResetUserPassword(ResetPasswordViewModel viewModel);
        Task<JWTTokenIdentityResult> SignInWithJWT<TLoginUserModel>(TLoginUserModel model) where TLoginUserModel : LoginModelBase;
        Task<TAvestaUser> GetUserIfUserAuthenticate<TLoginModel>(TLoginModel model) where TLoginModel : LoginModelBase;
        Task<JWTTokenIdentityResult> RefreshJWTTokens(JWTTokenIdentityResult model);

    }




    public class AuthenticationService<TAvestaUser,TRole> : IAuthenticationService<TAvestaUser>
        where TAvestaUser : AvestaUser
        where TRole : IdentityRole
    {
        readonly IIdentityRepository<TAvestaUser, TRole> _identityRepository;
        readonly IJWTAuthenticationService _jWTAuthenticationService;
        readonly IConfiguration _configuration;
        readonly IMapper _mapper;
        public AuthenticationService(
            IIdentityRepository<TAvestaUser, TRole> identityRepository
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


        public async Task<IdentityReturnTemplate> ResetUserPassword(ResetPasswordViewModel viewModel)
        {
            var user = await _identityRepository.GetUser(u => u.PhoneNumber == viewModel.UserPhonenumber, exceptionIfNotExist: true);
            var result = await _identityRepository.ResetUserPassword(user, viewModel.ResetPasswordToken, viewModel.Password);
            return new IdentityReturnTemplate
            {
                Errors = result.Errors.Select(e => e.Description).ToArray(),
                Succeed = result.Succeeded
            };
        }

        public async Task<string> GenerateResetPasswordTokenByPhonenumber(string phoneNumber)
        {
            var user = await _identityRepository.GetUser(u => u.PhoneNumber == phoneNumber, exceptionIfNotExist: true);
            var token = await _identityRepository.GenerateResetPasswordToken(user);
            return token;
        }

        public async Task<IdentityReturnTemplate> RegisterNewUser(RegisterUserViewModel model)
        {
            var user = _mapper.Map<TAvestaUser>(model);
            var result = await _identityRepository.RegisterNewUser(user, model.Password);
            return new IdentityReturnTemplate
            {
                Errors = result.Errors?.Select(e => e.Description).ToArray(),
                Succeed = result.Succeeded
            };
        }

        public async Task<IdentityReturnTemplate> LoginUser<TLoginModel>(TLoginModel model) where TLoginModel : LoginModelBase
        {
            model.ID = await _identityRepository.GetUserIDByEmail(model.Email);
            var result = await _identityRepository.SignIn<LoginModelBase>(model, isPersistent: model.RememberMe);
            if (!result.Succeed)
                throw new CanNotFoundAnyUserWithThisUsernameAndPassword($"status of signin result : {result.Succeed}");
            return new IdentityReturnTemplate
            {
                Errors = result.Errors,
                Succeed = result.Succeed
            };
        }

        public async Task<TAvestaUser> GetUserIfUserAuthenticate<TLoginModel>(TLoginModel model) where TLoginModel : LoginModelBase
        {
            var user = await _identityRepository.GetUserByEmail(model.Email);
            model.ID = user.Id;
            var result = await _identityRepository.SignIn<LoginModelBase>(model, isPersistent: model.RememberMe);
            if (!result.Succeed)
                throw new CanNotFoundAnyUserWithThisUsernameAndPassword($"status of signin result : {result.Succeed}");

            return user;

        }

        public async Task<JWTTokenIdentityResult> SignInWithJWT<TLoginUserModel>(TLoginUserModel model) where TLoginUserModel : LoginModelBase
        {
            var user = await GetUserIfUserAuthenticate(model);
            if (user == null)
                throw new IdentityException(msg: "", code: ExceptionConstant.IdentityException);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email)
            };
            var result = await _jWTAuthenticationService.GenerateToken(claims);
            user.RefreshToken = result.RefreshToken;
            await _identityRepository.UpdateUser(user);

            return result;

        }

        public async Task<JWTTokenIdentityResult> RefreshJWTTokens(JWTTokenIdentityResult model)
        {
            var result = await _jWTAuthenticationService.ReinitialIdentityJWTTokens(model);
            return result;
        }

        public async Task<IdentityReturnTemplate> RegisterNewUser<TRegisterUserModel>(TRegisterUserModel model) where TRegisterUserModel : RegisterModelBase
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