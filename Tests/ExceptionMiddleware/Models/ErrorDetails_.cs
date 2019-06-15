using ExceptionMiddleware.Models;
using Xunit;

namespace Tests.Entities.Models
{
    public class ErrorDetails_
    {
        [Fact]
        public void stringifies_exception_middleware_exception()
        {
            ErrorDetails errorDetails = new ErrorDetails()
            {
                StatusCode = 500,
                Message = "Internal server error"
            };
            

            Assert.Equal("{\"StatusCode\":500,\"Message\":\"Internal server error\"}", errorDetails.ToString());
        }
    }
}
