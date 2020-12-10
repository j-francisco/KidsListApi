using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KidsList.Data
{
    public class WishList
    {
        public WishList()
        {
            if (WishListItems == null)
            {
                WishListItems = new List<WishListItem>();
            }
        }

        public int Id { get; set; }

        [Required]
        public int KidId { get; set; }

        public Kid Kid { get; set; }

        public List<WishListItem> WishListItems { get; set; }
    }
}