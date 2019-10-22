using System;

namespace edu_croaker.Data.Dtos 
{
    public class PublicUserData
    {
        public string UserId { get; set; }
        
        public string Username { get; set; }

        public string DisplayedName { get; set; }

        public string Avatar { get; set; }

        public string Bio { get; set; }

        public int PostsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowedCount { get; set; }

        public int LikesCount { get; set; }

        public int SharesCount { get; set; }
    }
}