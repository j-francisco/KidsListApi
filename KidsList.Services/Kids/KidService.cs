using System.Threading.Tasks;
using KidsList.Data;
using KidsList.Services.Exceptions;
using Mapster;

namespace KidsList.Services.Kids
{
    public class KidService : IKidService
    {
        private readonly KidsListContext _context;

        public KidService(KidsListContext context)
        {
            _context = context;
        }

        public async Task<KidDto> AddKidToFamily(int userId, AddKidRequest addKidRequest)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new ValidationException($"User ID {userId} not found.");
            }

            var kid = new Kid { Name = addKidRequest.Name, FamilyId = user.FamilyId };

            _context.Kids.Add(kid);

            await _context.SaveChangesAsync();

            return kid.Adapt<KidDto>();
        }
    }
}
