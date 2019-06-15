using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using System;

namespace Integration.CRMTestAPI.Controllers
{
    public class UsersController_
    {
        private readonly TestContext _context;
        private readonly Guid ADMIN_ID = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private readonly Guid NON_ADMIN_ID = Guid.Parse("22222222-2222-2222-2222-222222222222");

        public UsersController_()
        {
            _context = new TestContext();
        }

        [Fact]
        public async Task returns_ok_response_when_getting_all_users()
        {
            var response = await _context.Client.GetAsync("/users");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task returns_ok_response_when_getting_user_with_valid_id()
        {
            var response = await _context.Client.GetAsync($"/users/{ADMIN_ID}");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task returns_bad_request_response_when_getting_user_with_bad_formatted_id()
        {
            var response = await _context.Client.GetAsync("/users/1");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task returns_created_response_when_creating_new_user_with_valid_data()
        {
            StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PostAsync("/users", postData);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task returns_bad_request_response_when_creating_new_user_with_invalid_data()
        {
            StringContent postData = new StringContent("{\"name\":\"John\"", Encoding.UTF8, "application/json");
            var response = await _context.Client.PostAsync("/users", postData);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task returns_created_user_when_creating_new_user_with_valid_data()
        {
            StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PostAsync("/users", postData);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            Guid guid;
            Assert.True(Guid.TryParse(JObject.Parse(responseContent).GetValue("id").ToString(), out guid));
            Assert.Equal("John", JObject.Parse(responseContent).GetValue("name").ToString());
            Assert.Equal("Doe", JObject.Parse(responseContent).GetValue("surname").ToString());
        }

        [Fact]
        public async Task returns_bad_request_response_when_editing_user_with_bad_formatted_id()
        {
            StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PutAsync("/users/1", postData);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task returns_no_content_response_when_editing_user_with_valid_data()
        {
            StringContent postData = new StringContent("{\"name\":\"John\",\"surname\":\"Doe\"}", Encoding.UTF8,
                "application/json");
            var response = await _context.Client.PutAsync($"/users/{ADMIN_ID}", postData);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task returns_bad_request_response_when_editing_user_with_invalid_data()
        {
            StringContent postData = new StringContent("{\"name\":\"John\"", Encoding.UTF8, "application/json");
            var response = await _context.Client.PutAsync($"/users/{NON_ADMIN_ID}", postData);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task returns_no_content_response_when_deleting_user_with_valid_id()
        {
            var response = await _context.Client.DeleteAsync($"/users/{NON_ADMIN_ID}");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task returns_bad_request_response_when_deleting_user_with_bad_formatted_id()
        {
            var response = await _context.Client.DeleteAsync("/users/1");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}