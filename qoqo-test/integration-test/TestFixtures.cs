using System;
using System.Collections.Generic;
using qoqo.Model;

namespace qoqo_test.integration_test;

public class TestFixtures
{
    private readonly QoqoContext _context;

    private readonly List<User> _users = new();
    private readonly List<Offer> _offers = new();
    private readonly List<Order> _orders = new();
    private readonly List<Click> _clicks = new();

    public TestFixtures(QoqoContext context)
    {
        _context = context;
    }

    public void Setup()
    {
        SetupUser();
        SetupOffer();
        SetupOrder();
        SetupClick();
    }

    private void SetupUser()
    {
        var user = new User
        {
            UserName = "Jean",
            PasswordHash = "$2a$11$mttOYFwAIsxonQM.DWIMA.swdPep2o/ABHhUcycgbO68FOY9gpLZO", // 123456
            FirstName = "Jean",
            LastName = "Didier",
            Email = "jean@gmail.com",
            Street = "Small cat Street",
            City = "Gland",
            Npa = 1900
        };
        var adminUser = new User
        {
            UserName = "Admin",
            PasswordHash = "$2a$11$ss.H3F2abvWfm6TnZAgKT.Lu6ihbxJbL/Khqlyzi2V/bFtbztzItC", // abcdef
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@gmail.com",
            Street = "Admin street",
            City = "Admin city",
            Npa = 1700
        };
        _users.Add(user);
        _users.Add(adminUser);
        _context.Users.AddRange(_users);
        _context.SaveChanges();
    }

    private void SetupOffer()
    {
        var now = DateTime.Now;
        var liveOffer = new Offer
        {
            Title = "Offer 1",
            Description = "Offer 1 description",
            StartAt = now.AddDays(-2),
            EndAt = now.AddDays(2),
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 100,
            SpecificationText = "Specification text",
            ImageUrl = "https://img.com",
            IsOver = false,
            IsDraft = false
        };

        var someOffer = new Offer
        {
            Title = "Offer 2",
            Description = "Offer 2 description",
            StartAt = now.AddDays(4),
            EndAt = now.AddDays(6),
            BarredPrice = 100,
            Price = 0,
            ClickObjective = 100,
            SpecificationText = "Specification text",
            ImageUrl = "https://img.com",
            IsOver = false,
            IsDraft = false
        };
        _offers.Add(liveOffer);
        _offers.Add(someOffer);
        _context.Offers.AddRange(_offers);
        _context.SaveChanges();
    }

    private void SetupOrder()
    {
        var order = new Order
        {
            Status = OrderStatus.PENDING,
            UserId = _users[0].UserId,
            OfferId = _offers[0].OfferId
        };
        _orders.Add(order);
        _context.Orders.AddRange(_orders);
        _context.SaveChanges();
    }

    private void SetupClick()
    {
        var click = new Click
        {
            UserId = _users[0].UserId,
            OfferId = _orders[0].OfferId
        };
        _clicks.Add(click);
        _context.Clicks.AddRange(_clicks);
        _context.SaveChanges();
    }
}