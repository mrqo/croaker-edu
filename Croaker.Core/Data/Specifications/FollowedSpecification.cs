using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class FollowedSpecification : BaseSpecification<Follower>
    {
        public FollowedSpecification(string followingUserId)
            : base(x => x.FollowingUserId.Equals(followingUserId))
        {
        }
    }
}
