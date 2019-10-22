using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace edu_croaker.Data.Entities
{
    public class Like : EntityBase
    {
        public int CroakId { get; set; }

        public string UserId { get; set; }
    }
}
