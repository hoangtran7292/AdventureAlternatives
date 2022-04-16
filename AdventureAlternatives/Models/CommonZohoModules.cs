using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureAlternatives.Models
{
    public class CommonZohoModules
    {
        public class Owner
        {
            public string name { get; set; }
            public string id { get; set; }
            public string email { get; set; }
        }

        public class Modified_By
        {
            public string name { get; set; }
            public string id { get; set; }
            public string email { get; set; }
        }

        public class Created_By
        {
            public string name { get; set; }
            public string id { get; set; }
            public string email { get; set; }
        }

        public class Parent_Id
        {
            public string name { get; set; }
            public string id { get; set; }
        }
    }
}
