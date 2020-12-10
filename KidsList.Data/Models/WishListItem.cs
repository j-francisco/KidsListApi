using System.ComponentModel.DataAnnotations;

namespace KidsList.Data
{
    public class WishListItem
    {
        public int Id { get; set; }

        [Required]
        public int WishListId { get; set; }

        public WishList WishList { get; set; }
    }
}