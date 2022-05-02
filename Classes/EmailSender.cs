using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher0._2.Classes
{
    public class EmailSender
    {
        public async Task SendMailAsync(string email, int code)
        {

            MailAddress from = new MailAddress("Polyaksuppord@gmail.com", "Восстановление пароля Venera");
            MailAddress to = new MailAddress(email);
            MailMessage mailmessage = new MailMessage(from, to);

            mailmessage.Subject = "Код подтверждения/восстановления аккаунта Venera";
            mailmessage.Body = $"Ваш код: {code}\nГруппа поддержки Venera....:)";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("Polyaksuppord@gmail.com", "Polyakkrut13");
            smtp.EnableSsl = true;
            try
            {
                await smtp.SendMailAsync(mailmessage);
                mailmessage.Dispose();
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
