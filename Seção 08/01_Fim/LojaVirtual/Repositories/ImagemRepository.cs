using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private LojaVirtualContext _banco;

        public ImagemRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }
        public void CadastrarImagens(List<Imagem> ListaImagens, int ProdutoId)
        {
            if (ListaImagens != null && ListaImagens.Count > 0)
            {
                foreach (var imagem in ListaImagens)
                {
                    _banco.Imagens.Add(imagem);
                }
                _banco.SaveChanges();
            }    
        }
        public void Cadastrar(Imagem imagem)
        {
            _banco.Imagens.Add(imagem);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Imagem imagem = _banco.Imagens.Find (Id);
            _banco.Imagens.Remove(imagem);
            _banco.SaveChanges();
        }

        public void ExcluirImagensDoProduto(int ProdutoId)
        {
           List <Imagem> imagens = _banco.Imagens.Where(p => p.ProdutoId == ProdutoId).ToList();
            foreach (Imagem imagem in imagens)
            {
                _banco.Imagens.Remove(imagem);
            }
            _banco.SaveChanges();
        }
    }
}
