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
            byte [] data = DataSource.GetBytes(resourceID);
            if(null != data && data.Length > 0 ) {                
                cereal = SKBitmap.Decode(data);
                //SKCodecResult result = codec.GetPixels(cereal.Info, cereal.GetPixels());
                //if (result == SKCodecResult.Success)
                //{
                //    return cereal;
                //}
                
                return cereal;
            }
            return null;
        }
        public static SKImage LoadImage(string resourceID)
        {

            SKBitmap bmp = LoadBitmap(resourceID);
            if (null == bmp) { bmp = LoadBitmap("Images/food/bagel.png"); }
            if (null == bmp) { return null; }
            return SKImage.FromBitmap(bmp);
        }        
    }
}
