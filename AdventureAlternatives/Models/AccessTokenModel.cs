using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureAlternatives.Models
{
    public class AccessTokenModel
    {
        public string AccessToken { get; set; }
        public DateTime? ExpiredTime { get; set; }
    }
}
