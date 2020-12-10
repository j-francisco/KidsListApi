using System.Collections.Generic;

namespace KidsList.Data
{
    public class Family
    {
        public Family()
        {
            if (Users == null)
            {
                Users = new List<User>();
            }

            if (Kids == null)
            {
                Kids = new List<Kid>();
            }
        }

        public int Id { get; set; }

        public List<User> Users { get; set; }

        public List<Kid> Kids { get; set; }
    }
}