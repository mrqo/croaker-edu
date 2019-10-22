using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class FollowersSpecification : BaseSpecification<Follower>
    {
        public FollowersSpecification(string followedUserId)
            : base(x => x.FollowedUserId.Equals(followedUserId))
        {
        }
    }
}
