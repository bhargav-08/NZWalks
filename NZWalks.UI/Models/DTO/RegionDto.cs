using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Code must be of minimum 3 character!!")]
        [MaxLength(3, ErrorMessage = "Code must be of maximum 3 character!!")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name must be less than 100 characters!!")]
        public string Name { get; set; }

        [DisplayName("Image Url")]
        public string? RegionImageUrl { get; set; }
    }
}
