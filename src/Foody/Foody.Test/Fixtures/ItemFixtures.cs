using Microsoft.AspNetCore.Http;

namespace Foody.Test.Fixtures
{
    public static class ItemFixtures
    {
        public static Category GetCategory() => new() { Id = 201, Name = "Sweets" };

        public static FormFile InvalidFileType()
        {
            string fileName = "test.txt";
            if (!File.Exists(fileName))
                File.Create(fileName);

            var stream = File.OpenRead(fileName);
            var file = new FormFile(stream, 0, stream.Length, "ImageUpload", Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain",
                ContentDisposition = "form-data"
            };

            return file;
        }
    }
}

