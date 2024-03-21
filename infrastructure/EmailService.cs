using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using infrastructure.Configurations;
using Application.Services.Interfaces;
using Domain.Models.Models;
using Application.Services.Interfaces;
using Domain.Entities;

namespace infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _settings;
        public  EmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
            
        }

        public  async Task SendMail(Mail mail)
        {
            using (SmtpClient  Client = new SmtpClient())
            {

                // setup client
                await Client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.SslOnConnect);
                
                await Client.AuthenticateAsync(_settings.Username, _settings.Password);

                MimeMessage Message = new MimeMessage();

                Message.From.Add(new MailboxAddress(_settings.SenderName, _settings.Username));
                Message.To.Add(new MailboxAddress(mail.EmailTo, mail.EmailTo));
                Message.Subject = mail.Subject;


                BodyBuilder MessageBody = new BodyBuilder();

                MessageBody.HtmlBody = mail.Body;

                Message.Body = MessageBody.ToMessageBody();
                await Client.SendAsync(Message);
                Client.Disconnect(true);
            }


        }

        public async Task SendInviteEmail(Employees inviter, string inviteeEmail, string SecretKey , string host)
        {
            await this.SendMail(new Mail { Body = @$"

                    <div style=""max-width: 600px; margin: 0 auto; background-color: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                        <h2 style=""text-align: center; color: #333;"">Account Registration</h2>
                        <p style=""color: #666;"">You have been invited to join Task Manager workspace by <strong>{inviter.FirstName} {inviter.LastName}</strong>. Task Manager is a platform designed to streamline task management and collaboration.</p>
                        <p style=""color: #666;"">To accept the invitation and join Task Manager, please click the button below:</p>
                        <div style=""text-align: center; margin-top: 20px;"">
                            <a href="" https://{host}/auth/RegisterationRedirect/{inviteeEmail}/{SecretKey}""  style=""display: inline-block; padding: 10px 20px; background-color: #924AEF; color: #fff; text-decoration: none; border-radius: 5px;"">Register Now</a>
                        </div>
                    </div>
                            " , EmailTo = inviteeEmail , Subject= "Account Registration" });
        }

        public async Task SendResetPasswordEmail(string Email, string SecretKey, string host)
        {
            await this.SendMail(new Mail { Body = @$"

                    <div style=""max-width: 600px; margin: 0 auto; background-color: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
                        <h2 style=""text-align: center; color: #333;"">Reset Password</h2>
                        <p style=""color: #666; text-align: center; "">To Reset your Password, please click the button below</p>
                        <div style=""text-align: center; margin-top: 20px;"">
                            <a href="" https://{host}/auth/ResetPasswordRedirect/{Email}/{SecretKey}""  style=""display: inline-block; padding: 10px 20px; background-color: #924AEF; color: #fff; text-decoration: none; border-radius: 5px;"">Reset Password</a>
                        </div>
                    </div>
                            ", EmailTo = Email, Subject = "Reset Password" });

        }
    }
}
