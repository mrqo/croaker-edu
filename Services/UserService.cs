using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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

        public Task<PublicUserData> GetPublicUserData(string userName)
        {
            return Task.Run(async () =>
            {
                var appUser = await _userManager.FindByNameAsync(userName);

                if (appUser != null)
                {
                    return _mapper.Map<PublicUserData>(appUser);
                }
                return null;
            });
        }

        public Task<IEnumerable<PublicUserData>> GetUsersToDiscover(string userName)
        {
            return Task.Run(() =>
            {
                return new List<PublicUserData>()
                {
                    new PublicUserData()
                    {
                        UserId = "1",
                        Username = "Janek32"
                    },
                    new PublicUserData()
                    {
                        UserId = "2",
                        Username = "__DEV__"
                    },
                    new PublicUserData()
                    {
                        UserId = "3",
                        Username = "koszmar"
                    }
                }
                .AsEnumerable();
            });
        }

        public Task<int> FollowUser(FollowerDto followerDto)
        {
            var follower = _mapper.Map<Follower>(followerDto);
            return _repo.AddFollower(follower);
        }
    }
}