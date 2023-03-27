using System;

namespace Business.Models
{
    public class CreateCustomerModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int DiscountValue { get; set; }
    }
}
