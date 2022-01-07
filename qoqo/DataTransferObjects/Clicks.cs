using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class ClickDto
{
    public int ClickCount { get; set; }
    public int ClickObjective { get; set; }
    public UserClickDto User { get; set; }

    public static ClickDto FromUserClick(UserClickDto userClickDto, int clickCount, int clickObjective)
    {
        return new ClickDto
        {
            User = userClickDto,
            ClickCount = clickCount,
            ClickObjective = clickObjective
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
    public ClickEventResult(bool confetti)
    {
        Confetti = confetti;
    }

    public bool Confetti { get; set; }
}

public class ClickEventFinishResult
{
    public ClickEventFinishResult(int userId, string userName, string finishSentence, int count)
    {
        UserId = userId;
        UserName = userName;
        FinishSentence = finishSentence;
        ClickCount = count;
    }

    public int UserId { get; set; }
    public string UserName { get; set; }
    public string FinishSentence { get; set; }
    public int ClickCount { get; set; }
}