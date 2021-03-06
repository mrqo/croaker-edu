using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace edu_croaker.Data.Entities
{
    public class Croak : EntityBase
    {        
        public string Author { get; set; }

        public string Content { get; set; }

        public int Shares { get; set; }

        public int Likes { get; set; }

        public List<string> Hashtags { get; set; } = new List<string>();
    }
}