using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATestApplication.Models.Document;
using ATestApplication.Models.Email;
using ATestApplication.Models.Phone;

namespace ATestApplication.Models.Customer
{
    public class ClientGetModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public List<EmailGetModel> Emails { get; set; }

        public List<DocumentGetModel> Documents { get; set; }

        public List<PhoneGetModel> Phones { get; set; }

        public DateTime BirthDate { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime ModifiedAt { get; set; }


    }
}
