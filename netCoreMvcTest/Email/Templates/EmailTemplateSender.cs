using netCoreMvcTest.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace netCoreMvcTest.Email.Templates
{
    /// <summary>
    /// handles sending template emails
    /// </summary>
    public class EmailTemplateSender : IEmailTemplateSender
    {
        public async Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);

            //read general template from file
            using (var reader = new StreamReader(Path.Combine("Email/Templates/GeneralTemplate.html"), Encoding.UTF8))
            {
                templateText = await reader.ReadToEndAsync();
            }

            //replace special values
            templateText = templateText
                .Replace("--Title--", title)
                .Replace("--Content1--", content1)
                .Replace("--Content2--", content2)
                .Replace("--ButtonText--", buttonText)
                .Replace("--ButtonUrl--", buttonUrl);

            //set details content
            details.Content = templateText;
          
            var emailSender = Ioc.IoC.EmailSender;

            return await Ioc.IoC.EmailSender.SendEmailAsync(details);
        }
    }
}
