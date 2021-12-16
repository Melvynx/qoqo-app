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
public class UsersController : ControllerBase
{
    private readonly QoqoContext _context;

    public UsersController(QoqoContext qoqoContext)
    {
        _context = qoqoContext;
    }

    [HttpGet]
    public List<User> Get()
    {
        return _context.Users.ToList();
    }
}