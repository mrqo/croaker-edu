using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class LikesByCroakSpecification : BaseSpecification<Like>
    {
        public LikesByCroakSpecification(int croakId)
            : base(like => like.CroakId.Equals(croakId))
        {
        }
    }
}
