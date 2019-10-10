using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edu_croaker.Dtos
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
