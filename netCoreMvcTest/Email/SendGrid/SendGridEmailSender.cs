using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreMvcTest.Email
{
    /// <summary>
    /// sends emails using sendgrid service
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        public async Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details)
        {
            //get sendgrid key
            var apiKey = Ioc.IocContainer.Configuration["SendGrid:Key"];
            //create new sendgrid client
            var client = new SendGridClient(apiKey);

            //from details
            var from = new EmailAddress(details.FromEmail, details.FromName);
            //to details
            var to = new EmailAddress(details.ToEmail, details.ToName);
            //subject
            var subject = details.Subject;

            //content
            var content = details.Content;
            //create email class ready to send

            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                details.IsHtml ? null : details.Content,
                details.IsHtml ? details.Content : null);

            //send email
            var response = await client.SendEmailAsync(msg);

            //if success
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                //return sucessfull response
                return new SendEmailResponse();
            }

            //otherwise we failed

            try
            {
                //get result in the body
                var bodyResult = await response.Body.ReadAsStringAsync();

                var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);

                //add any errors to response
                var errorResponse = new SendEmailResponse
                {
                    Errors = sendGridResponse?.Errors.Select(f => f.Message).ToList()
                };

                if (errorResponse.Errors == null || errorResponse.Errors.Count() == 0)
                {
                    // add unknown error
                    errorResponse.Errors = new List<string>() { "Unknown Error from email sending service. Please contact support" };
                }

                return errorResponse;

            }
            catch (Exception ex)
            {
                //TODO: localization

                //break if debugging
                if (Debugger.IsAttached)
                {
                    var error = ex;
                    Debugger.Break();
                }

                //if something unexpected
                return new SendEmailResponse
                {
                    Errors = new List<string>() { "Unknown error occured" }
                }; 
            }


        }

    }
}
