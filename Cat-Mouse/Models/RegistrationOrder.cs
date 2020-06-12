using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat_Mouse.Models
{
    public class RegistrationOrder
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "The amount must be greater than 0")]
        public int Amount { get; set; }
        public string OrderNumber { get; set; }
        public string ReturnUrl { get; set; }
        public string FailUrl { get; set; }
        public string Description { get; set; }
        public string OrderId { get; set; }
    }
}
