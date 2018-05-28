using System;
using System.Collections.Generic;

namespace StoreCore.Models
{
    public partial class Ordersku
    {
        public int Id { get; set; }
        public int Skuid { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
    }
}
