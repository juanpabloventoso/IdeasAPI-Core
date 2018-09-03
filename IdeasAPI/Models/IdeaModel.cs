using System.ComponentModel.DataAnnotations;

namespace IdeasAPI.Models
{
    public class IdeaModel
    {

        public string id { get; set; }

        [Required]
        [MaxLength(255)]
        public string content { get; set; }

        [Required]
        [Range(1, 10)]
        public int impact { get; set; }

        [Required]
        [Range(1, 10)]
        public int ease { get; set; }

        [Required]
        [Range(1, 10)]
        public int confidence { get; set; }

        public float average_score { get; set; }

        public long created_at { get; set; }

    }
}
