using Microsoft.Extensions.DependencyInjection;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// extensions methods for any send grid classes
    /// </summary>
    public static class SendGridExtensions
    {
        /// <summary>
        /// injects send grid email sender into services to handle IEmailSender service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
        {
            //Inject the SendGridEmailSender
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            //return collection for  chaining
            return services;
        }
    }
}
