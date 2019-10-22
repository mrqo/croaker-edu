using System;
using System.Collections.Generic;

namespace edu_croaker.Data.Entities
{
    public class Hashtag : EntityBase
    {
        public string Caption { get; set; }

        public List<int> CroakIds { get; set; } = new List<int>(); 
    }
}