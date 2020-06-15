using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Database;
using LojaVirtual.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using X.PagedList;

namespace LojaVirtual.Repositories.Contracts
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private LojaVirtualContext _banco;
        private IConfiguration _conf;

        public CategoriaRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Atualizar(Categoria categoria)
        {
            _banco.Categorias.Update(categoria);
            _banco.SaveChanges();
        }

        public void Cadastrar(Categoria categoria)
        {
            _banco.Categorias.Add(categoria);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Categoria categoria = ObterCategoria(Id);
            _banco.Categorias.Remove(categoria);
            _banco.SaveChanges();
        }

        public Categoria ObterCategoria(int Id)
        {
            return _banco.Categorias.Find(Id);
        }

        public Categoria ObterCategoria(string Slug)
        {
            return _banco.Categorias.Where(a => a.Slug == Slug).FirstOrDefault();
        }

        List<Categoria> Categorias;
        private List<Categoria> ListaCategoriaRecursiva = new List<Categoria>();
        public IEnumerable<Categoria> ObterCategoriaRecursivas(Categoria categoriaPai)
        {
            if(Categorias == null)
            {
                Categorias = ObterTodasCategorias().ToList();
            }

            if (!ListaCategoriaRecursiva.Exists(c => c.Id == categoriaPai.Id))
            {
                ListaCategoriaRecursiva.Add(categoriaPai);
            }

            var ListaCategoriaFilho = Categorias.Where(c => c.CategoriaPaiId == categoriaPai.Id);
            if (ListaCategoriaFilho.Count() > 0)
            {
                ListaCategoriaRecursiva.AddRange(ListaCategoriaFilho.ToList());
                foreach (var categoria in ListaCategoriaFilho)
                {
                    ObterCategoriaRecursivas(categoria);
                }
            }
            return ListaCategoriaRecursiva;
        }

       
           public IPagedList<Categoria> ObterTodasCategorias(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return _banco.Categorias.Include(a => a.CategoriaPai).ToPagedList<Categoria>(NumeroPagina, _conf.GetValue<int>("RegistroPorPagina"));
        }

        public IEnumerable<Categoria> ObterTodasCategorias()
        {
            return _banco.Categorias;
        }
    }
}
