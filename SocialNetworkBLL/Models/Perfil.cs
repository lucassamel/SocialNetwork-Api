using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SocialNetworkBLL.Models
{
    public class Perfil 
    {   
        [Key]
        public int PerfilId { get; set; }
        public IEnumerable<Perfil> Amizades { get; set; }
        public Boolean Privado { get; set; }

        [ForeignKey("Usuario")]
        public int UserId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
