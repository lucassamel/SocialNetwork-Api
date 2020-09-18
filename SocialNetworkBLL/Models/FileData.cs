﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SocialNetworkBLL.Models
{
    public class FileData
    {

        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string ModifiedOn { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }

}
