using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATestApplication.Models
{
    public class ErrorResponseModel<T> : ResponseModel<T>
    {
        public ErrorModel Error { get; set; }
    }
}
