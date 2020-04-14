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

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private LojaVirtualContext _banco;
        private IClienteRepository _clienteRepository;
        private INewsletterRepository _newsletterRepository;
        public HomeController(IClienteRepository clienteRepository, INewsletterRepository newsletterRepository)
        {
           // _banco = banco;
            _clienteRepository = clienteRepository;
            _newsletterRepository = newsletterRepository;
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

        public IActionResult Login()
        {
            return View();
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