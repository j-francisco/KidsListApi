using System.Threading.Tasks;

namespace KidsList.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int userId);

        Task<UserDto> GetUser(string email);

        Task<UserDto> CreateUserAndFamily(CreateUserRequest request);

        Task<UserDto> AddUserToFamily(CreateUserRequest request);

        Task RemoveUserFromFamily(int userIdToRemove);

        Task DeleteFamilyAndUsers();
    }
}
