using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.Email.Templates
{
    /// <summary>
    /// Extensions for any emailtemplate sender classes
    /// </summary>
    public static class EmailTemplateSenderExtensions
    {
        /// <summary>
        /// injects Iemailtemplatesender into services to handle IEmailSender service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailTemplateSender(this IServiceCollection services)
        {
            //Inject the SendGridEmailSender
            services.AddTransient<IEmailTemplateSender, EmailTemplateSender>();

            //return collection for  chaining
            return services;
        }
    }
}
