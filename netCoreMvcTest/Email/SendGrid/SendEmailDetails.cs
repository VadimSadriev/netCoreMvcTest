 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// Details about email to send
    /// </summary>
    public class SendEmailDetails
    {
        /// <summary>
        /// name of the sender
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Email of the sender
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Name of the reciever
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Email of the reciever
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The email body content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// indicates if content is html content
        /// </summary>
        public bool IsHtml { get; set; }
    }
}
