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

public class ClickEventResult
{
    public bool Confetti { get; set; }

    public ClickEventResult(bool confetti)
    {
        Confetti = confetti;
    }
}

public class ClickEventFinishResult
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string FinishSentence { get; set; }
    public int ClickCount { get; set; }
    
    public ClickEventFinishResult(int userId, string userName, string finishSentence, int count)
    {
        UserId = userId;
        UserName = userName;
        FinishSentence = finishSentence;
        ClickCount = count;
    }
}