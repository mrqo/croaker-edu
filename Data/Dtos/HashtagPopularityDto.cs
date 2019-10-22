using System;
using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Dtos
{
    public class HashtagPopularityDto
    {
        public string Caption { get; set; }

        public int HitCount { get; set; }
    }
}