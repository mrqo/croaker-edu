using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Models;
using edu_croaker.DataAccess;
using edu_croaker.Data;
using edu_croaker.Dtos;

namespace edu_croaker.Services
{
    public class UserService
    {
        private readonly IRepository _repo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> Register(UserRegisterDto user)
        {
            var appUser = new ApplicationUser()
            {
                UserName = user.Username,
                Email = user.Email,
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(appUser, false);
            }

            return result.Succeeded;
        }

        public async Task<bool> Login(UserLoginDto user)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }

        public async Task<PublicUserData> GetPublicUserData(int userId)
        {
            return new PublicUserData()
            {
                UserId = 0,
                Username = "marek_123",
                PostsCount = 100,
                LikesCount = 32,
                SharesCount = 40
            };
        }

        public async Task<int> GetCurrentUserId()
        {
            return 0;
        }
    }
}