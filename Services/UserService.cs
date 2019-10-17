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

        public async Task<PublicUserData> GetPublicUserData(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser != null)
            {
                var userDetails = await _repo.FindUserDetails(appUser.Id);
                return _mapper.Map(appUser, userDetails);
            }
            return null;
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

        public async Task<IEnumerable<PublicUserData>> GetFollowers(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser == null)
            {
                return new List<PublicUserData>().AsEnumerable();
            }

            var followerIds = await _repo.FindAllFollowers(appUser.Id);

            return await Task.WhenAll(
                followerIds.Select(x => Task.Run(() => _repo.FindUser(x)))
            );
        }

        public async Task<IEnumerable<PublicUserData>> GetFollowedBy(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser == null)
            {
                return new List<PublicUserData>().AsEnumerable();
            }

            var followingIds = await _repo.FindAllFollowedBy(appUser.Id);

            return await Task.WhenAll(
                followingIds.Select(x => Task.Run(() => _repo.FindUser(x)))
            );
        }

        public async Task<int> FollowUser(FollowerDto followerDto)
        {
            var follower = _mapper.Map<Follower>(followerDto);

            await TryFindAndUpdateUser(followerDto.FollowedUserId, user => user.FollowersCount++);
            await TryFindAndUpdateUser(followerDto.FollowingUserId, user => user.FollowedCount++);

            return await _repo.AddFollower(follower);
        }

        public async Task<bool> UnfollowUser(FollowerDto followerDto)
        {
            var follower = _mapper.Map<Follower>(followerDto);

            await TryFindAndUpdateUser(followerDto.FollowedUserId, user => user.FollowersCount--);
            await TryFindAndUpdateUser(followerDto.FollowingUserId, user => user.FollowedCount--);

            return await _repo.RemoveFollower(follower);
        }

        public async Task<bool> IsFollowing(FollowerDto followerDto)
        {
            var follower = await _repo.FindFollower(
                followerDto.FollowedUserId, 
                followerDto.FollowingUserId
            );

            return follower != null;
        }

        protected async Task TryFindAndUpdateUser(string userId, Action<PublicUserData> updater)
        {
            var userWithDetails = await _repo.FindUserWithDetails(userId);

            if (userWithDetails != null)
            {
                updater(userWithDetails);
                await _repo.UpdateUserDetails(userWithDetails);
            }
        }
    }
}