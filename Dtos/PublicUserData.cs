using System;

namespace edu_croaker.Dtos 
{
    public class PublicUserData
    {
        public int UserId { get; set; }
        
        public string Username { get; set; }

        public int PostsCount { get; set; }

        public int LikesCount { get; set; }

        public int SharesCount { get; set; }
    }
}