using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edu_croaker.Data
{
    public class Follower
    {
        public int Id { get; set; }
        public string FollowedUserId { get; set; }
        public string FollowingUserId { get; set; }
    }
}
