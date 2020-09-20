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
        public int Usuario { get; set; }

        [ForeignKey("Perfil")]
        public int Amigo { get; set; }

    }
}
