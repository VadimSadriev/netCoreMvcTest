using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// service that handles sending emails
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// sends emails message
        /// </summary>
        /// <param name="details">Details about email to send</param>
        /// <returns></returns>
        Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details);
    }
}
