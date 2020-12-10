using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KidsList.Data
{
    public class User
    {
        public int Id { get; set;}

        [Required]
        [Column(TypeName = "citext")]
        public string Email { get; set; }

        public string FullName { get; set; }

        [Required]
        public int FamilyId { get; set; }

        public Family Family { get; set; }
    }
}