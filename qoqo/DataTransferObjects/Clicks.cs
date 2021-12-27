using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class ClickDto
{
    public int ClickCount { get; set; }
    public UserClickDto User { get; set; }

    public static ClickDto FromUserClick(UserClickDto userClickDto, int clickCount)
    {
        return new ClickDto
        {
            User = userClickDto,
            ClickCount = clickCount
        };
    }
}

public class UserClickDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    
    public static UserClickDto FromUser(User user)
    {
        return new UserClickDto
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }
}

public class UserClick
{
    public int Count { get; set; }
    public int OfferId { get; set; }
    public string OfferTitle { get; set; }
}

public class ClickOrderDto
{
    public DateTime CreatedAt { get; set; }
    public int ClickCount { get; set; }
}