namespace Foody.Test.Fixtures
{
    public static class Subcribers
    {
        public static IEnumerable<Newsletter> GetSubcribers() =>
            new List<Newsletter>()
            {
                new()
                {
                    Id = 211,
                    Name = "Jack",
                    Email = "jackma@example.com"
                },
                new()
                {
                    Id = 213,
                    Name = "Mary",
                    Email = "mary1998@example.com"
                },
                new()
                {
                    Id = 215,
                    Name = "Lohun",
                    Email = "lohun@example.com"
                }
            };
    }
}

