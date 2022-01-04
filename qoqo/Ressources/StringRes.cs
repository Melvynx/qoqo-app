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
    public const string Logout = "Logout";
    public const string LogoutFailed = "Logout failed";
    public const string LoginFailed = "Invalid username or password";

    // ClickController
    public const string ClickMinimum10Seconds = "You can't click this offer more than once every 10 seconds";
    public const string OfferNotFound = "Offer not found";
    public const string NeedToBeLoggedToClick = "You can't click this offer if you are not logged in";
    
    // OfferController
    public const string ErrorDuringOfferCreation = "Error during offer creation";
    public const string ErrorDuringOfferUpdate = "Error during offer update";
    public const string OfferUpdated = "Offer updated";
    public const string OfferCreated = "Offer created";
    public const string NoCurrentOffer = "No current offer";
    
    // OfferProvider
    public const string OfferStartAtDateRequired = "Offer start at date is required";
    public const string OfferEndAtDateRequired = "Offer start at date is required";
    public const string OfferEndAtBeforeStartAt = "Offer end at date must be after start at date";
    public const string OfferClickObjectiveMustBeUpperThan10 = "Offer click objective must be upper than 10";

    public const string OfferSameTime = "Offer already exist at this time";
    
    // OrderProvider
    public const string OrderUpdated = "Order updated";
    public const string ErrorDuringOrderUpdate = "Error during order update";

}