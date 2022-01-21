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
        _users.Add(user);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    private void SetupOffer()
    {
        var now = DateTime.Now;

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
        _offers.Add(someOffer);
        _context.Offers.Add(someOffer);
        _context.SaveChanges();
    }

    private void SetupOrder()
    {
        // UserId and OfferId come from default values on QoQoContext
        var order = new Order
        {
            Status = OrderStatus.PENDING,
            UserId = 1,
            OfferId = 1
        };
        _orders.Add(order);
        _context.Orders.AddRange(_orders);
        _context.SaveChanges();
    }

    private void SetupClick()
    {
        // UserId and OfferId come from default values on QoQoContext
        var click = new Click
        {
            UserId = 1,
            OfferId = 1
        };
        _clicks.Add(click);
        _context.Clicks.AddRange(_clicks);
        _context.SaveChanges();
    }
}