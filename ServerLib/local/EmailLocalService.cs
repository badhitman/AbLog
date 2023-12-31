﻿////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using MailKit.Net.Smtp;
using MailKit.Security;
using SharedLib;
using MimeKit;
using MailKit;

namespace ServerLib;

/// <summary>
/// Email отправка/получение
/// </summary>
public class EmailLocalService : IEmailService, IDisposable
{
    SmtpClient? client_smtp = null;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConnectSmtpAsync(EmailConfigModel conf)
    {
        if (client_smtp?.IsConnected == true)
        {
            client_smtp.Disconnect(true);
            client_smtp.Dispose();
        }

        ResponseBaseModel res = new();

        client_smtp = conf.LogsSmtp
           ? new SmtpClient(new ProtocolLogger(GlobalStatic.LogsSmtpPath))
           : new SmtpClient();

        if (conf.UseSslSmtp == SecureSocketOptionsEnum.SelfSignedSsl)
        {
            client_smtp.ServerCertificateValidationCallback = (_, __, ___, ____) => true;
            client_smtp.CheckCertificateRevocation = false;
        }

        SecureSocketOptions _sec_opt = conf.UseSslSmtp switch
        {
            SecureSocketOptionsEnum.StartTls => SecureSocketOptions.StartTls,
            SecureSocketOptionsEnum.None => SecureSocketOptions.None,
            SecureSocketOptionsEnum.StartTlsWhenAvailable => SecureSocketOptions.StartTlsWhenAvailable,
            SecureSocketOptionsEnum.SslOnConnect => SecureSocketOptions.SslOnConnect,
            _ => SecureSocketOptions.Auto,
        };

        try
        {
            await client_smtp.ConnectAsync(conf.HostSmtp, conf.PortSmtp, _sec_opt);
            await client_smtp.AuthenticateAsync(conf.Login, conf.Password);
            res.AddSuccess("SMTP подключение: Ok");
            return res;
        }
        catch (Exception ex)
        {
            return res.AddError(ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendEmailAsync(string email, string subject, string message, bool html_format, EmailConfigModel conf)
    {
        using MimeMessage emailMessage = new();

        emailMessage.From.Add(new MailboxAddress(conf.Email, conf.Email));
        emailMessage.To.Add(new MailboxAddress(email, email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(html_format ? MimeKit.Text.TextFormat.Plain : MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        ResponseBaseModel res = await ConnectSmtpAsync(conf);
        if (!res.IsSuccess)
            return res;

        try
        {
            res.AddInfo(await client_smtp!.SendAsync(emailMessage));
            return res;
        }
        catch (Exception ex)
        {
            return res.AddError(ex.Message);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        client_smtp?.Disconnect(true);
        client_smtp?.Dispose();
    }
}