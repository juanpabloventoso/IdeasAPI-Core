using System;
using System.ComponentModel.DataAnnotations;

namespace IdeasAPI.Domain
{
    public class User
    {

        [Required]
        public int id { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string password { get; set; }

        public string refresh_token { get; set; }

    }
}
