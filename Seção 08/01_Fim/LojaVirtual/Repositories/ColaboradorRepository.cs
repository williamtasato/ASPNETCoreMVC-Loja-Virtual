using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace LojaVirtual.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private const int _registroPorPagina = 10;
        private LojaVirtualContext _banco;
        private IConfiguration _conf;

        public ColaboradorRepository(LojaVirtualContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Atualizar(Colaborador colaborador)
        {
            _banco.Colaboradores.Update(colaborador);
            _banco.Entry(colaborador).Property(c => c.Senha).IsModified = false;
            _banco.SaveChanges();
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            _banco.Colaboradores.Update(colaborador);
            _banco.Entry(colaborador).Property(c => c.Nome).IsModified = false;
            _banco.Entry(colaborador).Property(c => c.Email).IsModified = false;
            _banco.Entry(colaborador).Property(c => c.Tipo).IsModified = false;
            _banco.SaveChanges();
        }

        public void Cadastrar(Colaborador colaborador)
        {
            _banco.Colaboradores.Add(colaborador);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            Colaborador colaborador = ObterColaborador(Id);
            _banco.Colaboradores.Remove(colaborador);
            _banco.SaveChanges();
        }

        public Colaborador Login(string Email, string Senha)
        {
            return _banco.Colaboradores.Where(m => m.Email == Email && m.Senha == Senha).FirstOrDefault();
        }

        public Colaborador ObterColaborador(int Id)
        {
            return _banco.Colaboradores.Find(Id);
        }

        public List<Colaborador> ObterColaboradorPorEmail(string email)
        {
            return _banco.Colaboradores.Where(c => c.Email == email).AsNoTracking().ToList();
        }

        public IEnumerable<Colaborador> ObterTodosColaboradores()
        {
            return _banco.Colaboradores.Where(c => c.Tipo != "G").ToList();
        }

        public IPagedList<Colaborador> ObterTodosColaboradores(int? pagina)
        {
            int NumeroPagina = pagina ?? 1;
            return _banco.Colaboradores.Where(c => c.Tipo != "G").ToPagedList<Colaborador>(NumeroPagina, _conf.GetValue<int> ("RegistroPorPagina"));
        }
    }
}
