﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialNetworkAPI.Services;
using SocialNetworkBLL.Models;
using SocialNetworkDLL;

namespace SocialNetworkAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly SocialNetworkContext _context;
        private readonly UserManager<IdentityUser> _userManager;  
        private readonly IIMagemService _ImageService;

        public PerfilController(IIMagemService imagemService,
            SocialNetworkContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _ImageService = imagemService;
        }
        

        // GET: api/Perfil
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfis()
        {
            return await _context.Perfis.ToListAsync();
        }

        // GET: api/Perfil/5
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfil(int id)
        {
            var perfil = await _context.Perfis
                .Include(p => p.Seguindo)
                .Include(p => p.Seguidores)
                .FirstAsync(p => p.PerfilId == id);

            if (perfil == null)
            {
                return NotFound();
            }

            return perfil;
        }

        // PUT: api/Perfil/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public async Task<IActionResult> PutPerfil(int id, Perfil perfil,IFormFile files)
        {
            if (id != perfil.PerfilId)
            {
                return BadRequest();
            }           

            perfil.ImagemPerfil = await _ImageService.ImagemPerfil(files,perfil);          

            _context.Entry(perfil).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Perfil
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult<Perfil>> PostPerfil(Perfil perfil)
        {
            _context.Perfis.Add(perfil);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerfil", new { id = perfil.PerfilId }, perfil);
        }

        // DELETE: api/Perfil/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<ActionResult<Perfil>> DeletePerfil(int id)
        {
            var perfil = await _context.Perfis.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            _context.Perfis.Remove(perfil);
            await _context.SaveChangesAsync();

            return perfil;
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfis.Any(e => e.PerfilId == id);
        }
        
        // GET: api/Perfil/5
        [Authorize]
        [Microsoft.AspNetCore.Mvc.HttpPost("{id}/seguir")]
        public async Task<ActionResult<Perfil>> Seguir(int id)
        {
            var perfil = await _context.Perfis
                .Include(p => p.Seguindo)
                .Include(p => p.Seguidores)
                .FirstOrDefaultAsync(p =>p.PerfilId == id);

            if (perfil == null)
            {
                return NotFound();
            }
            

            var account = await _userManager.GetUserAsync(this.User);

            var perfilLogado = await _context.Perfis
                .Include(p => p.Seguindo)
                .Include(p => p.Seguidores)
                .FirstAsync(p => p.Usuario.Email == account.Email);

            if (perfilLogado.PerfilId == id)
            {
                return Ok(new 
                {
                    perfilSeguido = perfil, perfilLogado
                });
            }
            
            // perfil ja seguiu entao nem precisa adionar
            if (perfilLogado.Seguindo != null && perfilLogado.Seguindo.Any(a => a.PerfilSeguido.PerfilId == id))
            {
                return Ok(new 
                {
                    perfilSeguido = perfil, perfilLogado
                });
            }
            
            var novaAmizade = new Amizade
            {
                Perfil = perfilLogado,
                PerfilSeguido = perfil,
            };
            
            await _context.Amizades.AddAsync(novaAmizade);            
            await _context.SaveChangesAsync();

            return Ok(new 
            {
                perfilSeguido = perfil, perfilLogado
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("{id}/foto-perfil")]
        public async Task<ActionResult<Perfil>> ImagemPerfil(IFormFile files, int id)
        {
            var perfil = await _context.Perfis
                .Include(p => p.Seguindo)
                .Include(p => p.Seguidores)
                .FirstOrDefaultAsync(p => p.PerfilId == id);

            if(perfil == null)
            {
                return NotFound();
            }

            perfil.ImagemPerfil = await _ImageService.ImagemPerfil(files,perfil);
            
            await _context.SaveChangesAsync();

            return Ok(perfil);
        }


        [Microsoft.AspNetCore.Mvc.HttpGet("{id}/galeria")]
        public async Task<ActionResult<IEnumerable<string>>> Galeria(int id)
        {
            var perfil = await _context.Perfis.FindAsync(id);

            if (perfil == null)
            {
                return NotFound();
            }

            var galeria = await _ImageService.Galeria(perfil);

            return galeria;
        }
        
        // GET: api/Perfil/5
        [Authorize]
        [Microsoft.AspNetCore.Mvc.HttpPost("{id}/parar-de-seguir")]
        public async Task<ActionResult<Perfil>> PararSeguir(int id)
        {
            var account = await _userManager.GetUserAsync(this.User);

            var perfilLogado = await _context.Perfis
                .Include(p => p.Seguindo)
                .Include(p => p.Seguidores)
                .FirstAsync(p => p.Usuario.Email == account.Email);
            
            var perfil = await _context.Perfis
                .Include(p => p.Seguindo)
                .Include(p => p.Seguidores)
                .FirstOrDefaultAsync(p =>p.PerfilId == id);

            if (perfil == null)
            {
                return NotFound();
            }

            if (perfilLogado.PerfilId == id)
            {
                return Ok(new 
                {
                    perfilSeguido = perfil, perfilLogado
                });
            }
            
            var amizade = perfilLogado
                .Seguindo
                .FirstOrDefault(a => a.PerfilSeguidoId == id);

            if (amizade == null)
            {
                return Ok(new 
                {
                    perfilSeguido = perfil, perfilLogado
                }); 
            }

            _context.Amizades.Remove(amizade);
            await _context.SaveChangesAsync();

            return Ok(new 
            {
                perfilSeguido = perfil, perfilLogado
            });
        }
    }
}
