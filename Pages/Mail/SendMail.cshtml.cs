using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MailKit.Net.Smtp;
using MimeKit;

namespace ClientsCRUD.Pages.Mail
{

    public class SendMailModel : PageModel
    {
        public void OnGet()
        {
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("JackHwang", "yjhwang59@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("aspxjack@gmail.com", "ipnaebrorgdvggnu");
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
        [BindProperty]
        public string ToEmail { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Body { get; set; }

        public void OnPost()
        {
            SendEmail(ToEmail, Subject, Body);
        }
    }
}
