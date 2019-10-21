using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class UserDetailsByUserSpecification : BaseSpecification<UserDetails>
    {
        public UserDetailsByUserSpecification(string userId)
            : base(userDetails => userDetails.UserId.Equals(userId))
        {
        }
    }
}
