using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace SocialNetworkBLL.Models
{
    public class Perfil 
    {   
        [Key]
        public int PerfilId { get; set; }
        
        public ICollection<Amizade> Seguindo { get; set; }
        public ICollection<Amizade> Seguidores { get; set; }
        
        public Boolean Privado { get; set; }

        [ForeignKey("Usuario")]
        public int UserId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }

        public string ImagemPerfil { get; set; }
    }
}
