using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edu_croaker.Data
{
    public class UserDetails
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string DisplayedName { get; set; }

        public string Avatar { get; set; }

        public string Bio { get; set; }

        public int PostsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowedCount { get; set; }
    }
}
