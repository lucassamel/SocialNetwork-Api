using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SocialNetworkBLL.Models
{
    
    public class Amizade
    {
        [Key]
        public int AmizadeId { get; set; }

        [ForeignKey("Perfil")]
        public int PerfilUsuarioId { get; set; }
        public Perfil PerfilUsuario { get; set; }

        [ForeignKey("Perfil")]
        public int PerfilAmigoId { get; set; }
        public Perfil PerfilAmigo { get; set; }

    }
}
