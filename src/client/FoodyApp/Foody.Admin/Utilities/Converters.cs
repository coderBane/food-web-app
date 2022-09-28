using System.Globalization;
using System.Diagnostics.CodeAnalysis;


namespace Foody.Admin.Utilities
{
    public class Base64ImageConverter : IValueConverter
    {
        [return: NotNullIfNotNull("value")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = value as string;

            if (image is null) return null;

            var array = System.Convert.FromBase64String(image);

            return ImageSource.FromStream(() => new MemoryStream(array)); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolObjectConverter : IValueConverter
    {
        public object TrueObject { get; set; }

        public object FalseObject { get; set; }

        [return: NotNullIfNotNull("value")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value is true ? TrueObject : FalseObject;        
            
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

