using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class CroaksByAuthorSpecification : BaseSpecification<Croak>
    {
        public CroaksByAuthorSpecification(string authorId)
            : base(croak => croak.Author.Equals(authorId))
        {
        }
    }
}
