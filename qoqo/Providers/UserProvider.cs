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

    public async Task<User?> Login(LoginDto loginDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
        var query = _context.Users
            .Where(u => u.UserName == loginDto.Username && u.PasswordHash == passwordHash);

        return await query.FirstOrDefaultAsync();
    }

    private static string HashPassword(string password)
    {
        var salt = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetNonZeroBytes(salt);
        }

        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
}