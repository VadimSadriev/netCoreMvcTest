using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.Email
{
    public static class AppEmailSender
    {
        /// <summary>
        /// sends verification email to the user
        /// </summary>
        /// <param name="displayName">user's display first name</param>
        /// <param name="email">the user's email to be verified</param>
        /// <param name="verificationUrl">the url the user needs to click to verify email</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl)
        {
          return await Ioc.IoC.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                Content = "this is first html <b>content</b>",
                FromEmail = Ioc.IocContainer.Configuration["EmailSettings:FromEmail"],
                FromName = Ioc.IocContainer.Configuration["EmailSettings:FromName"],
                IsHtml = true,
                ToName = displayName,
                ToEmail = email,
                Subject = "Verify your email - TestApp"
           },
           "VerifyEmail",
           $"Hi {displayName ?? "stranger"}",
           "Thanks for creating account <br/> Please verify by pressing the button below",
           "Verify email",
           verificationUrl);
        }
    }
}
