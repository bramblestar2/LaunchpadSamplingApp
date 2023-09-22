using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadSamplingApp.Helpers
{
    public class BitmapStringConverter : IValueConverter
    {
        public static readonly BitmapStringConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {

            if (value is string path)
            {
                if (targetType.IsAssignableTo(typeof(IImage)))
                {
                    if (File.Exists(path))
                    {
                        return new Bitmap(path);
                    }
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
