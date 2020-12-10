using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KidsList.Data
{
    public class Kid
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int FamilyId { get; set; }

        public Family Family { get; set; }

    }
}