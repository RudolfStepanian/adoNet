﻿using ATestApplication.Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ATestApplication.Models.Phone
{
    public class PhoneGetModel
    {
        [Key]
        public long Id { get; set; }

        public string Number { get; set; }

        public string Operator { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
