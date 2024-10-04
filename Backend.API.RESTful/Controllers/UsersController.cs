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
using Backend.API.RESTful.Services;

namespace Backend.API.RESTful.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _database;
        private readonly HashService _hash;

        public UsersController(DatabaseContext database, HashService hash)
        {
            _database = database;
            _hash = hash;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
            // Password SHA256
            userModel.Password = _hash.generateSHA256(userModel.Password);

            _database.Users.Add(userModel);
            await _database.SaveChangesAsync();

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

            return await _database.Users.ToListAsync();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
            var userModel = await _database.Users.FindAsync(id);

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

            _database.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _database.SaveChangesAsync();

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
            var userModel = await _database.Users.FindAsync(id);
            if (userModel == null)
            {
                // Log
                Log.Error($"Endpoint access DELETE api/users/{id} (not found)");

                return NotFound();
            }

            _database.Users.Remove(userModel);
            await _database.SaveChangesAsync();

            // Log
            Log.Information($"Endpoint access DELETE api/users/{id} (save changes)");

            return NoContent();
        }

        private bool UserModelExists(int id)
        {
            return _database.Users.Any(e => e.IDUser == id);
        }
    }
}
