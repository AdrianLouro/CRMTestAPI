using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;
using Security;
using Xunit;
using static System.DateTime;

namespace Tests.Security
{
    public class JwtSecurityTokenFactory_
    {

        JwtSecurityToken _jwtToken;

        public JwtSecurityTokenFactory_()
        {
            _jwtToken = GetJwtToken();
        }

        [Fact]
        public void sets_every_role_as_claim()
        {
            Assert.Equal(2, _jwtToken.Claims.Count(claim => claim.Type.Equals("role")));
            Assert.Equal("admin", _jwtToken.Claims.Where((claim => claim.Type.Equals("role"))).First().Value);
            Assert.Equal("superadmin", _jwtToken.Claims.Where((claim => claim.Type.Equals("role"))).Last().Value);
        }

        [Fact]
        public void sets_subject()
        {
            Assert.Equal(1, _jwtToken.Claims.Count(claim => claim.Type.Equals("sub")));
            Assert.Equal("11111111-1111-1111-1111-111111111111", _jwtToken.Claims.Where((claim => claim.Type.Equals("sub"))).First().Value);
        }

        private JwtSecurityToken GetJwtToken()
        {
            return JwtSecurityTokenFactory.GetToken(
                "",
                "",
                "11111111-1111-1111-1111-111111111111",
                new List<string>() { "admin", "superAdmin" },
                Now,
                "ThisIsAFakeSecretKey",
                HmacSha256
            );
        }
    }
}