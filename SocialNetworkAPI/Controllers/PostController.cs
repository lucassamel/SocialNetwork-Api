﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkAPI.Services;
using SocialNetworkBLL.Models;
using SocialNetworkDLL;
using SQLitePCL;

namespace SocialNetworkAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly SocialNetworkContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIMagemService _ImageService;
        private readonly UserManager<IdentityUser> _userManager;

        public PostController(SocialNetworkContext context,
            SignInManager<IdentityUser> signInManager,
            IIMagemService imagemService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _ImageService = imagemService;
            _userManager = userManager;

        }

        // GET: api/Post
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            //var x = from w in _context.Posts
            //        select new Post
            //        {
            //            Corpo = w.Corpo,
            //            Comentarios = w.Comentarios.ToList(),
            //            CountFake = w.CountFake,
            //            CountFato = w.CountFato,
            //            DataPost = w.DataPost,
            //            Imagem = w.Imagem,
            //            Perfil = w.Perfil,
            //            PerfilId = w.PerfilId,
            //            PostId = w.PostId,

            //        };

            //var posts = _context.Posts.ToList().AsTracking<Comentario>();
            //foreach (var post in posts)
            //{
            //    post.Comentarios = 
            //}

            //return await/* x.ToListAsync()*/;

            return await _context.Posts.               
                Include(p => p.Perfil).
                Include(p => p.Perfil.Usuario).
                Include(p => p.Comentarios).
                ThenInclude(c => c.Perfil).
                Include(c => c.Perfil.Usuario)
                .ToListAsync();
        }

        // GET: api/Post/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post, IFormFile files)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }          

            post.Imagem = await _ImageService.ImagemPost(files,post);

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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


        // POST: api/Post
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            var account = await _userManager.GetUserAsync(this.User);
            var perfilLogado = await _context.Perfis
                .FirstAsync(p => p.Usuario.Email == account.Email);

            post.PerfilId = perfilLogado.PerfilId;
            post.DataPost = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            //var teste = _context.Comentarios.
            //     Include(p => p.ComentarioId);
                

            return CreatedAtAction("GetPost", new { id = post.PostId }, post );
        }

        [HttpPost("{id}/fato")]
        public async Task<ActionResult<Post>> Fato(int id)
        {
                var post = await _context.Posts.
                Include(p => p.Perfil).
                Include(p => p.Perfil.Usuario).
                Include(p => p.Comentarios).
                ThenInclude(c => c.Perfil).
                Include(c => c.Perfil.Usuario)
                .SingleAsync(p => p.PostId == id);

            post.CountFato += 1;


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost",post);
        }

        [HttpPost("{id}/fake")]
        public async Task<ActionResult<Post>> Fake(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            post.CountFake += 1;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", post);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("{id}/foto-post")]
        public async Task<ActionResult<Post>> ImagemPost(IFormFile files, int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if(post == null)
            {
                return NotFound();
            }

            post.Imagem = await _ImageService.ImagemPost(files, post);

            await _context.SaveChangesAsync();

            return Ok();
        }


        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
