using System.Collections.Generic;

namespace Cat_Mouse.Models.ViewModels
{
    public class RegistrationOrderViewModel
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public IEnumerable<RegistrationOrder> RegistrationOrders { get; set; }
    }
}
