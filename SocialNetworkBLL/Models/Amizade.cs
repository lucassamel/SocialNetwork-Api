using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace SocialNetworkBLL.Models
{
    
    public class Amizade
    {
        [Key]
        public int AmizadeId { get; set; }

        public int PerfilId { get; set; }
        [JsonIgnore]
        public Perfil Perfil { get; set; }

        public int PerfilSeguidoId { get; set; }
        [JsonIgnore]
        public Perfil PerfilSeguido { get; set; }

    }
}
