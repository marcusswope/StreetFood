using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace StreetFood.Authentication
{
    public interface IJwtValidator
    {
        JwtValidationResult Validate(string idToken);
    }

    public class JwtValidator : IJwtValidator
    {
        private const string CLIENT_ID = "373289761542-q89spcr7cr3su1c3f6gcvvrs15jpd67q.apps.googleusercontent.com";

        public JwtValidationResult Validate(string idToken)
        {
            var result = new JwtValidationResult();

            var certificates = getCerts();

            var tvp = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidAudience = CLIENT_ID,
                ValidateIssuer = true,
                ValidIssuer = "accounts.google.com",
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                CertificateValidator = X509CertificateValidator.None,
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    return identifier.Select(x =>
                    {
                        if (certificates.ContainsKey(x.Id.ToUpper()))
                        {
                            return new X509SecurityKey(certificates[x.Id.ToUpper()]);
                        }
                        return null;
                    }).First(x => x != null);
                },
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromHours(13)
            };

            try
            {
                SecurityToken validatedToken;
                var cp = new JwtSecurityTokenHandler().ValidateToken(idToken, tvp, out validatedToken);
                result.IsValid = cp != null;
            }
            catch (Exception e)
            {
                result.IsValid = false;
            }

            var token = new JwtSecurityToken(idToken);
            var claims = token.Claims.ToArray();
            foreach (var claim in claims)
            {
                if (claim.Type.Equals("email"))
                {
                    result.EmailAddress = claim.Value;
                }
            }

            return result;
        }

        private const string BeginCert = "-----BEGIN CERTIFICATE-----\\n";
        private const string EndCert = "\\n-----END CERTIFICATE-----\\n";
        public Dictionary<string, X509Certificate2> getCerts()
        {
            var request = WebRequest.Create("https://www.googleapis.com/oauth2/v1/certs");
            var reader = new StreamReader(request.GetResponse().GetResponseStream());
            var responseFromServer = reader.ReadToEnd();
            var split = responseFromServer.Split(':');
            var certBytes = new byte[2][];
            var index = 0;
            var utf8 = new UTF8Encoding();
            foreach (string item in split)
            {
                if (item.IndexOf(BeginCert) <= 0) continue;
                var startSub = item.IndexOf(BeginCert);
                var endSub = item.IndexOf(EndCert) + EndCert.Length;
                certBytes[index] = utf8.GetBytes(item.Substring(startSub, endSub).Replace("\\n", "\n"));
                index++;
            }

            var certificates = new Dictionary<string, X509Certificate2>();

            for (var i = 0; i < certBytes.Length; i++)
            {
                var certificate = new X509Certificate2(certBytes[i]);
                certificates.Add(certificate.Thumbprint, certificate);
            }

            return certificates;
        }
    }
}