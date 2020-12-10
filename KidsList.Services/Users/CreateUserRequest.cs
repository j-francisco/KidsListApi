using System.ComponentModel.DataAnnotations;

namespace KidsList.Services.Users
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [StringLength(320)]
        public string Email { get; set; }
    }
}