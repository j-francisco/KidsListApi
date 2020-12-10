using System.Threading.Tasks;

namespace KidsList.AuthService
{
    public interface IAuthService
    {
        Task<AuthUser> GetUserInfo(string accessToken);
    }
}
