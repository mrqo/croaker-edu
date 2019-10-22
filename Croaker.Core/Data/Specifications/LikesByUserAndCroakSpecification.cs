using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class LikesByUserAndCroakSpecification : BaseSpecification<Like>
    {
        public LikesByUserAndCroakSpecification(string userId, int croakId)
            : base(like => like.UserId.Equals(userId) && like.CroakId.Equals(croakId))
        {
        }
    }
}
