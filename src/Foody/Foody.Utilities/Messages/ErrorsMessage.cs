namespace Foody.Utilities.Messages;

public static class ErrorsMessage
{
    public static class Generic
    {
        public const string BadRequest = "Bad Request";
        public const string NotFound = "Not found";
        public const string NullSet = "Entity set is null";
        public const string InvalidRequest = "Invalid Request";
        public const string AddFailure = "Failed to Add Entity";
        public const string ValidationError = "Validation Error";
        public const string UnknownError = "Something went wrong. Try again.";
    }

    public static class Category
    {
        public const string NotExist = "Category does not exist";
        public const string Exists = "Category with given name already in database";
    }

    public static class Newsletter
    {
        public const string NotExist = "Email is not part of subscribtion list";
    }
}

