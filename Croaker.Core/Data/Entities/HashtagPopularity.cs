using System;
using edu_croaker.Data.Entities;

namespace edu_croaker.Data.Entities
{
    public class HashtagPopularity
    {
        public Hashtag Hashtag { get; set; }

        public int HitCount { get; set; }
    }
}