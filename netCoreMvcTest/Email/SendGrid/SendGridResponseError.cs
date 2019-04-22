namespace netCoreMvcTest.Email
{
    /// <summary>
    /// error response for sendgridresponse
    /// </summary>
    public class SendGridResponseError
    {
        /// <summary>
        /// error message
        /// </summary>
       public string Message { get; set; }

        /// <summary>
        /// field inside the email message that is error is related to
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// usefull information for resolving the error
        /// </summary>
        public string Help { get; set; }
    }
}
