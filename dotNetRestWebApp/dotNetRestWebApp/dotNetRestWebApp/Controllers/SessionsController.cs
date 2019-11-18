using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNetRestWebApp;
using dotNetRestWebApp.Models;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace dotNetRestWebApp.Controllers
{
    // [Route("api/[controller]")]
    // [ApiController]
    [Produces("application/json")]
    [Route("rest/Sessions")]
    public class SessionsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SessionsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionRec>>> GetSessionRecs()
        {
            Thread.Sleep(200);// artificial delay for UI
            if (_context.SessionRecs.ToList().Count == 0)
            {
                InitSessionsData();
            }
            return await _context.SessionRecs.ToListAsync();
        }

        private void InitSessionsData()
        {
            string file;
            var assembly = Assembly.GetEntryAssembly();
            // string[] resources = assembly.GetManifestResourceNames(); // debugging purposes only to get list of embedded resources
            string[] resources = this.GetType().GetTypeInfo().Assembly.GetManifestResourceNames();
            using (var stream = assembly.GetManifestResourceStream("dotNetRestWebApp.Data.sessions.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    file = reader.ReadToEnd();
                }
            }
            List<SessionRec> sessionRecs = JsonConvert.DeserializeObject<SessionRec[]>(file).ToList();
            foreach (var session in sessionRecs)
            {
                _context.SessionRecs.Add(session);
            }
            _context.SaveChanges();
            return;
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionRec>> GetSessionRec(int id)
        {
            var sessionRec = await _context.SessionRecs.FindAsync(id);

            if (sessionRec == null)
            {
                return NotFound();
            }

            return sessionRec;
        }

        // PUT: api/Sessions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessionRec(int id, SessionRec sessionRec)
        {
            if (id != sessionRec.Id)
            {
                return BadRequest();
            }

            _context.Entry(sessionRec).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionRecExists(id))
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

        // POST: api/Sessions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<SessionRec>> PostSessionRec(SessionRec sessionRec)
        {
            _context.SessionRecs.Add(sessionRec);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSessionRec", new { id = sessionRec.Id }, sessionRec);
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SessionRec>> DeleteSessionRec(int id)
        {
            var sessionRec = await _context.SessionRecs.FindAsync(id);
            if (sessionRec == null)
            {
                return NotFound();
            }

            _context.SessionRecs.Remove(sessionRec);
            await _context.SaveChangesAsync();

            return sessionRec;
        }

        private bool SessionRecExists(int id)
        {
            return _context.SessionRecs.Any(e => e.Id == id);
        }
    }
}
