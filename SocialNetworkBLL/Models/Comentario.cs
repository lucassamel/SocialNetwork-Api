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

        [ForeignKey("Perfil")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
