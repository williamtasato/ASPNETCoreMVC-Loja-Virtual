using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Email
{
    public class ContatoEmail
    {
        public static void EnviarContatoPorEmail(Contato contato)
        {
            /*
             *  SMTP -> Servidor que vai enviar a mensagem.
             */
            SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new  NetworkCredential("william@cisistemas.com.br", "");
            smtp.EnableSsl = true;

            string corpoMsg = string.Format ("<h2>Contato - Loja Virtual</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>Email: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                " <br /> E-mail enviado automáticamente pelo site LojaVirtual.", contato.Nome, contato.Email, contato.Texto);

            /*
             * MailMessage Construir a Mensagem a ser enviada
             */

            MailMessage messagem = new MailMessage();
            messagem.From = new MailAddress("william@cisistemas.com.br");
            messagem.To.Add("william@cisistemas.com.br");
            messagem.Subject = "Contato - Loja Virtual - E-mail: " + contato.Email;
            messagem.Body = corpoMsg;
            messagem.IsBodyHtml = true;

            //Enviar messagem via smtp
            smtp.Send(messagem);
        }
    }
}
