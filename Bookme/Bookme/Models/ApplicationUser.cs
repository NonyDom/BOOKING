using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookme.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }   
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public int? GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual CommonDropDown? Gender { get; set;}
        public string? Address { get; set;}
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public decimal? Price { get; set; }
        public string? MusicSpecialization { set; get; }
        public string? Image { set; get; }
        public DateTime DateCreated { get; set; }
        public bool IsDeactivated { get; set; }
        public bool? IsAvailable { get; set; }


    }
}
