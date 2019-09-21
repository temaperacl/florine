using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    class ResourceLoader
    {
        public static ISkiaSharpFlorineDataSource DataSource = null;
        public static SKBitmap LoadBitmap(string resourceID)
        {
            if (null == DataSource) { return null; }
            SKBitmap cereal;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(ResourceLoader));
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                cereal = SKBitmap.Decode(DataSource.GetStream(resourceID));
            }
            return cereal;
        }
        public static SKImage LoadImage(string resourceID)
        {
            return SKImage.FromBitmap(LoadBitmap(resourceID));
        }
    }
}
