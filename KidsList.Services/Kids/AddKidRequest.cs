using System.ComponentModel.DataAnnotations;

namespace KidsList.Services.Kids
{
    public class AddKidRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
