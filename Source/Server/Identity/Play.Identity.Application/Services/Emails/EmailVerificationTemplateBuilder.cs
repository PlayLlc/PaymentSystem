﻿using Play.Telecom.Twilio.Email;

namespace Play.Identity.Application.Services;

public class EmailVerificationTemplateBuilder : EmailTemplateBuilder
{
    #region Static Metadata

    // HACK: we're going to load this from a text file when the builder is created
    protected const string _Template =
        "<!DOCTYPE html><html><head> <meta charset=\"\"utf-8\"\"> <meta http-equiv=\"\"x-ua-compatible\"\" content=\"\"ie=edge\"\"> <title>Email Confirmation</title> <meta name=\"\"viewport\"\" content=\"\"width=device-width, initial-scale=1\"\"> <link rel=\"\"stylesheet\"\" type=\"\"text/css\"\" href=\"\"//fonts.googleapis.com/css?family=Quicksand\"\"/> <style>.backgroundColor{background-color: #f9f9f9;}.formBackgroundColor{background-color: #F4F6F9;}.formText{font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;}.footerText{font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; color: #213848;}h1, h2, h3, h4, h5, h6{font-weight: 300; line-height: normal; color: #213848;}h1{font-size: 3em; color: #213848;}h2{color: #213848; font-size: 2em;}p{color: #213848; font-size: 14px; font-weight: normal; line-height: 24px;}.titleText{color: #213848; font-family: 'Open Sans', sans-serif;}@media screen{@font-face{font-family: 'Source Sans Pro'; font-style: normal; font-weight: 400; src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');}@font-face{font-family: 'Source Sans Pro'; font-style: normal; font-weight: 700; src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');}}body, table, td, a{-ms-text-size-adjust: 100%; /* 1 */ -webkit-text-size-adjust: 100%; /* 2 */}table, td{mso-table-rspace: 0pt; mso-table-lspace: 0pt;}img{-ms-interpolation-mode: bicubic;}a.simpleButton:hover, a:active, a:focus{color: #ffffff; background: #00C3D9; outline: none;}.simpleButton{font-family: 'Open Sans', sans-serif !important; font-size: 16px; font-weight: normal; background: #29ca8e; border: 0; border-radius: 50px; color: #ffffff; padding: 12px 30px;}a{color: #00C3D9; text-decoration: none !important;}a:hover, a:active, a:focus{color: #00D994; outline: none;}div[style*=\"\"margin: 16px 0;\"\"]{margin: 0 !important;}body{width: 100% !important; height: 100% !important; padding: 0 !important; margin: 0 !important;}table{border-collapse: collapse !important;}img{height: auto; line-height: 100%; text-decoration: none; border: 0; outline: none;}</style></head><body style=\"\"color:#f9f9f9\"\"> <div class=\"\"preheader\"\" style=\"\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\"\"> Welcome to Play! </div><table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\" width=\"\"100%\"\"> <tr> <td align=\"\"center\"\" class=\"\"backgroundColor\"\"> <table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\" width=\"\"100%\"\" style=\"\"max-width: 600px;\"\"> <tr> <td align=\"\"center\"\" valign=\"\"top\"\" style=\"\"padding: 36px 24px;\"\"> <span><span style=\"\"font-family: 'Quicksand', sans-serif; font-size: 24px; letter-spacing: 6px; color: #213848 !important\"\">Play</span><span style=\"\"color:#00C3D9; font-weight: bold;\"\">.</span></span> </td></tr></table> </td></tr><tr> <td align=\"\"center\"\" class=\"\"backgroundColor\"\"> <table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\" width=\"\"100%\"\" style=\"\"max-width: 600px;\"\"> <tr> <td align=\"\"left\"\" class=\"\"formBackgroundColor titleText\"\" style=\"\"padding: 36px 24px 0; border-top: 3px solid #f0f0f0;\"\"> <h1 style=\"\" text-align: center;\"\"> Email Verification </h1> </td></tr></table> </td></tr><tr> <td align=\"\"center\"\" class=\"\"backgroundColor\"\"> <table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\" width=\"\"100%\"\" style=\"\"max-width: 600px;\"\"> <tr> <td align=\"\"left\"\" class=\"\"formBackgroundColor formText\"\" style=\"\"padding: 24px; font-size: 16px; line-height: 24px;\"\"> <p style=\"\"margin: 0;\"\">Tap the button below to confirm your email address. If you didn\"\"t create an account with <a style=\"\"font-family: 'Quicksand', sans-serif; \"\" href=\"\"https://paywithplay.com\"\">Play</a>, you can safely delete this email.</p></td></tr><tr> <td align=\"\"left\"\" class=\"\"formBackgroundColor\"\"> <table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\" width=\"\"100%\"\"> <tr> <td align=\"\"center\"\" class=\"\"formBackgroundColor\"\" style=\"\"padding: 12px;\"\"> <table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\"> <tr> <td align=\"\"center\"\" bgcolor=\"\"#1a82e2\"\" style=\"\"border-radius: 6px;\"\"> <a class=\"\"simpleButton\"\" href=\"\"\"\" target=\"\"_blank\"\">Complete Verification</a> </td></tr></table> </td></tr></table> </td></tr><tr> <td align=\"\"left\"\" class=\"\"formBackgroundColor formText\"\" style=\"\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\"\"> <p style=\"\"margin: 0;\"\">If that doesn\"\"t work, copy and paste the following link in your browser:</p><p style=\"\"margin: 0;\"\"><a href=\"\"https://blogdesire.com\"\" target=\"\"_blank\"\"></a></p></td></tr><tr> <td align=\"\"left\"\" class=\"\"formBackgroundColor\"\" style=\"\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #f0f0f0\"\"> <span style=\"\"margin: 0; color: #213848 !important; font-size: 14px; font-weight: normal; line-height: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; \"\">Cheers,<br></span> <span><span style=\"\"font-family: 'Quicksand', sans-serif; font-size: 24px; letter-spacing: 6px; color: #213848 !important\"\">Play</span><span style=\"\"color:#00C3D9; font-weight: bold;\"\">.</span></span> </td></tr></table> </td></tr><tr> <td align=\"\"center\"\" class=\"\"backgroundColor\"\" style=\"\"padding: 24px;\"\"> <table border=\"\"0\"\" cellpadding=\"\"0\"\" cellspacing=\"\"0\"\" width=\"\"100%\"\" style=\"\"max-width: 600px;\"\"> <tr> <td align=\"\"center\"\" class=\"\"backgroundColor footerText\"\" style=\"\"padding: 12px 24px; font-size: 14px; line-height: 20px;\"\"> <p style=\"\"margin: 0;\"\">You received this email because we received a request to create your account. If you didn't request to create an account you can safely delete this email.</p></td></tr><tr> <td align=\"\"center\"\" class=\"\"backgroundColor footerText\"\" style=\"\"padding: 12px 24px; font-size: 14px; line-height: 20px;\"\"> <p style=\"\"margin:0;\"\">support@paywithplay.com</p><p style=\"\"margin: 0; font-size: 14px\"\">Dallas Texas\U0001f920🐮⭐</p></td></tr></table> </td></tr></table> </body> </html>";

    #endregion

    #region Constructor

    public EmailVerificationTemplateBuilder() : base("Please Verify Your Email Address")
    { }

    #endregion

    #region Instance Members

    protected override string GetTemplate() => _Template;

    public string CreateEmail(string callbackUri) => CreateMessage(new() {{nameof(callbackUri), callbackUri}});

    #endregion
}