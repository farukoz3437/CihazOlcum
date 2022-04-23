using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Common
{
    public class CommonFeature
    {
        public bool? IsDelete { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteIp { get; set; }
    }
}
