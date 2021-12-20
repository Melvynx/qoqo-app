using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;

namespace qoqo.Providers;

public class UserProvider
{
    private readonly QoqoContext _context;

    public UserProvider(QoqoContext context)
    {
        _context = context;
    }
    
    // use bcrypt to hash password
    // https://jasonwatmore.com/post/2020/07/16/aspnet-core-3-hash-and-verify-passwords-with-bcrypt
    public async Task<User?> Login(LoginDto loginDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
        var query = _context.Users
            .Where(u => u.UserName == loginDto.Username && u.PasswordHash == passwordHash);

        return await query.FirstOrDefaultAsync();
    }
}