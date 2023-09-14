using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueTrigger.Model
{
    
        public class FileData
        {
            public string? ImageName { get; set; }
            public string ImageExtension { get; set; }
            public DateTime FileCreated { get; set; }
        public int UserId { get; set; }
        }
    
}
