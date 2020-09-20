using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkBLL.Models;
using SocialNetworkDLL;

namespace SocialNetworkAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmizadeController : ControllerBase
    {
        private readonly SocialNetworkContext _context;

        public AmizadeController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: api/Amizade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Amizade>>> GetAmizades()
        {
            return await _context.Amizades.ToListAsync();
        }

        // GET: api/Amizade/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amizade>> GetAmizade(int id)
        {
            var amizade = await _context.Amizades.FindAsync(id);

            if (amizade == null)
            {
                return NotFound();
            }

            return amizade;
        }

        // PUT: api/Amizade/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmizade(int id, Amizade amizade)
        {
            if (id != amizade.AmizadeId)
            {
                return BadRequest();
            }

            _context.Entry(amizade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmizadeExists(id))
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

        // POST: api/Amizade
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Amizade>> PostAmizade(Amizade amizade)
        {
            _context.Amizades.Add(amizade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmizade", new { id = amizade.AmizadeId }, amizade);
        }

        // DELETE: api/Amizade/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Amizade>> DeleteAmizade(int id)
        {
            var amizade = await _context.Amizades.FindAsync(id);
            if (amizade == null)
            {
                return NotFound();
            }

            _context.Amizades.Remove(amizade);
            await _context.SaveChangesAsync();

            return amizade;
        }

        private bool AmizadeExists(int id)
        {
            return _context.Amizades.Any(e => e.AmizadeId == id);
        }
    }
}
