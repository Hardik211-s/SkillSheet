using System.Net.Mail;
using System.Net;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Services.Resource;

namespace SkillSheetAPI.Services.Services
{
    public class EmailService :IEmailService
    {

        public async Task SendEmail(string username,string password,string email,string msg)
        {
            string userEmailBODY = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }}
                .container {{ max-width: 600px; margin: auto; background: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                h2 {{ color: #2c3e50; }}
                .details {{ background: #f8f9fa; padding: 15px; border-radius: 5px; margin-top: 10px; }}
                .button {{ display: inline-block; margin-top: 15px; padding: 10px 20px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; font-weight: bold; }}
                .footer {{ margin-top: 20px; font-size: 12px; color: #7f8c8d; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>🎉 Account {msg} Successfully!</h2>
                <p>Dear <strong>{username}</strong>,</p>
                <p>Your account has been successfully created. Here are your login details:</p>

                <div class='details'>
                    <p><strong>Username:</strong> {username}</p>
                    <p><strong>Password:</strong> {password}</p>
                </div>

              

                <p class='footer'>Thank you, <br> Hardik Savaliya </p>
            </div>
        </body>
        </html>";
            try
            {
                // Create a new instance of the SmtpClient class
                SmtpClient client = new SmtpClient(GeneralResource.SmtpClient, 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(GeneralResource.SourceMail, GeneralResource.SourcePassword);

                // Create a new instance of the MailMessage class
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(GeneralResource.SourceMail);
                mailMessage.To.Add(email);
                mailMessage.Subject = GeneralResource.MailSubject;
                mailMessage.Body = userEmailBODY;
                mailMessage.IsBodyHtml = true;

                // Send the email
                  client.SendAsync(mailMessage, null);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
