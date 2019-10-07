using System;
using System.Collections.Generic;

namespace edu_croaker.Data
{
    public class Hashtag
    {
        public int Id { get; set; }

        public string Caption { get; set; }

        public List<int> CroakIds { get; set; } = new List<int>(); 
    }
}