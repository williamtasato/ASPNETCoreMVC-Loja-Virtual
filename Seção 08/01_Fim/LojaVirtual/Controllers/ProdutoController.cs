using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Controllers
{
    public class ProdutoController : Controller
    {
        /*
         *ActionResult
         *IActionResult
         */
        private ICategoriaRepository _categoriaRepository;
        private IProdutoRepository _produtoRepository;
         public ProdutoController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }
         
        [HttpGet]
        [Route("/Produto/Categoria/{slug}")]
        public ActionResult ListagemCategoria(string slug)
        {

            return View(_categoriaRepository.ObterCategoria(slug));
        }


        /***************************************************************************************************/
        [HttpGet]
        public ActionResult Visualizar(int id)
        {
            // Produto produto =  GetProduto();
            Produto produto = _produtoRepository.ObterProduto(id);
            return View(produto);
           // return new ContentResult() {Content= "<h3>produto -> Visualizar</h3>", ContentType="text/html"};
        }

    }
}
