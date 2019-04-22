using System.Threading.Tasks;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// sends emails using email sender and creating html email from specific templates
    /// </summary>
    public interface IEmailTemplateSender
    {
        /// <summary>
        /// sends an email with given details using the general template
        /// </summary>
        /// <param name="details">email message details, content proporty is ignored and replaced with the template</param>
        /// <param name="title">title</param>
        /// <param name="content1">first line contents</param>
        /// <param name="content2">second line contents</param>
        /// <param name="buttontText">button text</param>
        /// <param name="buttonUrl">button url</param>
        /// <returns></returns>
        Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttontText, string buttonUrl);
    }
}
