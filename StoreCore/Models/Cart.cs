using System;
using System.Collections.Generic;

namespace StoreCore.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Skuid { get; set; }
        public int Count { get; set; }
    }
}
