using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SocialNetworkBLL.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Tipo { get; set; }
        public string Corpo { get; set; }
        public DateTime DataPost { get; set; }       

        public IEnumerable<Comentario> Comentarios { get; set; }

        [ForeignKey("Perfil")]
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }
       
    }
}
