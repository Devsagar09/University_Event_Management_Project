using System.ComponentModel.DataAnnotations;

namespace EventsMVC.Models
{
    public class EventDTO
    {
        [Required(ErrorMessage = "Event Name is required.")]
        public string? EventName { get; set; }

        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Event Description is required.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Event Date is required.")]
        public string? Date { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }
    }
}
