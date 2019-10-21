
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using edu_croaker.Data.Entities;
using edu_croaker.Data.Dtos;

namespace edu_croaker.Data.Interfaces
{
    interface IHashtagRepository
    {
        IEnumerable<HashtagPopularity> ListPopular(int maxCount);
    }
}
