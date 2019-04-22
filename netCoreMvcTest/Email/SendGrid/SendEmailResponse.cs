using System.Collections.Generic;
using System.Linq;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// response from send email
    /// </summary>
    public class SendEmailResponse
    {
        /// <summary>
        /// true if email sent successfully
        /// </summary>
        public bool Success => !(Errors?.Count() > 0);

        /// <summary>
        /// error message if email didnt send
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
