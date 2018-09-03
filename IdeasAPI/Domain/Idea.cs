using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdeasAPI.Domain
{
    public class Idea
    {
        [Required]
        public string id { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int idUsers { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        public int impact { get; set; }

        [Required]
        public int ease { get; set; }

        [Required]
        public int confidence { get; set; }

        [Required]
        public DateTime created_at { get; set; }

    }
}
