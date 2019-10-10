using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edu_croaker.Dtos
{
    public class FollowerDto
    {
        public string FollowedUserId { get; set; }
        public string FollowingUserId { get; set; }
    }
}
