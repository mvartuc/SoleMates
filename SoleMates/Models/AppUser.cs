﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoleMates.Models
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }
        public int? Milage { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }
    }
}
