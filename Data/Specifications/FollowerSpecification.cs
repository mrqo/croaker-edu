using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class FollowerSpecification : BaseSpecification<Follower>
    {
        public FollowerSpecification(string followedUserId, string followingUserId)
            : base(follower => follower.FollowedUserId.Equals(followedUserId) && follower.FollowingUserId.Equals(followingUserId))
        {
        }
    }
}
