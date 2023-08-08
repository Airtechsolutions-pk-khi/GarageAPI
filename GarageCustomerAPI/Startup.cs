using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web.Configuration;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Reflection;

[assembly: OwinStartup(typeof(GarageCustomerAPI.Startup))]

namespace GarageCustomerAPI
{
	public class Startup
	{
        //public void Configuration(IAppBuilder app)
        //{
        //	app.UseJwtBearerAuthentication(
        //		new JwtBearerAuthenticationOptions
        //		{
        //			AuthenticationMode = AuthenticationMode.Active,
        //			TokenValidationParameters = new TokenValidationParameters()
        //			{
        //				ValidateIssuer = true,
        //				ValidateAudience = true,
        //				ValidateIssuerSigningKey = true,
        //				ValidIssuer = WebConfigurationManager.AppSettings["Issuer"], //some string, normally web url,  
        //				ValidAudience = WebConfigurationManager.AppSettings["Audience"],
        //				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["SecretKey"]))
        //			}
        //		});
        //}
        public void Configuration(IAppBuilder app)
        {
            var issuer = WebConfigurationManager.AppSettings["Issuer"];
            var audience = WebConfigurationManager.AppSettings["Audience"];
            var secretKey = WebConfigurationManager.AppSettings["SecretKey"];

            //var keyByteArray = TextEncodings.Base64Url.Decode(secretKey);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); 


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = signingKey
            };

            var jwtBearerAuthenticationOptions = new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = tokenValidationParameters
            };

            app.UseJwtBearerAuthentication(jwtBearerAuthenticationOptions);

        }
    }
}
