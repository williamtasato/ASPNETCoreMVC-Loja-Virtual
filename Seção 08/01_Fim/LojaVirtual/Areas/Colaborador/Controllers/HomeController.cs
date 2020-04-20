using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using LojaVirtual.Models;
using LojaVirtual.Libraries.Filtro;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private LoginColaborador _loginColaborador;
        private IColaboradorRepository _colaboradorRepository;
        public HomeController(IColaboradorRepository colaboradorRepository,LoginColaborador loginColaborador)
        {
            _colaboradorRepository = colaboradorRepository;
            _loginColaborador = loginColaborador;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LojaVirtual.Models.Colaborador colaborador)
        {

            Models.Colaborador colaboradorDB = _colaboradorRepository.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDB != null)
            {
                //Email, nome, senha, cpf

                //HttpContext.Session.Set("ID", new byte[] {(byte)clienteDB.Id });
                //HttpContext.Session.SetString("Email",cliente.Email );
                //HttpContext.Session.SetInt32("Idade", 29);

                _loginColaborador.Login(colaboradorDB);

                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "O usuário não encontrado, verifique e-mail e senha digitado!";
                return View();

            }
        }

        [ColaboradorAutorizacao]
        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction("Login","Home");
        }

        [HttpGet]
        [ColaboradorAutorizacao]
        public IActionResult Painel()
        {

            return View();


        }

        public IActionResult RecuperarSenha()
        {
            return View();
        }

        public IActionResult CadastrarNovaSenha()
        {
            return View();
        }
    }
}