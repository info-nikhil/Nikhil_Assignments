using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineResortBooking.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UserService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ================= REGISTER =================
        public async Task<User> RegisterUserAsync(User user)
        {
            if (user == null)
                throw new Exception("User data is required");

            // Check existing user
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
                throw new Exception("User already exists with this email");

            // Hash password
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password is required");

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Normalize role
            if (string.Equals(user.UserRole, "Admin", StringComparison.OrdinalIgnoreCase))
                user.UserRole = "Admin";
            else
                user.UserRole = "Customer";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // ================= LOGIN HELP =================
        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email is required");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        // ================= VERIFY PASSWORD =================
        public bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        // ================= JWT TOKEN =================
        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            if (user == null)
                throw new Exception("User is required for token generation");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole),
                new Claim("UserId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ================= GET ALL USERS =================
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // ================= GET USER BY ID =================
        public async Task<User> GetUserByIdAsync(long userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }
    }
}