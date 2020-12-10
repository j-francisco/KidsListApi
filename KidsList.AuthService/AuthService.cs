using System.Threading.Tasks;
using Auth0.AuthenticationApi;

namespace KidsList.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthenticationApiClient _client;

        public AuthService(IAuthenticationApiClient client)
        {
            _client = client;
        }

        public async Task<AuthUser> GetUserInfo(string accessToken)
        {
            var userInfo = await _client.GetUserInfoAsync(accessToken);
            return new AuthUser
            {
                Email = userInfo.Email,
                FullName = userInfo.FullName
            };
        }
    }
}
