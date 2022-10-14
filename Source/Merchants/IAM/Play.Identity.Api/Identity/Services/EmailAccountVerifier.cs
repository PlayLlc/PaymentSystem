using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Play.Identity.Api.Identity.Services;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

using Play.Randoms;

using SendGrid;
using SendGrid.Helpers.Mail;

using Attachment = SendGrid.Helpers.Mail.Attachment;

namespace Play.Identity.Api.Identity.Services;

public class SendGridConfig
{
    #region Instance Values

    public string ApiKey { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;

    [EmailAddress]
    public string FromEmail { get; set; } = string.Empty;

    public int Ports { get; set; } // Hack: I don't know where to specify the HTTPS port

    #endregion
}

//public interface ISendEmail
//{
//    #region Instance Members

//    public Task<RestfulResult> SendEmail(EmailMessage emailMessage);

//    #endregion
//}

//public class EmailClient : ISendEmail
//{
//    #region Instance Values

//    private readonly SendGridClient _Client;
//    private readonly SendGridConfig _Config;

//    #endregion

//    #region Constructor

//    public EmailClient(SendGridConfig config)
//    {
//        _Config = config;
//        _Client = new SendGridClient(config.ApiKey) {UrlPath = config.Server};
//    }

//    #endregion

//    #region Instance Members

//    public async Task<RestfulResult> SendEmail(EmailMessage emailMessage)
//    {
//        var from = new EmailAddress(_Config.FromEmail, _Config.Nickname);
//        var to = new EmailAddress(emailMessage.Email, emailMessage.Name);
//        var msg = MailHelper.CreateSingleEmail(from, to, emailMessage.Subject, emailMessage.Message, emailMessage.HtmlContent);
//        Response? response = await _Client.SendEmailAsync(msg).ConfigureAwait(false);

//        if (response.IsSuccessStatusCode)
//            return new RestfulResult();

//        return new RestfulResult(response.StatusCode, new string[] {$"Header: {response.Headers} \\n\\nBody: {response.Body}"});
//    }

//    #endregion
//}

//public class EmailAddress
//{ 
//    [EmailAddress]
//    public readonly string Email;

//    public readonly string Name;

//    public EmailAddress(string email, string name)
//    {
//        Email = email;
//        Name = name;
//    }
//}

//public class EmailMessage
//{
//    #region Instance Values

//    public readonly EmailAddress Email; 
//    public readonly string Subject;
//    public readonly string Message;
//    public string? HtmlContent;

//    #endregion

//    #region Constructor

//    #endregion

//    public EmailMessage(EmailAddress email, string subject, string message, string? htmlContent)
//    {
//        Email = email;
//        Subject = subject;
//        Message = message;
//        HtmlContent = htmlContent;
//    }
//}

public static class TestEmailVerificationTemplate
{
    #region Static Metadata

    public const string Template =
        @"<!doctypehtml> <meta charset=""utf-8""> <meta content=""ie=edge"" http-equiv=""x-ua-compatible""> <title>Email Confirmation</title> <meta content=""width=device-width,initial-scale=1"" name=""viewport""> <style>.backgroundColor{background-color: #f9f9f9}.formBackgroundColor{background-color: #f4f6f9}.formText{font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif}.footerText{font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; color: #213848}h1, h2, h3, h4, h5, h6{font-weight: 300; line-height: normal; color: #213848}h1{font-size: 3em; color: #213848}h2{color: #213848; font-size: 2em}p{color: #213848; font-size: 14px; font-weight: 400; line-height: 24px}.titleText{color: #213848; font-family: 'Open Sans', sans-serif}@media screen{@font-face{font-family: 'Source Sans Pro'; font-style: normal; font-weight: 400; src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff')}@font-face{font-family: 'Source Sans Pro'; font-style: normal; font-weight: 700; src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff')}}a, body, table, td{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%}table, td{mso-table-rspace: 0; mso-table-lspace: 0}img{-ms-interpolation-mode: bicubic}.simpleButton{font-family: 'Open Sans', sans-serif !important; font-size: 16px; font-weight: 400; background: #29ca8e; border: 0; border-radius: 50px; color: #fff; padding: 12px 30px; transition: .5s .2s; position: relative; border: 1px solid #eee; background-color: #fff; color: #000; -ms-transition: transform .1s; -o-transition: transform .1s; -webkit-transition: transform .1s; transition: transform .1s; -moz-filter: drop-shadow(5px 5px 8px #213848) !important; -webkit-filter: drop-shadow(5px 5px 8px #213848) !important; filter: drop-shadow(5px 5px 8px #213848) !important}a{color: #00c3d9; text-decoration: none !important}a:active, a:focus, a:hover{color: #00d994; outline: 0}div[style*=""margin: 16px 0;""]{margin: 0 !important}body{width: 100% !important; height: 100% !important; padding: 0 !important; margin: 0 !important}table{border-collapse: collapse !important}img{height: auto; line-height: 100%; text-decoration: none; border: 0; outline: 0}</style> <body style=""color:#f9f9f9""> <div class=""preheader"" style=""display:none;max-width:0;max-height:0;overflow:hidden;font-size:1px;line-height:1px;color:#fff;opacity:0"">A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.</div><table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""> <tr> <td align=""center"" class=""backgroundColor""><!--[if (gte mso 9)|(IE)]><table border=""0""cellpadding=""0""cellspacing=""0""width=""600""align=""center""><tr><td align=""center""valign=""top""width=""600""><![endif]--> <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px""> <tr> <td align=""center"" style=""padding:36px 24px"" valign=""top""> <span> <span style=""font-family:Quicksand,sans-serif;font-size:86px;letter-spacing:10px;color:#213848!important"">play</span> <span style=""color:#00c3d9;font-size:64px;"">.</span> </span> </table><!--[if (gte mso 9)|(IE)]><![endif]--> <tr> <td align=""center"" class=""backgroundColor""><!--[if (gte mso 9)|(IE)]><table border=""0""cellpadding=""0""cellspacing=""0""width=""600""align=""center""><tr><td align=""center""valign=""top""width=""600""><![endif]--> <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px""> <tr> <td align=""left"" class=""formBackgroundColor titleText"" style=""padding:36px 24px 0;border-top:3px solid #f0f0f0""> <h1 style=""text-align:center"">Email Verification</h1> </table><!--[if (gte mso 9)|(IE)]><![endif]--> <tr> <td align=""center"" class=""backgroundColor""><!--[if (gte mso 9)|(IE)]><table border=""0""cellpadding=""0""cellspacing=""0""width=""600""align=""center""><tr><td align=""center""valign=""top""width=""600""><![endif]--> <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px""> <tr> <td align=""left"" class=""formBackgroundColor formText"" style=""padding:24px;font-size:16px;line-height:24px""> <p style=""margin:0"">Tap the button below to confirm your email address. If you didn""t create an account with <a href=""https://paywithplay.com"" style=""font-family:Quicksand,sans-serif"">Play</a>, you can safely delete this email. <tr> <td align=""left"" class=""formBackgroundColor""> <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""> <tr> <td align=""center"" class=""formBackgroundColor"" style=""padding:12px""> <table border=""0"" cellpadding=""0"" cellspacing=""0""> <tr> <td align=""center"" style=""border-radius:6px"" bgcolor=""#1a82e2""> <a href=""https://www.paywithplay.com"" target=""_blank"" class=""simpleButton"">Complete Verification</a> </table> </table> <tr> <td align=""left"" class=""formBackgroundColor formText"" style=""padding:24px;font-family:'Source Sans Pro',Helvetica,Arial,sans-serif;font-size:16px;line-height:24px""> <p style=""margin:0"">If that doesn""t work, copy and paste the following link in your browser: <p style=""margin:0""> <a href=""https://blogdesire.com"" target=""_blank"">https://www.paywithplay.com</a> <tr> <td align=""left"" class=""formBackgroundColor"" style=""padding:24px;font-family:'Source Sans Pro',Helvetica,Arial,sans-serif;font-size:16px;line-height:24px;border-bottom:3px solid #f0f0f0""> <span style=""margin:0;color:#213848!important;font-size:14px;font-weight:400;line-height:24px;font-family:'Source Sans Pro',Helvetica,Arial,sans-serif"">Cheers, <br></span> <span> <span style=""font-family:Quicksand,sans-serif;font-size:24px;letter-spacing:6px;color:#213848!important"">play</span> <span style=""color:#00c3d9;font-weight:700"">.</span> </span> </table><!--[if (gte mso 9)|(IE)]><![endif]--> <tr> <td align=""center"" class=""backgroundColor"" style=""padding:24px""><!--[if (gte mso 9)|(IE)]><table border=""0""cellpadding=""0""cellspacing=""0""width=""600""align=""center""><tr><td align=""center""valign=""top""width=""600""><![endif]--> <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px""> <tr> <td align=""center"" class=""backgroundColor footerText"" style=""padding:12px 24px;font-size:14px;line-height:20px""> <p style=""margin:0"">You received this email because we received a request to create your account. If you didn""t request to create an account you can safely delete this email. <tr> <td align=""center"" class=""backgroundColor footerText"" style=""padding:12px 24px;font-size:14px;line-height:20px""> <p style=""margin:0"">support@paywithplay.com <p style=""margin:0;font-size:14"">Dallas Texas🤠🐮⭐ </table><!--[if (gte mso 9)|(IE)]><![endif]--> </table> <link href=""//fonts.googleapis.com/css?family=Quicksand"" rel=""stylesheet"">";

    #endregion
}

// TODO: Looks like we can use System.New.Mail to send emails
public class EmailAccountVerifier //: IVerifyEmailAccount
{
    #region Instance Members

    //public Task SendVerificationCode(EmailAddress email)
    //{

    //    return Task.CompletedTask;
    //}

    //public Task<bool> VerifyConfirmationCode(string email, int code)
    //{
    //    return (Task<bool>) Task.CompletedTask;
    //}
    public async Task SendVerificationCode()
    {
        var client = new SendGridClient("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");

        client.UrlPath = "smtp.sendgrid.net";

        var from = new EmailAddress("jvaughn@paywithplay.com", "Jesse");

        var to = new EmailAddress("jvaughn@paywithplay.com", "J Dawgie Doggie");

        var message = new SendGridMessage();
        message.SetFrom(from);
        message.HtmlContent = TestEmailVerificationTemplate.Template;

        message.AddTo(to);
        message.SetSubject("Work Mother Licker 3");

        var response = await client.SendEmailAsync(message);

        //message.HtmlContent = TestEmailVerificationTemplate.Template;

        //client.UrlPath = "smtp.sendgrid.net";

        //var response = await client.SendEmailAsync(message);
    }

    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        Console.WriteLine("Fuck Yeah");
    }

    #endregion

    //private readonly ISendEmail _EmailClient; 

    //public EmailAccountVerifier(ISendEmail emailClient)
    //{
    //    _EmailClient = emailClient; 
    //}

    //public static void CreateTestMessage2(string server)
    //{
    //    string to = "jane@contoso.com";
    //    string from = "ben@contoso.com";
    //    MailMessage message = new MailMessage(from, to);
    //    message.Subject = "Using the new SMTP client.";
    //    message.Body = @"Using this new feature, you can send an email message from an application very easily.";
    //    SmtpClient client = new SmtpClient(server);
    //    // Credentials are necessary if the server requires the client
    //    // to authenticate before it will send email on the client's behalf.
    //    client.UseDefaultCredentials = true;

    //    try
    //    {
    //        client.Send(message);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
    //            ex.ToString());
    //    }
    //}
}