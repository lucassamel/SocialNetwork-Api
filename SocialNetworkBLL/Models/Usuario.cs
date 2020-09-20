﻿using SocialNetworkBLL.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SocialNetworkBLL.Models
{
    public class Usuario 
    {
        [Key]
        public int UsuarioId { get; set; }
        
        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Sobrenome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime? Aniversario { get; set; }
        
        public string Localidade { get; set; }
        
        public Perfil Perfil { get; set; }
    }
}
