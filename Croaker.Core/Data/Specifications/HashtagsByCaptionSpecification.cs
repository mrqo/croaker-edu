using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class HashtagsByCaptionSpecification : BaseSpecification<Hashtag>
    {
        public HashtagsByCaptionSpecification(string caption)
            : base(hashtag => hashtag.Caption.Equals(caption))
        {
        }
    }
}
