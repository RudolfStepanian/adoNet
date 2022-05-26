using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATestApplication.Models.Customer
{
    public class ClientList
    {
        public long? TotalCount { get; set; }

        public long? PageNumber { get; set; }

        public long? PageSize { get; set; }

        public List<ClientGetModel> Items { get; set; }
    }
}
