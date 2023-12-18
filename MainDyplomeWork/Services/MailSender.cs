using System;
using System.Net;
using System.Net.Mail;

namespace SmartReservationCinema.Services
{
    public class MailSender
    {
        public async void SendMessage(string email,string subject, string text)
        {
            string fromEmail = "smartrevervation12@outlook.com";
            string yourPassword = "sMrtResr1";
            string fromName = "Smart Reservation Cinema :3";
            string toEmail = email;
            //smartreservation11           

            string msgTheme = subject;// args[1];


            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress(fromEmail, fromName);
            // кому отправляем
            MailAddress to = new MailAddress(toEmail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = msgTheme;
            // текст письма
            m.Body = text;
            // письмо представляет код html
            m.IsBodyHtml = false;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);//465
            // логин и пароль
            smtp.Credentials = new NetworkCredential(fromEmail, yourPassword);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(m);
            

        }
    }
}
