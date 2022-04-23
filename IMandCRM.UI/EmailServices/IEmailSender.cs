using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.EmailServices
{
    public interface IEmailSender
    {
        //smtp=>gmail,hotmail
        //api=>sendgrid

        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailAttachmentAsync(string email, string subject, string htmlMessage, string attachmentFile,string bcc);
    }
}
