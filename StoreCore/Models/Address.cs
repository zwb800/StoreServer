using System;
using System.Collections.Generic;

namespace StoreCore.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string DetailInfo { get; set; }
        public string TelNumber { get; set; }
        public int? UserId { get; set; }
    }
}
