using System;

namespace Ordering.API.DTOs
{
    public class OrderSummaryDTO
    {
        public int OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
    }
}