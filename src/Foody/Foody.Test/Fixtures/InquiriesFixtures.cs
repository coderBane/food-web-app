namespace Foody.Test.Fixtures
{
    public static class InquiriesFixtures
    {
        public static List<Contact> GetContacts() =>
            Enumerable.Range(211, 218).Select(_ =>
            {
                var name = Faker.Name.FullName();

                return new Contact
                {
                    Id = _,
                    Name = name,
                    Email = Faker.Internet.Email(name),
                    Subject = Faker.Lorem.Sentence(10),
                    Message = string.Join(Environment.NewLine, Faker.Lorem.Sentences(8)),
                    Date = Faker.Identification.DateOfBirth()
                };
            }).ToList();

        public static ContactDto ValidContact() =>
            new()
            {
                Name = "Steve Mana",
                Email = Faker.Internet.Email("Steve Mana"),
                Subject = Faker.Lorem.Sentence(5),
                Message = string.Join(Environment.NewLine, Faker.Lorem.Sentences(8))
            };
    }
}

