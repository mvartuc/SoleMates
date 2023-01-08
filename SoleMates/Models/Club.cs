using SoleMates.Data.Enum;
using SoleMates.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoleMates.Models
{
    public class Club : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ClubCategory ClubCategory { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
