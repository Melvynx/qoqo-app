using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Ressources;

namespace qoqo.Providers;

public class UserProvider
{
    private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,1000}$");
    private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_]{3,30}$");
    private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
    private readonly QoqoContext _context;

    public UserProvider(QoqoContext context)
    {
        _context = context;
    }
    
    // get user from userId
    public async Task<User?> GetUser(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto, int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null) return new BadRequestObjectResult(new {message = "User not found."});

        var errors = new UserErrorDto
        {
            UserName = CheckUserName(userDto.UserName, userId),
            Email = CheckEmail(userDto.Email, userId)
        };

        if (!errors.IsValid()) return new BadRequestObjectResult(errors);

        user.UserName = userDto.UserName;
        user.Email = userDto.Email;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.AvatarUrl = userDto.AvatarUrl;

        await _context.SaveChangesAsync();
        return UserDto.FromUser(user);
    }

    public async Task<bool> Logout(string value)
    {
        var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Value == value);
        if (token == null) return false;

        token.ExpiredAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    // use bcrypt to hash password
    // https://jasonwatmore.com/post/2020/07/16/aspnet-core-3-hash-and-verify-passwords-with-bcrypt
    public async Task<UserDto?> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .Where(u => u.UserName == loginDto.UserName)
            .FirstOrDefaultAsync();

        if (user == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) return null;

        var userDto = UserDto.FromUser(user);

        userDto.Token = await GenerateToken(user.Id);
        return userDto;
    }

    public async Task<ActionResult<UserDto?>> Register(RegisterDto registerDto)
    {
        var userError = ValidateUser(registerDto);

        if (!userError.IsValid()) return new BadRequestObjectResult(userError);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var user = new User
        {
            UserName = registerDto.UserName,
            PasswordHash = passwordHash,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            Street = registerDto.Street,
            City = registerDto.City,
            Npa = registerDto.Npa
        };

        var newUser = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var userDto = UserDto.FromUser(newUser.Entity);
        userDto.Token = await GenerateToken(userDto.UserId);
        return userDto;
    }

    private async Task<string> GenerateToken(int userId)
    {
        var token = await _context.Tokens.AddAsync(Token.GenerateToken(userId));
        await _context.SaveChangesAsync();
        return token.Entity.Value;
    }

    private UserErrorDto ValidateUser(RegisterDto registerDto)
    {
        var errors = new UserErrorDto
        {
            UserName = CheckUserName(registerDto.UserName),
            Email = CheckEmail(registerDto.Email),
            Password = CheckPassword(registerDto.Password)
        };

        return errors;
    }

    private string? CheckUserName(string? userName, int? userId = null)
    {
        if (userName == null) return StringRes.UserNameRequired;

        if (_context.Users.Any(u => u.UserName == userName && u.UserId != userId))
            return StringRes.UsernameAlreadyExist;

        return !UsernameRegex.IsMatch(userName) ? StringRes.UserNameRegexError : null;
    }

    private static string? CheckPassword(string? password)
    {
        if (password == null) return StringRes.PasswordRequired;

        return !PasswordRegex.IsMatch(password) ? StringRes.PasswordRegexError : null;
    }

    private string? CheckEmail(string? email, int? userId = null)
    {
        if (email == null) return StringRes.EmailRequired;

        if (_context.Users.Any(u => u.Email == email && u.UserId != userId)) return StringRes.EmailAlreadyExist;

        return !EmailRegex.IsMatch(email) ? StringRes.EmailRegexError : null;
    }
}