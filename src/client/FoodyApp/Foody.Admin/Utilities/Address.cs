namespace Foody.Admin.Utilities;

public static class Address
{
    public static class Base
    {
        public const string BaseAddress = "https://localhost:7157";
    }

    public static class Account
    {
        public const string BaseAddress = "/v1/Account";
        public const string LoginAddress = BaseAddress + "/Login";
    }

    public static class Category
    {
        public const string BaseAddress = "/v1/Category";
    }
}

