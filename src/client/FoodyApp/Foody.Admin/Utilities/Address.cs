namespace Foody.Admin.Utilities;

public static class Address
{
    public static class Base
    {
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ?
            "https://10.0.2.2:7157" : "https://localhost:7157";
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

