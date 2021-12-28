using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class OfferClickDto
{
    public int Click { get; set; }
    public int RemainingTime { get; set; }
    public int UserId { get; set; }
}

public class OfferOrderDto
{
    public string Title { get; set; }
    public int Id { get; set; }
}