using System.Globalization;
using System.Diagnostics.CodeAnalysis;


namespace Foody.Admin.Utilities
{
    public class Base64ImageConverter : IValueConverter
    {
        [return: NotNullIfNotNull("value")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = System.Convert.FromBase64String(value as string);

            if (array is null) return null;

            return ImageSource.FromStream(() => new MemoryStream(array)); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

