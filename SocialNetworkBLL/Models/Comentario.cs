using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetworkBLL.Models
{
    public class Comentario 
    {
        [Key]
        public int ComentarioId { get; set; }
        public string Coment { get; set; }
        public DateTime Data { get; set; }     
        
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("Perfil")]
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }
    }
}
