using System.Threading.Tasks;

namespace Experion.Marina.Core
{
    public interface IEmailSender
    {
        /// <summary>
        /// Configures the email service parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        void ConfigureEmailServiceParameters(EmailParameter parameter);

        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="toAddresses">To addresses.</param>
        /// <param name="ccAddresses">The CC addresses.</param>
        /// <param name="bccAddresses">The BCC addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="htmlEmailBody">The HTML email body - This will be the main content of the email.</param>
        /// <returns></returns>
        Task SendEmailAsync(
            string[] toAddresses,
            string[] ccAddresses,
            string[] bccAddresses,
            string subject,
            string htmlEmailBody);

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
        Task SendEmailWithAttachmentsAsync(
            string[] toAddresses,
            string[] ccAddresses,
            string[] bccAddresses,
            string subject,
            string htmlEmailBody,
            string[] attachments);

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
        Task SendEmailWithAttachmentsAsync(
            string[] toAddresses,
            string[] ccAddresses,
            string[] bccAddresses,
            string subject,
            string htmlEmailBody,
            string[] attachments,
            byte[] attachmentContent);
    }
}