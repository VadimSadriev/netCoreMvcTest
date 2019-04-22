using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using netCoreMvcTest.Data;
using netCoreMvcTest.Email;
using System;

namespace netCoreMvcTest.Ioc
{
    /// <summary>
    /// dependency injection container making use of in .net core service provider
    /// </summary>
    public static class IocContainer
    {
        /// <summary>
        /// service provider for this application 
        /// </summary>
        public static IServiceProvider Provider { get; set; }
        public static ServiceProvider Provider1 { get; set; }

        /// <summary>
        /// configuration manager for application
        /// </summary>
        public static IConfiguration Configuration { get; set; }
    }

    public static class IoC
    {
        public static ApplicationDbContext ApplicationDbContext => IocContainer.Provider.GetService<ApplicationDbContext>();

        public static IEmailSender EmailSender => IocContainer.Provider.GetService<IEmailSender>();

        public static IEmailTemplateSender EmailTemplateSender => IocContainer.Provider.GetService<IEmailTemplateSender>();
    }
}
