using System;
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

        public PostController(SocialNetworkContext context,
            SignInManager<IdentityUser> signInManager,
            IIMagemService imagemService)
        {
            _context = context;
            _signInManager = signInManager;
            _ImageService = imagemService;
        }

        // GET: api/Post
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
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
        public async Task<ActionResult<Post>> PostPost(Post post, int id, IFormFile files)
        {
            post.Imagem = await _ImageService.ImagemPost(files, post);
            post.PerfilId = id;
            post.DataPost = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();            

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
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
