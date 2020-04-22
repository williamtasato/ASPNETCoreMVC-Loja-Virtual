using LojaVirtual.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Email
{
    public class GerenciarEmail
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;

        public GerenciarEmail(SmtpClient smtp, IConfiguration configuration) {
            _smtp = smtp;
            _configuration = configuration;
        }

        public  void EnviarContatoPorEmail(Contato contato)
        {
            string corpoMsg = string.Format ("<h2>Contato - Loja Virtual</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>Email: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                " <br /> E-mail enviado automáticamente pelo site LojaVirtual.", contato.Nome, contato.Email, contato.Texto);

            /*
             * MailMessage Construir a Mensagem a ser enviada
             */

            MailMessage messagem = new MailMessage();
            messagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            messagem.To.Add("william@cisistemas.com.br");
            messagem.Subject = "Contato - Loja Virtual - E-mail: " + contato.Email;
            messagem.Body = corpoMsg;
            messagem.IsBodyHtml = true;

            //Enviar messagem via smtp
            _smtp.Send(messagem);
        }

        public void EnviarSenhaParaColaboradorPorEmail(Colaborador colaborador) {
            string corpoMsg = string.Format("<h2>Colaborador - Loja Virtual</h2> "
                + "Sua senha é : " +
                "<h3>{0}</h3>", colaborador.Senha);

            /*
             * MailMessage Construir a Mensagem a ser enviada
             */

            MailMessage messagem = new MailMessage();
            messagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            messagem.To.Add(colaborador.Email);
            messagem.Subject = "Colaborador - Loja Virtual - Senha do colaborador: " + colaborador.Nome;
            messagem.Body = corpoMsg;
            messagem.IsBodyHtml = true;

            //Enviar messagem via smtp
            _smtp.Send(messagem);
        }
    }
}
