using Microsoft.AspNetCore.Http;
using SocialNetworkBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkAPI.Services
{
    public interface IIMagemService
    {
        public Task<string> ImagemPerfil(IFormFile files, Perfil perfilLogado);
        public Task<string> ImagemPost(IFormFile files, Post post);
        public Task<List<string>> Galeria(Perfil perfil);
        
    }
}
