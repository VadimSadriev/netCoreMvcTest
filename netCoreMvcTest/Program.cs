using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace netCoreMvcTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();


        //Not using default builder cuz he adds things we dont need something like iis integration(we use cross platform 
        //Kestrel web server),
        //use secret store, env variables on machine, not scrooplatform default logger
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
               new WebHostBuilder()
                //user crossplatform server
                .UseKestrel()
                //add app settings to configuration
                .ConfigureAppConfiguration((hosting, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional:true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hosting.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                 //specify folder to serve app
                .UseContentRoot(Directory.GetCurrentDirectory())
                 //use args from command line
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                //use startap to configure app
                .UseStartup<Startup>();
    }
}
