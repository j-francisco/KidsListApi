using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KidsList.Data;
using KidsList.Services.Users;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using Xunit;

namespace KidsList.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
    {
        protected ApiWebApplicationFactory Factory { get; }
        protected HttpClient Client { get; }
        protected IConfiguration Configuration { get; }
        protected DbContextOptions<KidsListContext> ContextOptions { get; private set; }
        protected TestAuthService TestAuthService { get; }

        private static readonly Checkpoint _dbCheckpoint = new Checkpoint
        {
            SchemasToInclude = new[]
            {
                "public"
            },
            DbAdapter = DbAdapter.Postgres
        };

        public BaseIntegrationTest(
            ApiWebApplicationFactory factory,
            bool unauthenticated = false,
            string currentUserEmail = "jeff@test.com",
            string currentUserFullName = "Jeff Francisco")
        {
            Factory = factory;

            TestAuthService = new TestAuthService(currentUserEmail, currentUserFullName);

            if (unauthenticated)
            {
                Client = Factory.CreateClient();
            }
            else
            {
                Client = Factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddTestAuthentication(TestAuthService);
                    });
                }).CreateClient();
            }

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile("integrationTestSettings.json")
                  .Build();

            var connString = Configuration.GetConnectionString("IntegrationTestsContext");

            // ContextOptions can be used by tests to create db context instance
            ContextOptions = new DbContextOptionsBuilder<KidsListContext>()
                .UseNpgsql(connString)
                .UseSnakeCaseNamingConvention()
                .Options;
        }

        public async Task InitializeAsync()
        {
            // Use Respawn to reset the db
            var connString = Configuration.GetConnectionString("IntegrationTestsContext");
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            await _dbCheckpoint.Reset(conn);

            await AfterInitializeAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterInitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected static StringContent CreateStringContent<T>(T dto)
        {
            var json = JsonSerializer.Serialize(dto);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        protected async static Task<T> DeserializeDto<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        protected async Task AddUserToDb(string email, string fullName)
        {
            using var db = new KidsListContext(ContextOptions);
            db.Users.Add(new User { Family = new Family(), Email = email, FullName = fullName });
            await db.SaveChangesAsync();
        }

        protected async Task<UserDto> GetUser(string email)
        {
            using var db = new KidsListContext(ContextOptions);
            var dbUser = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null)
            {
                return null;
            }
            return dbUser.Adapt<UserDto>();
        }
    }
}
