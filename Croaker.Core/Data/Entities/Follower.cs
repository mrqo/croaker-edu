using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edu_croaker.Data.Entities
{
    public class Follower : EntityBase
    {
        public string FollowedUserId { get; set; }
        public string FollowingUserId { get; set; }
    }
}
