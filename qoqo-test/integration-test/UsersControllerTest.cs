using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Services;
using Xunit;

namespace qoqo_test.integration_test;

public class UsersControllerTest : IClassFixture<IntegrationFixtures>
{
    private readonly IntegrationFixtures _fixtures;

    public UsersControllerTest(IntegrationFixtures fixtures)
    {
        _fixtures = fixtures;
    }

    [Fact]
    public async Task GetUsers()
    {
        var client = _fixtures.Setup();

        var response = await client.GetAsync("api/users");
        response.EnsureSuccessStatusCode();

        var users = TestHelpers.GetBody<List<User>>(response);
        await using var context = _fixtures.Context;

        var count = context.Users.Count();
        Assert.Equal(users?.Count, count);
    }

    [Fact]
    public async Task GetUserIdWithInvalidEmail()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client);

        const string testString = "TEST";
        var response = await client.PutAsJsonAsync("api/users/1", new UserDto
        {
            UserName = "a",
            Email = "a",
            FirstName = testString,
            LastName = testString,
            AvatarUrl = testString
        });

        var error = TestHelpers.GetBody<UserErrorDto>(response);

        Assert.NotNull(error?.Email);
        Assert.NotNull(error?.UserName);
    }

    [Fact]
    public async Task GetUserIdWithInvalidAlreadyExistUsername()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client);

        await using var context = _fixtures.Context;
        var usernameExist = await context.Users
            .Where(u => u.UserId != 1)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();

        const string testString = "TEST";
        var response = await client.PutAsJsonAsync("api/users/1", new UserDto
        {
            UserName = usernameExist ?? "",
            Email = "valid@gmail.com",
            FirstName = testString,
            LastName = testString,
            AvatarUrl = testString
        });

        var error = TestHelpers.GetBody<UserErrorDto>(response);

        Assert.Null(error?.Email);
        Assert.NotNull(error?.UserName);
    }

    [Fact]
    public async Task GetUserIdValid()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client);

        var userDto = new UserDto
        {
            UserName = "Test",
            Email = "test@gmail.com",
            FirstName = "test_firstname",
            LastName = "test_lastname",
            AvatarUrl = "https://img.com"
        };

        var response = await client.PutAsJsonAsync("api/users/1", userDto);
        response.EnsureSuccessStatusCode();
        var user = TestHelpers.GetBody<UserDto>(response);

        Assert.Equal(user?.UserName, userDto.UserName);
        Assert.Equal(user?.Email, userDto.Email);
        Assert.Equal(user?.FirstName, userDto.FirstName);
        Assert.Equal(user?.LastName, userDto.LastName);
        Assert.Equal(user?.AvatarUrl, userDto.AvatarUrl);
    }

    [Fact]
    public async Task GetMe()
    {
        var client = _fixtures.Setup();
        _fixtures.Authenticate(client);

        var response = await client.GetAsync("api/users/me");
        response.EnsureSuccessStatusCode();

        var user = TestHelpers.GetBody<UserDto>(response);

        Assert.Equal(1, user?.UserId);
    }

    [Fact]
    public async Task GetMeWithoutAuth()
    {
        var client = _fixtures.Setup();

        var response = await client.GetAsync("api/users/me");

        var message = TestHelpers.GetBody<RequestMessage>(response);

        Assert.NotNull(message?.Message);
    }

    [Fact]
    public async Task LoginWithNoValidValues()
    {
        var client = _fixtures.Setup();

        var loginDto = new LoginDto
        {
            UserName = "test",
            Password = "test"
        };

        var response = await client.PostAsJsonAsync("api/users/login", loginDto);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LoginWithValues()
    {
        var client = _fixtures.Setup();

        var loginDto = new LoginDto
        {
            UserName = "Jean",
            Password = "123456"
        };

        var response = await client.PostAsJsonAsync("api/users/login", loginDto);
        response.EnsureSuccessStatusCode();

        var user = TestHelpers.GetBody<UserDto>(response);

        Assert.Equal(2, user?.UserId);
    }

    [Fact]
    public async Task RegisterWrongPassword()
    {
        var client = _fixtures.Setup();

        var registerDto = new RegisterDto
        {
            UserName = "Didier",
            Email = "didier@gmail.com",
            FirstName = "Didier",
            LastName = "Dupont",
            Password = "123456"
        };

        var response = await client.PostAsJsonAsync("api/users/register", registerDto);

        var errorDto = TestHelpers.GetBody<UserErrorDto>(response);

        Assert.NotNull(errorDto?.Password);
    }

    [Fact]
    public async Task RegisterUserNameExist()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.GetContext();

        var usernameExist = await context.Users
            .Select(u => u.UserName)
            .FirstAsync();

        var registerDto = new RegisterDto
        {
            UserName = usernameExist,
            Email = "didier@gmail.com",
            FirstName = "Didier",
            LastName = "Dupont",
            Password = "BigPassword1234"
        };

        var response = await client.PostAsJsonAsync("api/users/register", registerDto);

        var errorDto = TestHelpers.GetBody<UserErrorDto>(response);

        Assert.NotNull(errorDto?.UserName);
    }


    [Fact]
    public async Task RegisterEmailExist()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.GetContext();

        var emailExist = await context.Users
            .Select(u => u.Email)
            .FirstAsync();

        var registerDto = new RegisterDto
        {
            UserName = "didierrr",
            Email = emailExist,
            FirstName = "Didier",
            LastName = "Dupont",
            Password = "BigPassword1234"
        };

        var response = await client.PostAsJsonAsync("api/users/register", registerDto);

        var errorDto = TestHelpers.GetBody<UserErrorDto>(response);

        Assert.NotNull(errorDto?.Email);
    }

    [Fact]
    public async Task Register()
    {
        var client = _fixtures.Setup();

        await using var context = _fixtures.Context;
        var count = context.Users.Count();

        var registerDto = new RegisterDto
        {
            UserName = "Didier",
            Email = "didier@gmail.com",
            FirstName = "Didier",
            LastName = "Dupont",
            Password = "123456ABcd"
        };

        var response = await client.PostAsJsonAsync("api/users/register", registerDto);

        var errorDto = TestHelpers.GetBody<UserDto>(response);

        Assert.Equal(errorDto?.UserName, registerDto.UserName);
        Assert.Equal(errorDto?.Email, registerDto.Email);
        Assert.Equal(errorDto?.FirstName, registerDto.FirstName);
        Assert.Equal(errorDto?.LastName, registerDto.LastName);

        var countAfter = context.Users.Count();
        Assert.Equal(count + 1, countAfter);
    }
}