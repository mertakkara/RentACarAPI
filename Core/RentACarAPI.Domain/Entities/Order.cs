using RentACarAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Domain.Entities
{
    public class Order:BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string Descirption { get; set; }
        public string Address { get; set; }
        public ICollection<Car> Cars { get; set; }

        public Customer Customer { get; set; }
    }
}
