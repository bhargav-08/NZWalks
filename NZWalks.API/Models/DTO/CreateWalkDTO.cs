using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class CreateWalkDTO
    {
        [Required]
        [MaxLength(100,ErrorMessage ="Name must be less than 100 characters!!")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Name must be less than 100 characters!!")]
        public string Description { get; set; }

        [Required]
        [Range(5,40,ErrorMessage ="Walks must be between 5km to 40km.")]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionID { get; set; }
    }
}
