using System.Threading.Tasks;
using KidsList.AuthService;

namespace KidsList.IntegrationTests
{
    public class TestAuthService : IAuthService
    {
        public string AuthUserEmail { get; set; }
        public string AuthUserFullName { get; set; }

        public TestAuthService(string authUserEmail, string authUserFullName)
        {
            AuthUserEmail = authUserEmail;
            AuthUserFullName = authUserFullName;
        }

        public Task<AuthUser> GetUserInfo(string accessToken)
        {
            return Task.FromResult(new AuthUser
            {
                Email = AuthUserEmail,
                FullName = AuthUserFullName
            });
        }
    }
}
