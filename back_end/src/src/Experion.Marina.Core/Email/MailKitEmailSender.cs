using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experion.Marina.Core
{
    public class MailKitEmailSender : IEmailSender
    {
        private string accountAddress;
        private string accountPassword;
        private bool isAuthenticationEnabled;
        private bool isSslEnabled;
        private string senderName;
        private string smtpHost;
        private int smtpPort;

        /// <summary>
        /// Configures the email service parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void ConfigureEmailServiceParameters(EmailParameter parameter)
        {
            smtpHost = parameter.SmtpHost;
            smtpPort = parameter.SmtpPort;
            isSslEnabled = parameter.IsSslEnabled;
            accountAddress = parameter.AccountAddress;
            accountPassword = parameter.AccountPassword;
            senderName = parameter.SenderName;
            isAuthenticationEnabled = parameter.IsAuthenticationEnabled;
        }

        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="toAddresses">To addresses.</param>
        /// <param name="ccAddresses">The CC addresses.</param>
        /// <param name="bccAddresses">The BCC addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlEmailBody">The HTML email body - This will be the main content of the email.</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string[] toAddresses, string[] ccAddresses, string[] bccAddresses, string subject, string htmlEmailBody)
        {
            var email = new MimeMessage();
            ConstructMail(email, toAddresses, ccAddresses, bccAddresses, subject, htmlEmailBody, null, null);
            this.SendMail(email);
        }

        /// <summary>
        /// Sends the email with attachments asynchronous.
        /// </summary>
        /// <param name="toAddresses">To addresses.</param>
        /// <param name="ccAddresses">The CC addresses.</param>
        /// <param name="bccAddresses">The BCC addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlEmailBody">The HTML email body - This will be the main content of the email.</param>
        /// <param name="attachments">The attachments for the email.</param>
        /// <returns></returns>
        public async Task SendEmailWithAttachmentsAsync(string[] toAddresses, string[] ccAddresses, string[] bccAddresses, string subject, string htmlEmailBody, string[] attachments)
        {
            var email = new MimeMessage();
            ConstructMail(email, toAddresses, ccAddresses, bccAddresses, subject, htmlEmailBody, attachments, null);
            this.SendMail(email);
        }

        /// <summary>
        /// Sends the email with attachments asynchronous.
        /// </summary>
        /// <param name="toAddresses">To addresses.</param>
        /// <param name="ccAddresses">The CC addresses.</param>
        /// <param name="bccAddresses">The BCC addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlEmailBody">The HTML email body - This will be the main content of the email.</param>
        /// <param name="attachments">The attachments for the email.</param>
        /// <param name="attachmentContent">The attachment content.</param>
        /// <returns></returns>
        public async Task SendEmailWithAttachmentsAsync(string[] toAddresses, string[] ccAddresses, string[] bccAddresses, string subject, string htmlEmailBody, string[] attachments, byte[] attachmentContent)
        {
            var email = new MimeMessage();
            ConstructMail(email, toAddresses, ccAddresses, bccAddresses, subject, htmlEmailBody, attachments, attachmentContent);
            this.SendMail(email);
        }

        /// <summary>
        /// Constructs the mail.
        /// </summary>
        /// <param name="email">The email message object.</param>
        /// <param name="toAddresses">To addresses.</param>
        /// <param name="ccAddresses">The Cc addresses.</param>
        /// <param name="bccAddresses">The Bcc addresses.</param>
        /// <param name="subject">The subject of the mail.</param>
        /// <param name="htmlEmailBody">The HTML email body.</param>
        /// <param name="attachments">The attachments.</param>
        private void ConstructMail(MimeMessage email, string[] toAddresses, string[] ccAddresses, string[] bccAddresses, string subject, string htmlEmailBody, string[] attachments, byte[] attachmentContent)
        {
            // Setting the From address
            email.From.Add(new MailboxAddress(senderName, accountAddress));

            // Constructing the To List of email
            var mailboxAddresses = ConvertToInternetAddress(toAddresses);
            email.To.AddRange(mailboxAddresses);

            // Constructing the Cc List of email
            var mailboxCcAddresses = ConvertToInternetAddress(ccAddresses);
            email.Cc.AddRange(mailboxCcAddresses);

            // Constructing the Bcc List of email
            var mailboxBccAddresses = ConvertToInternetAddress(bccAddresses);
            email.Bcc.AddRange(mailboxBccAddresses);

            // Constructing the Subject of email
            if (!string.IsNullOrEmpty(subject))
            {
                email.Subject = subject;
            }

            // Constructing the Body of email
            var emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = htmlEmailBody;

            // Adding attachments, if any
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    emailBodyBuilder.Attachments.Add(attachment, attachmentContent);
                }
            }
            email.Body = emailBodyBuilder.ToMessageBody();
        }

        /// <summary>
        /// Converts to Internet address.
        /// </summary>
        /// <param name="emailAddresses">The email addresses.</param>
        /// <returns></returns>
        private IEnumerable<InternetAddress> ConvertToInternetAddress(string[] emailAddresses)
        {
            var internetAddressList = new List<InternetAddress>();
            if (emailAddresses != null)
            {
                foreach (var emailAddress in emailAddresses)
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        internetAddressList.Add(new MailboxAddress(string.Empty, emailAddress));
                    }
                }
            }
            return internetAddressList;
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="email">The email.</param>
        private void SendMail(MimeMessage email)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    if (isAuthenticationEnabled)
                    {
                        client.Connect(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.None);
                    }
                    else
                    {
                        client.Connect(smtpHost, smtpPort, isSslEnabled);
                    }

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(accountAddress, accountPassword);

                    client.Send(email);
                    client.Disconnect(true);
                }
            }
            catch (ArgumentNullException ae)
            {
                throw new EmailException(ae.Message, EmailException.EMAIL_PARAMETER_ERROR, ae);
            }
            catch (Exception e)
            {
                throw new EmailException(e.Message, EmailException.EMAIL_AUTHENTICATION_ERROR, e);
            }
        }
    }
}