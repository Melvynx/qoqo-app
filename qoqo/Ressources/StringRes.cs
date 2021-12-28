namespace qoqo.Ressources;

public class StringRes
{
    // UserProvider
    public const string PasswordRegexError =
        "Password must be at least 8 characters and contain at least one lowercase letter, one uppercase letter and one number.";
    public const string UserNameRegexError =
        "Username must be between 3 and 30 characters and only contain letters, numbers and underscores.";
    public const string EmailRegexError = "Email must be a valid email address.";
    public const string UsernameAlreadyExist = "Username already exist.";
    public const string EmailAlreadyExist = "Email already exist.";
    public const string UserNameRequired = "Username is required.";
    public const string EmailRequired = "Email is required.";
    public const string PasswordRequired = "Password is required.";

    // ClickController
    public const string ClickMinimum10Seconds = "You can't click this offer more than once every 10 seconds";
    public const string OfferNotFound = "Offer not found";
    public const string NeedToBeLoggedToClick = "You can't click this offer if you are not logged in";
}