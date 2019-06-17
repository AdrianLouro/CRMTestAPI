using System.Net;
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

namespace Integration.CRMTestAPI.Controllers
{
    public class UsersController_: BeforeAfterTestAttribute
    {
        private readonly TestContext _context;
        private readonly Guid ADMIN_ID = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private readonly Guid NON_ADMIN_ID = Guid.Parse("22222222-2222-2222-2222-222222222222");
        private readonly Dictionary<string, string> JWT_TOKENS = new Dictionary<string, string>();
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
            var response = await _context.Client.GetAsync("/users");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_getting_a_user_without_admin_token()
        {
            var response = await _context.Client.GetAsync($"/users/{ADMIN_ID}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_creating_a_user_without_admin_token()
        {
            StringContent postData = new StringContent("{\"name\":\"John\"}", Encoding.UTF8, "application/json");
            var response = await _context.Client.PostAsync($"/users", postData);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_editing_a_user_without_admin_token()
        {
            StringContent postData = new StringContent("{\"name\":\"John\"}", Encoding.UTF8, "application/json");
            var response = await _context.Client.PutAsync($"/users/{ADMIN_ID}", postData);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task return_unauthorized_response_when_deleting_a_user_without_admin_token()
        {
            var response = await _context.Client.DeleteAsync($"/users/{ADMIN_ID}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        //[Fact]
        //public async Task returns_ok_response_when_getting_all_users()
        //{
        //    var response = await _context.Client.GetAsync("/users");
        //    response.EnsureSuccessStatusCode();

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_ok_response_when_getting_user_with_valid_id()
        //{
        //    var response = await _context.Client.GetAsync($"/users/{ADMIN_ID}");
        //    response.EnsureSuccessStatusCode();

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_bad_request_response_when_getting_user_with_bad_formatted_id()
        //{
        //    var response = await _context.Client.GetAsync("/users/1");

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_created_response_when_creating_new_user_with_valid_data()
        //{
        //    StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
        //        "application/json");
        //    var response = await _context.Client.PostAsync("/users", postData);
        //    response.EnsureSuccessStatusCode();

        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_bad_request_response_when_creating_new_user_with_invalid_data()
        //{
        //    StringContent postData = new StringContent("{\"name\":\"John\"}", Encoding.UTF8, "application/json");
        //    var response = await _context.Client.PostAsync("/users", postData);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_created_user_when_creating_new_user_with_valid_data()
        //{
        //    StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
        //        "application/json");
        //    var response = await _context.Client.PostAsync("/users", postData);
        //    response.EnsureSuccessStatusCode();

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    Guid guid;
        //    Assert.True(Guid.TryParse(JObject.Parse(responseContent).GetValue("id").ToString(), out guid));
        //    Assert.Equal("John", JObject.Parse(responseContent).GetValue("name").ToString());
        //    Assert.Equal("Doe", JObject.Parse(responseContent).GetValue("surname").ToString());
        //}

        //[Fact]
        //public async Task returns_bad_request_response_when_editing_user_with_bad_formatted_id()
        //{
        //    StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
        //        "application/json");
        //    var response = await _context.Client.PutAsync("/users/1", postData);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_no_content_response_when_editing_user_with_valid_data()
        //{
        //    StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
        //        "application/json");
        //    var response = await _context.Client.PutAsync($"/users/{ADMIN_ID}", postData);
        //    response.EnsureSuccessStatusCode();

        //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_bad_request_response_when_editing_user_with_invalid_data()
        //{
        //    StringContent postData = new StringContent("{\"name\":\"John\"", Encoding.UTF8, "application/json");
        //    var response = await _context.Client.PutAsync($"/users/{NON_ADMIN_ID}", postData);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_no_content_response_when_deleting_user_with_valid_id()
        //{
        //    var response = await _context.Client.DeleteAsync($"/users/{NON_ADMIN_ID}");
        //    response.EnsureSuccessStatusCode();

        //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        //}

        //[Fact]
        //public async Task returns_bad_request_response_when_deleting_user_with_bad_formatted_id()
        //{
        //    var response = await _context.Client.DeleteAsync("/users/1");

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}

        private void SetJwtTokens()
        {
            JWT_TOKENS.Add("admin", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJzdWIiOiIxMTExMTExMS0xMTExLTExMTEtMTExMS0xMTExMTExMTExMTEiLCJleHAiOjE4NzY0MjE5ODQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.SSs7C16LrMwKpO-owlmU2L2HPFhWQM6UzGN0jYAOVE8");
            JWT_TOKENS.Add("", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyMjIyMjIyMi0yMjIyLTIyMjItMjIyMi0yMjIyMjIyMjIyMjIiLCJleHAiOjE4NzY0MjIwMTQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9._3TLPTeAv7cvp4ei44XDHCe-_C4YahtJKcLYGXKk7Xc");

        }

        private void SetJwtTokenWithRole(string role)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "Your Oauth token");

        }
    }
}