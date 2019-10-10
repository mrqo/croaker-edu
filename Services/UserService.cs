using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Models;
using AutoMapper;
using edu_croaker.DataAccess;
using edu_croaker.Data;
using edu_croaker.Dtos;

namespace edu_croaker.Services
{
    public class UserService
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            IRepository repo, 
            IMapper mapper,
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager
        )
        {
            _repo = repo;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<PublicUserData> GetPublicUserData(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser != null)
            {
                return _mapper.Map<PublicUserData>(appUser);
            }
            return null;
        }
    }
}