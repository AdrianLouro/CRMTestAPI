using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Security
{
    public static class JwtSecurityTokenFactory
    {
        public static JwtSecurityToken GetToken(string provider, string audience, string subject,
            IEnumerable<string> roles, DateTime validTo, string securitySecretKey, string securityAlgorithm)
        {
            return new JwtSecurityToken(
                issuer: provider,
                audience: audience,
                claims: GetClaims(subject, roles),
                expires: validTo,
                signingCredentials: GetSigningCredentials(securitySecretKey, securityAlgorithm)
            );
        }

        private static SigningCredentials GetSigningCredentials(string securitySecretKey, string securityAlgorithm)
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySecretKey)),
                securityAlgorithm
            );
        }

        private static IEnumerable<Claim> GetClaims(string subject, IEnumerable<string> roles)
        {
            return GetRoles(roles).Append(GetSubject(subject));
        }

        private static Claim GetSubject(string subject)
        {
            return new Claim("sub", subject);
        }

        private static IEnumerable<Claim> GetRoles(IEnumerable<string> roles)
        {
            return roles.Select(role => new Claim("role", role.ToLower())).ToList();
        }
    }
}