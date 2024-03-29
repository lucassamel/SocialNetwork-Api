﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SocialNetworkBLL.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }        
        public string Corpo { get; set; }
        public DateTime DataPost { get; set; }
        public string Imagem { get; set; }

        [DefaultValue(0)]
        public int CountFato { get; set; }
        [DefaultValue(0)]
        public int CountFake { get; set; }
            
        public IEnumerable<Comentario> Comentarios { get; set; }

        [ForeignKey("Perfil")]
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }  

    }
}
