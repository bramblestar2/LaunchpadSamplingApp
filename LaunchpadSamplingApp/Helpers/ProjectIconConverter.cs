using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
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
    public class ProjectIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string path)
            {
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    if (Path.GetFileNameWithoutExtension(file) == "icon")
                    {
                        string ext = Path.GetExtension(file);
                        return new Bitmap(path + "\\icon" + ext);
                    }
                }
                //if (parameter is string ext)
                //{
                //    if (File.Exists(path + "\\icon" + ext))
                //    {
                //
                //        return new Bitmap(path + "\\icon" + ext);
                //    }
                //}
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
