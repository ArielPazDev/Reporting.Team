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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();

            // Log
            Log.Information("Endpoint access POST api/users");

            return CreatedAtAction("GetUserModel", new { id = userModel.IDUser }, userModel);
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            // Log
            Log.Information("Endpoint access GET api/users");

            return await _context.Users.ToListAsync();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
            var userModel = await _context.Users.FindAsync(id);

            if (userModel == null)
            {
                // Log
                Log.Error($"Endpoint access GET api/users/{id} (not found)");

                return NotFound();
            }
            else
            {
                // Log
                Log.Information($"Endpoint access GET api/users/{id}");
            }

            return userModel;
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.IDUser)
            {
                // Log
                Log.Error($"Endpoint access PUT api/users/{id} (bad request)");

                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Log
                Log.Information($"Endpoint access PUT api/users/{id} (save changes)");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
                {
                    // Log
                    Log.Error($"Endpoint access PUT api/users/{id} (not found)");

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(int id)
        {
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                // Log
                Log.Error($"Endpoint access DELETE api/users/{id} (not found)");

                return NotFound();
            }

            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();

            // Log
            Log.Information($"Endpoint access DELETE api/users/{id} (save changes)");

            return NoContent();
        }

        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.IDUser == id);
        }
    }
}
