using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;
using System.Text;
using Xunit.Sdk;
using static System.Net.HttpStatusCode;

namespace Integration.CRMTestAPI.Controllers
{
    public class AuthController_ : BeforeAfterTestAttribute
    {
        private readonly TestContext _context;
        private readonly HttpClient _client;

        public AuthController_()
        {
            _context = new TestContext();
            _client = _context.Client;
        }

        public override void Before(MethodInfo methodUnderTest)
        {
        }

        public override void After(MethodInfo methodUnderTest)
        {

        }

        [Fact]
        public async Task returns_ok_for_valid_credentials()
        {
            StringContent postData = new StringContent("{\"email\":\"admin@admin.es\",\"password\":\"password\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PostAsync("/login", postData);
            response.EnsureSuccessStatusCode();

            Assert.Equal(OK, response.StatusCode);
        }

        [Fact]
        public async Task returns_unauthorized_for_invalid_credentials()
        {
            StringContent postData = new StringContent("{\"email\":\"fakeMail\",\"password\":\"fakePassword\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PostAsync("/login", postData);

            Assert.Equal(Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task returns_unauthorized_for_deleted_user()
        {
            StringContent postData = new StringContent("{\"email\":\"deleted@user.es\",\"password\":\"password\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PostAsync("/login", postData);

            Assert.Equal(Unauthorized, response.StatusCode);
        }
    }
}