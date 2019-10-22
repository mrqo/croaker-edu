using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Specifications
{
    public class HashtagsByCaptionsSpecification : BaseSpecification<Hashtag>
    {
        public HashtagsByCaptionsSpecification(IEnumerable<string> captions)
            : base(hashtag => captions.Contains(hashtag.Caption))
        {
        }
    }
}
