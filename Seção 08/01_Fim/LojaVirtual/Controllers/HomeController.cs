using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LojaVirtual.Database;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Filtro;

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private LoginCliente _loginCliente;
        private IClienteRepository _clienteRepository;
        private INewsletterRepository _newsletterRepository;
        public HomeController(IClienteRepository clienteRepository, INewsletterRepository newsletterRepository, LoginCliente loginCliente)
        {
           // _banco = banco;
            _clienteRepository = clienteRepository;
            _newsletterRepository = newsletterRepository;
            _loginCliente = loginCliente;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm] NewsletterEmail newsletter)
        {
            if (ModelState.IsValid)
            {
                _newsletterRepository.Cadastrar(newsletter);

                 TempData["MSG_S"] = "E-mail cadastrado! Agora você vai receber promoções especiais no seu e-mail! Fique atento as novidades!";
                return RedirectToAction(nameof (Index));
                //return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {

            try
            {
                Contato contato = new Contato();

                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];

                var listaMessagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                bool isValid =  Validator.TryValidateObject(contato, contexto, listaMessagens,true );

                if (isValid)
                {
                    ContatoEmail.EnviarContatoPorEmail(contato);

                    ViewData["MSG_S"] = "Mensagem de contato enviada com sucesso";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var texto in listaMessagens)
                    {
                        sb.Append(texto.ErrorMessage +" <br /> ");
                    }

                    ViewData["MSG_E"] = sb.ToString();
                    ViewData["CONTATO"] = contato;
                }

                
            }
            catch (Exception e)
            {
                ViewData["MSG_E"] = "Ops! Tivemos um erro, tente novamente mais tarde!";
              //TODO - implementar Log
            }

          

            return View("Contato");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {

            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);
           
            if(clienteDB != null)
            {
                //Email, nome, senha, cpf

                //HttpContext.Session.Set("ID", new byte[] {(byte)clienteDB.Id });
                //HttpContext.Session.SetString("Email",cliente.Email );
                //HttpContext.Session.SetInt32("Idade", 29);

                _loginCliente.Login(clienteDB);

                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "O usuário não encontrado, verifique e-mail e senha digitado!";
                return View();

            }
        }

        [HttpGet]
        [ClienteAutorizacaoAttribute]
        public IActionResult Painel()
        {

            return new ContentResult() { Content = "Este é o Painel do Cliente. " };


        }

        [HttpPost]
        public IActionResult CadastroCliente([FromForm] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cliente);
               

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";
                return RedirectToAction(nameof(CadastroCliente));
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}