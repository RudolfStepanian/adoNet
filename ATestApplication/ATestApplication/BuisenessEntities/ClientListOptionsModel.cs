using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATestApplication.BuisenessEntities
{
    public class ClientFilterModel
    {
        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public bool IncludePhone { get; set; }

        public bool IncludeEmail { get; set; }

        public bool IncludeDocuments { get; set; }
    }
}
