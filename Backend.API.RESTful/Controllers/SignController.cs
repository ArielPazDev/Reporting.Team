using Backend.API.RESTful.Context;
using Backend.API.RESTful.DTOs;
using Backend.API.RESTful.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Serilog;
using System.Security.Policy;

namespace Backend.API.RESTful.Controllers
{
    [Route("api/sign")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly DatabaseContext _database;
        private readonly HashService _hash;
        private readonly JwtService _jwt;

        public SignController(DatabaseContext database, HashService hash, JwtService jwt)
        {
            _database = database;
            _hash = hash;
            _jwt = jwt;
        }

        // POST: api/sign/in
        [HttpPost]
        [Route("in")]
        public async Task<ActionResult> Signin(SigninDTO signinDTO)
        {
            var user = await _database.Users
                .Where(u => u.Email == signinDTO.Email && u.Password == _hash.GenerateSHA256(signinDTO.Password))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                // Log
                Log.Error($"Endpoint access GET api/sign/in (the sign in details are incorrect)");

                return BadRequest(new
                {
                    message = "The sign in details are incorrect"
                });
            }
            else
            {
                // Log
                Log.Information("Endpoint access POST api/sign/in");

                return Ok(new
                {
                    message = "The sign in details are correct",
                    name = user.NameFirst + " " + user.NameLast,
                    token = _jwt.GenerateToken(user)
                });
            }
        }
    }
}
