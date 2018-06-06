using System;
using System.Collections.Generic;

namespace StoreCore.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public int AddressId { get; set; }
        public int Status { get; set; }
    }
}
