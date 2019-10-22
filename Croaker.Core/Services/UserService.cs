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

using edu_croaker.Data.Dtos;
using edu_croaker.Data.Entities;
using edu_croaker.Data.Interfaces;
using edu_croaker.Data.Specifications;


namespace edu_croaker.Services
{
    public class UserService
    {
        private readonly IRepository<Follower> _followersRepo;
        private readonly IRepository<UserDetails> _userDetailsRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            IRepository<Follower> followersRepo,
            IRepository<UserDetails> userDetailsRepo,
            IMapper mapper,
            UserManager<ApplicationUser> userManager
        )
        {
            _followersRepo = followersRepo;
            _userDetailsRepo = userDetailsRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PublicUserData> GetPublicUserData(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser != null)
            {
                var userDetails = _userDetailsRepo.Get(new UserDetailsByUserSpecification(appUser.Id));
                return _mapper.Map(appUser, _mapper.Map<PublicUserData>(userDetails));
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

            var followerIds = _followersRepo
                .List(new FollowersSpecification(appUser.Id))
                .Select(x => x.FollowingUserId);

            return await Task.WhenAll(
                followerIds.Select(async x => 
                    _mapper.Map<PublicUserData>(await _userManager.FindByIdAsync(x))
                )
            );
        }

        public async Task<IEnumerable<PublicUserData>> GetFollowedBy(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser == null)
            {
                return new List<PublicUserData>().AsEnumerable();
            }

            var followingIds = _followersRepo
                .List(new FollowedSpecification(appUser.Id))
                .Select(x => x.FollowedUserId);

            return await Task.WhenAll(
                followingIds.Select(async x => 
                    _mapper.Map<PublicUserData>(await _userManager.FindByIdAsync(x))
                )
            );
        }

        public int FollowUser(FollowerDto followerDto)
        {
            var follower = _mapper.Map<Follower>(followerDto);

            TryFindAndUpdateUser(followerDto.FollowedUserId, user => user.FollowersCount++);
            TryFindAndUpdateUser(followerDto.FollowingUserId, user => user.FollowedCount++);

            return _followersRepo.Add(follower);
        }

        public bool UnfollowUser(FollowerDto followerDto)
        {
            var follower = _followersRepo.Get(
                new FollowerSpecification(
                    followerDto.FollowedUserId, 
                    followerDto.FollowingUserId
                )
            );

            TryFindAndUpdateUser(followerDto.FollowedUserId, user => user.FollowersCount--);
            TryFindAndUpdateUser(followerDto.FollowingUserId, user => user.FollowedCount--);

            _followersRepo.Delete(follower);
            return true;
        }

        public bool IsFollowing(FollowerDto followerDto)
        {
            var follower = _followersRepo.Get(
                new FollowerSpecification(
                    followerDto.FollowedUserId,
                    followerDto.FollowingUserId
                )
            );

            return follower != null;
        }

        protected bool TryFindAndUpdateUser(string userId, Action<UserDetails> updater)
        {
            var userWithDetails = _userDetailsRepo.Get(new UserDetailsByUserSpecification(userId));

            if (userWithDetails == null)
            {
                return false;
            }

            updater(userWithDetails);
            _userDetailsRepo.Edit(userWithDetails);
            return true;
        }
    }
}