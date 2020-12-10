using System;
using System.Linq;
using System.Threading.Tasks;
using KidsList.Data;
using KidsList.Services.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace KidsList.Services.Users
{
    public class UserService : IUserService
    {
        private readonly KidsListContext _context;

        public UserService(KidsListContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetUser(int userId)
        {
            // TODO try repository pattern here?
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

            return user.Adapt<UserDto>();
        }

        public async Task<UserDto> GetUser(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

            return user.Adapt<UserDto>();
        }

        public async Task<UserDto> AddUserToFamily(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> CreateUserAndFamily(CreateUserRequest request)
        {
            if (UserExistsByEmail(request.Email))
            {
                throw new ValidationException("There is already an account with that email.");
            }

            var user = new User()
            {
                Email = request.Email,
                FullName = request.FullName,
                Family = new Family()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Adapt<UserDto>();
        }

        public async Task DeleteFamilyAndUsers()
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUserFromFamily(int userIdToRemove)
        {
            throw new NotImplementedException();
        }

        private bool UserExistsByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
