using Bookme.Models;

namespace Bookme.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
