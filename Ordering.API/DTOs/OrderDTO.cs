using System;
using System.Collections.Generic;

namespace Ordering.API.DTOs
{
    public class OrderDTO
    {
        public int OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public decimal Total { get; set; }
    }
}