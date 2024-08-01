using Magic.Common.Options;
using Microsoft.Extensions.Configuration;

namespace Magic.Common
{
    public class AppConfig
    {
        public readonly DataBaseOptions dataBaseOptions;
        public readonly AuthOptions authOptions;
        public AppConfig(IConfiguration configuration)
        {
            var dataBaseOptionsSection = configuration.GetSection("ConnectionStrings");
            dataBaseOptions = new DataBaseOptions(dataBaseOptionsSection["DBConnectionString"]);

            var authOptionsSection = configuration.GetSection("AuthOptions");
            authOptions = new AuthOptions(authOptionsSection["Key"], authOptionsSection["Issuer"], authOptionsSection["Audience"], int.Parse(authOptionsSection["Lifetime"]));
        }
    }
}
