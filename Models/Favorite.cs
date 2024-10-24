namespace EventsMVC.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventImage { get; set; }
        public int? Price { get; set; }
    }
}
