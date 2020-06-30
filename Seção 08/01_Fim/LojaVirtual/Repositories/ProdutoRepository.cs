using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace LojaVirtual.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private LojaVirtualContext _banco;
        private IConfiguration _conf;

        public ProdutoRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }

        public void Atualizar(Produto produto)
        {
            _banco.Produtos.Update(produto);
            _banco.SaveChanges();
        }

        public void Cadastrar(Produto produto)
        {
            _banco.Produtos.Add(produto);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Produto produto = ObterProduto(Id);
            _banco.Produtos.Remove(produto);
            _banco.SaveChanges();
        }

        public Produto ObterProduto(int Id)
        {
            return _banco.Produtos.Include(a => a.Imagens).OrderBy(a => a.Nome).Where(a => a.Id  ==Id).FirstOrDefault();
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa)
        {
          return ObterTodosProdutos(pagina, pesquisa, "A", null);
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa, string ordenacao, IEnumerable<Categoria> categorias)
        {
            int NumeroPagina = pagina ?? 1;
            var bancoProdutos = _banco.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                bancoProdutos = bancoProdutos.Where(c => c.Nome.Contains(pesquisa.Trim()));
            }

            if(ordenacao == "A")
            {
                bancoProdutos = bancoProdutos.OrderBy(a => a.Nome);
            }

            if (ordenacao == "MA")
            {
                bancoProdutos = bancoProdutos.OrderByDescending(a => a.Valor );
            }

            if (ordenacao == "ME")
            {
                bancoProdutos = bancoProdutos.OrderBy(a => a.Valor);
            }

            if(categorias != null && categorias.Count() > 0)
            {
                // 1 - Informática - 5 - Teclado IEnumerable Categorias
                //SQL Where CategoriaId IN (1,5,....)
                bancoProdutos = bancoProdutos.Where(a => categorias.Select(b => b.Id).Contains(a.CategoriaId));
            }

            return bancoProdutos.Include(a => a.Imagens).ToPagedList<Produto>(NumeroPagina, _conf.GetValue<int>("RegistroPorPagina"));
        }
    }
}
