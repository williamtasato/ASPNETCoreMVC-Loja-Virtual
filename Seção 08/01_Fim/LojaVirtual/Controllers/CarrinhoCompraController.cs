using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private CarrinhoCompra _carrinhoCompra;
        private IProdutoRepository _produtoRepository;
        private IMapper _mapper;
        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository,IMapper mapper)
        {
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
          List<ProdutoItem> produtoItemNoCarrinho = _carrinhoCompra.Consultar();

            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();
            foreach (var item in produtoItemNoCarrinho)
            {
                Produto produto = _produtoRepository.ObterProduto(item.Id);

                ProdutoItem produtoItem = _mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);

            }

     

            return View(produtoItemCompleto);
        }

        public IActionResult AdicionarItem(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if (produto == null)
            {
                // Produto não existe
                return View("NaoExisteItem");
            }
            else
            {
              var item =  new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = 1 };
                _carrinhoCompra.Cadastrar(item);
                return RedirectToAction(nameof(Index));
            }
           
        }

        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
          Produto produto =  _produtoRepository.ObterProduto(id);
            if(quantidade < 1)
            {

                return BadRequest(new { messagem = Mensagem.MSG_E007});
            }else if(quantidade > produto.Quantidade)
            {

                return BadRequest(new { messagem = Mensagem.MSG_E008 });
            }
            else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = quantidade };
                _carrinhoCompra.Atualizar(item);
                return Ok(new { messagem = Mensagem.MSG_S001 });
            }

          
        }

        public IActionResult RemoverItem(int id)
        {
            _carrinhoCompra.Remover(new ProdutoItem() {Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}