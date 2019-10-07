using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using edu_croaker.DataAccess;
using edu_croaker.Dtos;

namespace edu_croaker.Data
{
    public class UserService
    {
        protected readonly IRepository Repo;

        public UserService(IRepository repo)
        {
            Repo = repo;
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