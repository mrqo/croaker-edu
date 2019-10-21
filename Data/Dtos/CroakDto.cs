using System;
using System.Collections.Generic;

namespace edu_croaker.Data.Dtos
{
    public class CroakDto
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }
        
        public string Content { get; set; }

        public string AuthorName { get; set; }

        public int SharesCount { get; set; }

        public int LikesCount { get; set; }

        public List<string> Hashtags { get; set; } = new List<string>();
    }
}
