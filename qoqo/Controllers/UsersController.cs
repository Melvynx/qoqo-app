using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qoqo.Model;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly QoqoContext _context;

    public UserController(QoqoContext qoqoContext)
    {
        _context = qoqoContext;
    }

    [HttpGet]
    public List<User> Get()
    {
        /*var user = new User { UserId = 1, Firstname = "John", Lastname = "Doe", Usernamne = "Jojo", PasswordHash = "abcdefghijklmnopqrstuvwxyz" };
        _context.Users.Add(user);*/
        return _context.Users.ToList();
    }
}