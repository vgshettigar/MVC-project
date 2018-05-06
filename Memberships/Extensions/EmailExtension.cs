using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Memberships.Extensions
{
    public static class EmailExtension
    {
        public static void Send(this IdentityMessage message)
        {
            try
            {
                var password = ConfigurationManager.AppSettings["password"];
            var from = ConfigurationManager.AppSettings["from"];
            var host = ConfigurationManager.AppSettings["host"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["port"]);

            //Create email to send

            var email = new MailMessage(from, message.Destination, message.Subject, message.Body);

            email.IsBodyHtml = true;

            var client = new SmtpClient(host, port);

            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(from, password);

                //send email
                client.Send(email);

            }
            catch
            {

            }
            

        }
    }
}