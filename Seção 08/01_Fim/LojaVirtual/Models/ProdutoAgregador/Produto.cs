﻿using LojaVirtual.Libraries.Lang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Models.ProdutoAgregador
{
    public class Produto
    {
        public int Id { get; set; }

        [JsonIgnore]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Display(Name ="Descrição")]
        [JsonIgnore]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Display(Name = "Preço")]
        [JsonIgnore]
        public decimal Valor { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Range(0,1000000, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [JsonIgnore]
        public int Quantidade { get; set; }

        //Frete - Correios 
        [Range(0.001, 30, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public double Peso { get; set; }

        [Range(2, 105, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public int Altura { get; set; }

        [Range(11, 105, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public int Largura { get; set; }

        [Range(16, 105, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E006")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public int Comprimento { get; set; }

        //EF - ORM - Biblioteca Unir - Banco de Dados e POO. (ORM - Mapeamento de Objetos <-> Relacionamento)
        //Fluent API - Attributes
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [Display(Name = "Categoria")]
        [JsonIgnore]
        //Banco de Dados - Relacionamento
        public int CategoriaId { get; set; }
        //POO - Associações entre Objetos
        [ForeignKey("CategoriaId")]
        [JsonIgnore]
        public virtual  Categoria Categoria { get; set; }

        [JsonIgnore]
        public virtual ICollection<Imagem> Imagens { get; set; }

    }
}
