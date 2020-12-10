using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using KidsList.Data;
using KidsList.Services.Kids;
using Xunit;

namespace KidsList.IntegrationTests.Kids
{
    [Collection("IntegrationTests")]
    public class KidProfileTestsUnauthenticated : BaseIntegrationTest
    {
        public KidProfileTestsUnauthenticated(ApiWebApplicationFactory factory) : base(factory, unauthenticated: true)
        {
        }

        [Fact]
        public async Task CannotAddKidToFamily_WhenNotAuthenticated()
        {
            var request = CreateStringContent(new AddKidRequest
            {
                Name = "Ian"
            });

            var response = await Client.PostAsync("/api/kids", request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Collection("IntegrationTests")]
    public class KidProfileTests : BaseIntegrationTest
    {
        public KidProfileTests(ApiWebApplicationFactory factory) : base(factory)
        {
        }

        protected async override Task AfterInitializeAsync()
        {
            await AddUserToDb("jeff@test.com", "Jeff Francisco");
        }

        [Fact]
        public async Task CanAddKidToFamilySuccessfully()
        {
            var request = CreateStringContent(new AddKidRequest
            {
                Name = "Ian"
            });

            var response = await Client.PostAsync("/api/kids", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var kid = await DeserializeDto<KidDto>(response);

            var parentUser = await GetUser("jeff@test.com");

            kid.Name.Should().Be("Ian");
            kid.FamilyId.Should().Be(parentUser.FamilyId);
            kid.Id.Should().BeGreaterThan(0);

            using var db = new KidsListContext(ContextOptions);
            var dbKid = await db.Kids.FindAsync(kid.Id);
            dbKid.Should().NotBeNull();
        }
    }
}
