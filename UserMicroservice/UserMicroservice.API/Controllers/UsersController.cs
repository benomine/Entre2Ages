using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using UserMicroservice.Domain.Models;
using UserMicroservice.EntityFramework;

namespace UserMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContextFactory _contextFactory;
        private readonly JwtSettings _settings;

        public UsersController(UserDbContextFactory contextFactory, JwtSettings settings)
        {
            _contextFactory = contextFactory;
            _settings = settings;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Get all users",
            Description = "Return a list of all users",
            OperationId = "UserController.GetAll",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var context = _contextFactory.CreateDbContext();
            return await context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:Guid}")]
        [SwaggerOperation(
            Summary = "Get a user with id",
            Description = "Get a user with id",
            OperationId = "UserController.GetById",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/email/
        [HttpGet("{email}/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Get a user by email",
            Description = "Get a user by email",
            OperationId = "UserController.GetByEmail",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Update a user with his id",
            Description = "Update a user with his id",
            OperationId = "UserController.Update",
            Tags = new []{"UserController"}
        )]
        public async Task<IActionResult> Update(Guid id, User update)
        {
            if (id != update.Id)
            {
                return BadRequest();
            }

            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = update.Name;
            user.Password = update.Password;
            user.Phone = update.Phone;
            user.Phone2 = update.Phone2;
            user.Address = update.Address;
            user.City = update.City;
            user.ZipCode = update.ZipCode;
            user.Email = update.Email;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Create a user",
            Description = "Create a user",
            OperationId = "UserController.Post",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<User>> Post(User postUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var user = new User()
            {
                Name = postUser.Name,
                Password = postUser.Password,
                Phone = postUser.Phone,
                Phone2 = postUser.Phone2,
                Address = postUser.Address,
                City = postUser.City,
                ZipCode = postUser.ZipCode,
                Email = postUser.Email
            };
            var context = _contextFactory.CreateDbContext();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Delete a user by id",
            Description = "Delete a user by id",
            OperationId = "UserController.DeleteById",
            Tags = new []{"UserController"}
        )]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/email
        [HttpDelete("{email}/")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Delete a user by email",
            Description = "Delete a user by email",
            OperationId = "UserController.DeleteByEmail",
            Tags = new []{"UserController"}
        )]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Login")]
        [SwaggerOperation(
            Summary = "Login with email and password",
            Description = "Login with email and password",
            OperationId = "UserController.Login",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<UserWithToken>> Login([FromBody] User user)
        {
            var context = _contextFactory.CreateDbContext();
            user = await context.Users.Include(u => u.RefreshTokens)
                .Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefaultAsync();
            //user = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

            UserWithToken userWithToken = null;
            if (user != null)
            {
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await context.SaveChangesAsync();

                userWithToken = new UserWithToken(user) {RefreshToken = refreshToken.Token};
            }
            if (userWithToken == null)
            {
                return NotFound();
            }
            userWithToken.AccessToken = GenerateAccessToken(user.Id);
            return userWithToken;
        }
        
        // POST: api/Users
        [HttpPost("Register")]
        [SwaggerOperation(
            Summary = "Register a user with email and password",
            Description = "Register a user with email and password",
            OperationId = "UserController.Register",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<UserWithToken>> Register([FromBody] User user)
        {
            var context = _contextFactory.CreateDbContext();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            user = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            
            UserWithToken userWithToken = null;
            if(user != null)
            {
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await context.SaveChangesAsync();

                userWithToken = new UserWithToken(user) {RefreshToken = refreshToken.Token};
            }
            if (userWithToken == null)
            {
                return NotFound();
            }
            //sign your token here here..
            userWithToken.AccessToken = GenerateAccessToken(user.Id);
            return userWithToken;
        }

        [HttpPost("RefreshToken")]
        [SwaggerOperation(
            Summary = "Get a refreshtoken",
            Description = "Get a refreshtoken",
            OperationId = "UserController.RefreshToken",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<UserWithToken>> RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            var user = await GetUserFromAccessToken(refreshRequest.AccessToken);

            if (user == null || !ValidateRefreshToken(user, refreshRequest.RefreshToken)) return null;
            var userWithToken = new UserWithToken(user) {AccessToken = GenerateAccessToken(user.Id)};

            return userWithToken;

        }

        // GET: api/Users
        [HttpPost("GetUserByAccessToken")]
        [SwaggerOperation(
            Summary = "Get a user by accesstoken",
            Description = "Get a user by accesstoken",
            OperationId = "UserController.GetUserByAccessToken",
            Tags = new []{"UserController"}
        )]
        public async Task<ActionResult<User>> GetUserByAccessToken([FromBody] string accessToken)
        {
            var user = await GetUserFromAccessToken(accessToken);

            return user ?? null;
        }

        private bool ValidateRefreshToken(User user, string refreshToken)
        {
            var context = _contextFactory.CreateDbContext();
            var refreshTokenUser =  context.RefreshTokens.Where(rt => rt.Token == refreshToken)
                                                .OrderByDescending(rt => rt.ExpiryDate)
                                                .FirstOrDefault();

            return refreshTokenUser != null && refreshTokenUser.UserId == user.Id
                                            && refreshTokenUser.ExpiryDate > DateTime.UtcNow;
        }

        private async Task<User> GetUserFromAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.SecretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principle.FindFirst(ClaimTypes.Name)?.Value;
                    var context = _contextFactory.CreateDbContext();
                    return await context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));
                }
            }
            catch (Exception)
            {
                return new User();
            }

            return new User();
        }

        private static RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }

        private string GenerateAccessToken(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        private bool UserExists(Guid id)
        {
            var context = _contextFactory.CreateDbContext();
            return context.Users.Any(e => e.Id == id);
        }
    }
}
