namespace Ordering.API.DTOs
{
    public class OrderItemDTO
    {
        public string ProductName { get; set; }
        public int Units { get; set; }
        public double UnitPrice { get; set; }
        public string PictureUrl { get; set; }
    }
}
