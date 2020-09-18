using SocialNetworkBLL.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        
        public DateTime Aniversario { get; set; }
        public string Localidade { get; set; }

        [Required]
        [ForeignKey("Register")]
        public int RegisterId { get; set; }
        public Register Register { get; set; }
    }
}
