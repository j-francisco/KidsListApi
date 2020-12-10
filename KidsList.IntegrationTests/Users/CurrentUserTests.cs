using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net;
using KidsList.Services.Users;

namespace KidsList.IntegrationTests.Users
{
    [Collection("IntegrationTests")]
    public class CurrentUserTestsUnauthenticated : BaseIntegrationTest
    {
        public CurrentUserTestsUnauthenticated(ApiWebApplicationFactory factory) : base(factory, unauthenticated: true)
        {
        }

        [Fact]
        public async Task CurrentUser_Returns401_WhenNotAuthenticated()
        {
            var response = await Client.GetAsync("/api/users/current");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Collection("IntegrationTests")]
    public class CurrentUserTests : BaseIntegrationTest
    {
        public CurrentUserTests(ApiWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CurrentUser_ReturnsSuccessfully_WhenExistingUser()
        {
            await AddUserToDb("jeff@test.com", "Jeff F");

            var response = await Client.GetAsync("/api/users/current");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userDto = await DeserializeDto<UserDto>(response);

            userDto.Should().NotBeNull();
            userDto.Email.Should().Be("jeff@test.com");
            userDto.FullName.Should().Be("Jeff F");
            userDto.FamilyId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CurrentUser_ReturnsSuccessfully_WhenExistingUser_WithEmailCaseMismatch()
        {
            await AddUserToDb("JEFF@test.com", "Jeff F");

            var response = await Client.GetAsync("/api/users/current");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userDto = await DeserializeDto<UserDto>(response);

            userDto.Should().NotBeNull();
            userDto.Email.Should().Be("JEFF@test.com");
            userDto.FullName.Should().Be("Jeff F");
            userDto.FamilyId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CurrentUser_CreatesNewUserAndFamily_WhenNewUser()
        {
            var response = await Client.GetAsync("/api/users/current");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userDto = await DeserializeDto<UserDto>(response);

            userDto.Should().NotBeNull();
            userDto.Email.Should().Be("jeff@test.com");
            userDto.FullName.Should().Be("Jeff Francisco");
            userDto.FamilyId.Should().BeGreaterThan(0);

            var userFromDb = await GetUser("jeff@test.com");
            userFromDb.Id.Should().Be(userDto.Id);
        }

        [Fact]
        public async Task CurrentUser_CreatesNewUserAndFamily_WhenNewUser_AllowsEmptyFullName()
        {
            TestAuthService.AuthUserFullName = null;

            var response = await Client.GetAsync("/api/users/current");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userDto = await DeserializeDto<UserDto>(response);

            userDto.Should().NotBeNull();
            userDto.Email.Should().Be("jeff@test.com");
            userDto.FullName.Should().BeNull();
            userDto.FamilyId.Should().BeGreaterThan(0);
        }


        [Fact]
        public async Task CreatingUserAndFamily_RequiresEmail()
        {
            TestAuthService.AuthUserEmail = null;

            var response = await Client.GetAsync("/api/users/current");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
