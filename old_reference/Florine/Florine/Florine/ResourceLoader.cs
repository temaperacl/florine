using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace Florine
{
    class ResourceLoader
    {
        public static System.IO.StreamReader LoadTextFile(string resourceID)
        {
            System.IO.StreamReader sr;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(ResourceLoader));
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                sr = new System.IO.StreamReader(assembly.GetManifestResourceStream(resourceID));
            }
            return sr;
        }
        public static SKBitmap LoadBitmap(string resourceID)
        {
            SKBitmap cereal;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(ResourceLoader));
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                cereal = SKBitmap.Decode(stream);
            }
            return cereal;
        }
        public static SKImage LoadImage(string resourceID)
        {
            return SKImage.FromBitmap(LoadBitmap(resourceID));
        }
    }
}
