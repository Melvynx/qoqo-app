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
}