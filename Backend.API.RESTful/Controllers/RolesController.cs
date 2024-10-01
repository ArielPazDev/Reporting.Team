using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.API.RESTful.Context;
using Backend.API.RESTful.Models;
using Serilog;

namespace Backend.API.RESTful.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/roles
        [HttpPost]
        public async Task<ActionResult<RolModel>> PostRolModel(RolModel rolModel)
        {
            _context.Roles.Add(rolModel);
            await _context.SaveChangesAsync();

            // Log
            Log.Information("Endpoint access POST api/roles");

            return CreatedAtAction("GetRolModel", new { id = rolModel.IDRol }, rolModel);
        }

        // GET: api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolModel>>> GetRoles()
        {
            // Log
            Log.Information("Endpoint access GET api/roles");

            return await _context.Roles.ToListAsync();
        }

        // GET: api/roles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RolModel>> GetRolModel(int id)
        {
            var rolModel = await _context.Roles.FindAsync(id);

            if (rolModel == null)
            {
                // Log
                Log.Error($"Endpoint access GET api/roles/{id} (not found)");

                return NotFound();
            }
            else
            {
                // Log
                Log.Information($"Endpoint access GET api/roles/{id}");
            }

            return rolModel;
        }

        // PUT: api/roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolModel(int id, RolModel rolModel)
        {
            if (id != rolModel.IDRol)
            {
                // Log
                Log.Error($"Endpoint access PUT api/roles/{id} (bad request)");

                return BadRequest();
            }

            _context.Entry(rolModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Log
                Log.Information($"Endpoint access PUT api/roles/{id} (save changes)");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolModelExists(id))
                {
                    // Log
                    Log.Error($"Endpoint access PUT api/roles/{id} (not found)");

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolModel(int id)
        {
            var rolModel = await _context.Roles.FindAsync(id);
            if (rolModel == null)
            {
                // Log
                Log.Error($"Endpoint access DELETE api/roles/{id} (not found)");

                return NotFound();
            }

            if (_context.Users.Any(e => e.IDRol == id))
            {
                // Log
                Log.Error($"Endpoint access DELETE api/roles/{id} (the role {id} cannot be deleted, if there are user/s with this role assigned)");

                return Conflict(new { message = $"The role {id} cannot be deleted, if there are user/s with this role assigned" });
            }

            _context.Roles.Remove(rolModel);
            await _context.SaveChangesAsync();

            // Log
            Log.Information($"Endpoint access DELETE api/roles/{id} (save changes)");

            return NoContent();
        }

        private bool RolModelExists(int id)
        {
            return _context.Roles.Any(e => e.IDRol == id);
        }
    }
}
