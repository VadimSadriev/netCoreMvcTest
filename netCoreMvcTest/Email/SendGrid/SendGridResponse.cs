using System.Collections.Generic;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// response from sendgrid sendmessage
    /// </summary>
    public class SendGridResponse
    {
        /// <summary>
        /// any errors from response
        /// </summary>
        public List<SendGridResponseError> Errors { get; set; }
    }
}
