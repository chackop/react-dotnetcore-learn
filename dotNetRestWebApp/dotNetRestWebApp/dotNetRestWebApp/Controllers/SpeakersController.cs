using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNetRestWebApp;
using dotNetRestWebApp.Models;

namespace dotNetRestWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SpeakersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Speakers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpeakerRec>>> GetSpeakerRecs()
        {
            return await _context.SpeakerRecs.ToListAsync();
        }

        // GET: api/Speakers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpeakerRec>> GetSpeakerRec(int id)
        {
            var speakerRec = await _context.SpeakerRecs.FindAsync(id);

            if (speakerRec == null)
            {
                return NotFound();
            }

            return speakerRec;
        }

        // PUT: api/Speakers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpeakerRec(int id, SpeakerRec speakerRec)
        {
            if (id != speakerRec.Id)
            {
                return BadRequest();
            }

            _context.Entry(speakerRec).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeakerRecExists(id))
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

        // POST: api/Speakers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<SpeakerRec>> PostSpeakerRec(SpeakerRec speakerRec)
        {
            _context.SpeakerRecs.Add(speakerRec);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpeakerRec", new { id = speakerRec.Id }, speakerRec);
        }

        // DELETE: api/Speakers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SpeakerRec>> DeleteSpeakerRec(int id)
        {
            var speakerRec = await _context.SpeakerRecs.FindAsync(id);
            if (speakerRec == null)
            {
                return NotFound();
            }

            _context.SpeakerRecs.Remove(speakerRec);
            await _context.SaveChangesAsync();

            return speakerRec;
        }

        private bool SpeakerRecExists(int id)
        {
            return _context.SpeakerRecs.Any(e => e.Id == id);
        }
    }
}
