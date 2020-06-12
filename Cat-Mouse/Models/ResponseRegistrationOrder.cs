using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat_Mouse.Models
{
    /// <summary>
    /// Ответ на успешную регистрацию 
    /// </summary>
    public class ResponseRegistrationOrder
    {
        public string orderId { get; set; }
        public string formUrl { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}
