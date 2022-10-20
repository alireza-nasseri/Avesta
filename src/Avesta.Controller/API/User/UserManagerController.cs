﻿using Avesta.Auth.User.Service;
using Avesta.Data.Model;
using Avesta.Model.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagerEndPointController = Avesta.Storage.Constant.EndPoints.User.UserManagerController;

namespace Avesta.Controller.API.User
{
    public class UserManagerController<TAvestaUser, TUserViewModel> : AvestaController
        where TAvestaUser : AvestaUser
        where TUserViewModel : UserBaseModel
    {
        readonly IUserService<TAvestaUser> _userService;
        public UserManagerController(IUserService<TAvestaUser> userService)
        {
            _userService = userService;
        }



        [HttpGet]
        [Route(UserManagerEndPointController.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAll();
            return Ok(result);
        }



        [HttpGet]
        [Route(UserManagerEndPointController.GetByEmail)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            return Ok(user);
        }



        [HttpGet]
        [Route(UserManagerEndPointController.GetById)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }



        [HttpPost]
        [Route(UserManagerEndPointController.Edit)]
        public async Task<IActionResult> EditUser(TUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Update(viewModel);
                return Ok(result);
            }
            return BadRequest(viewModel);
        }



        [HttpDelete]
        [Route(UserManagerEndPointController.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }




    }
}
