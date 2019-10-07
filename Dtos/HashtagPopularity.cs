using System;
using edu_croaker.Data;

namespace edu_croaker.Dtos
{
    public class HashtagPopularity
    {
        public Hashtag Hashtag { get; set; }

        public int HitCount { get; set; }
    }
}