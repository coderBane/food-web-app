namespace Foody.Admin.Utilities;

public static class Address
{
    public static class Base
    {
        public const string Port = "7157";
        public const string Scheme = "https";
        public static readonly string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ?
            $"{Scheme}://10.0.2.2:{Port}" : $"{Scheme}://localhost:{Port}";
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

