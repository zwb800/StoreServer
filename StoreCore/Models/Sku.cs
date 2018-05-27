using System;
using System.Collections.Generic;

namespace StoreCore.Models
{
    public partial class Sku
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint ProductId { get; set; }
    }
}
