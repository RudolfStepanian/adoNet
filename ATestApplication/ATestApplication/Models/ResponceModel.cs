using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATestApplication.Models
{
    public abstract class ResponseModel<T>
    {
        public T Data { get; set; }
    }
}
