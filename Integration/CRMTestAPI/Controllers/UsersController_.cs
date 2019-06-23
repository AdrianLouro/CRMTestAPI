using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using Xunit.Sdk;
using static System.Net.HttpStatusCode;

namespace Integration.CRMTestAPI.Controllers
{
    public class UsersController_: BeforeAfterTestAttribute
    {
        private readonly TestContext _context;
        private readonly Guid ADMIN_ID = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private readonly Guid NON_ADMIN_ID = Guid.Parse("22222222-2222-2222-2222-222222222222");
        private readonly Dictionary<string, string> _jwtTokens = new Dictionary<string, string>();
        private readonly HttpClient _client;
        
        public UsersController_()
        {
            _context = new TestContext();
            _client = _context.Client;
            SetJwtTokens();
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }

        public override void After(MethodInfo methodUnderTest)
        {
           
        }

        [Fact]
        public async Task returns_unauthorized_response_when_getting_all_users_without_admin_token()
        {
            var noTokenResponse = await _client.GetAsync("/users");
            SetJwtTokenWithRole("");
            var badTokenResponse = await _client.GetAsync("/users");

            Assert.Equal(Unauthorized, noTokenResponse.StatusCode);
            Assert.Equal(Forbidden, badTokenResponse.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_getting_a_user_without_admin_token()
        {
            var noTokenResponse = await _client.GetAsync($"/users/{ADMIN_ID}");
            SetJwtTokenWithRole("");
            var badTokenResponse = await _client.GetAsync($"/users/{ADMIN_ID}");

            Assert.Equal(Unauthorized, noTokenResponse.StatusCode);
            Assert.Equal(Forbidden, badTokenResponse.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_creating_a_user_without_admin_token()
        {
            StringContent postData = new StringContent("{\"name\":\"John\"}", Encoding.UTF8, "application/json");
            var noTokenResponse = await _client.PostAsync($"/users", postData);
            SetJwtTokenWithRole("");
            var badTokenResponse = await _client.PostAsync($"/users", postData);

            Assert.Equal(Unauthorized, noTokenResponse.StatusCode);
            Assert.Equal(Forbidden, badTokenResponse.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_editing_a_user_without_admin_token()
        {
            StringContent postData = new StringContent("{\"name\":\"John\"}", Encoding.UTF8, "application/json");
            var noTokenResponse = await _client.PutAsync($"/users/{ADMIN_ID}", postData);
            SetJwtTokenWithRole("");
            var badTokenResponse = await _client.PutAsync($"/users/{ADMIN_ID}", postData);

            Assert.Equal(Unauthorized, noTokenResponse.StatusCode);
            Assert.Equal(Forbidden, badTokenResponse.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_deleting_a_user_without_admin_token()
        {
            var noTokenResponse = await _client.DeleteAsync($"/users/{ADMIN_ID}");
            SetJwtTokenWithRole("");
            var badTokenResponse = await _client.DeleteAsync($"/users/{ADMIN_ID}");

            Assert.Equal(Unauthorized, noTokenResponse.StatusCode);
            Assert.Equal(Forbidden, badTokenResponse.StatusCode);
        }

        private void SetJwtTokens()
        {
            _jwtTokens.Add("admin", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJzdWIiOiIxMTExMTExMS0xMTExLTExMTEtMTExMS0xMTExMTExMTExMTEiLCJleHAiOjE4NzY0MjE5ODQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.SSs7C16LrMwKpO-owlmU2L2HPFhWQM6UzGN0jYAOVE8");
            _jwtTokens.Add("", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjIyMjIyMi0yMjIyLTIyMjItMjIyMi0yMjIyMjIyMjIyMjIiLCJleHAiOjE4NzY0MjIwMTQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9._3TLPTeAv7cvp4ei44XDHCe-_C4YahtJKcLYGXKk7Xc");

        }

        private void SetJwtTokenWithRole(string role)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _jwtTokens[role]);

        }
    }
}