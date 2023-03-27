using System.Collections.Generic;

namespace Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int DiscountValue { get; set; }
        public Person Person { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}
