using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IMandCRM.UI.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _enableSSL;
        private string _userName;
        private string _password;

        public EmailSender(string host, int port, bool enableSSL, string userName, string password)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _userName = userName;
            _password = password;

        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _enableSSL
            };
            return client.SendMailAsync(new MailMessage(_userName, email, subject, htmlMessage) { IsBodyHtml = true });
        }

        public Task SendEmailAttachmentAsync(string email, string subject, string htmlMessage, string attachmentFile,string bcc)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _enableSSL
            };
            MailMessage mail = new MailMessage(_userName, email, subject, htmlMessage) { IsBodyHtml = true };
            Attachment attachment;
            attachment = new System.Net.Mail.Attachment(attachmentFile);
            mail.Attachments.Add(attachment);

            MailAddress addressBCC = new MailAddress(bcc);
            mail.Bcc.Add(addressBCC);


            return client.SendMailAsync(mail);
        }
    }
}
